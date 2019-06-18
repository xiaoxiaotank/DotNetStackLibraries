using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace AspNetCore.Authentication.Digest
{
    public class DigestHandler : AuthenticationHandler<DigestOptions>
    {
        public DigestHandler(
            IOptionsMonitor<DigestOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
        }

        protected new DigestEvents Events
        {
            get => (DigestEvents)base.Events;
            set => base.Events = value;
        }

        /// <summary>
        /// 确保创建的 Event 类型是 DigestEvents
        /// </summary>
        /// <returns></returns>
        protected override Task<object> CreateEventsAsync() => Task.FromResult<object>(new DigestEvents());

        protected async override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var authorizationHeader = GetAuthenticationHeader(Context.Request);
            if (authorizationHeader == null)
            {
                return AuthenticateResult.NoResult();
            }

            try
            {
                var validateCredentialsContext = new ValidateCredentialsContext(Context, Scheme, Options)
                {
                    UserName = authorizationHeader.UserName
                };
                var password = await Events.ValidateCredentials(validateCredentialsContext);

                string computedResponse = string.Empty;
                string hashA1, hashA2;
                switch (authorizationHeader.Qop)
                {
                    case QopValues.Auth:
                        hashA1 = HashHelper.HashByMD5($"{authorizationHeader.UserName}:{authorizationHeader.Realm}:{password}");
                        hashA2 = HashHelper.HashByMD5($"{authorizationHeader.RequestMethod}:{authorizationHeader.URI}");
                        computedResponse = HashHelper.HashByMD5($"{hashA1}:{authorizationHeader.Nonce}:{authorizationHeader.NC}:{authorizationHeader.CNonce}:{authorizationHeader.Qop}:{hashA2}");
                        break;
                    case QopValues.AuthInt:
                        hashA1 = HashHelper.HashByMD5($"{HashHelper.HashByMD5($"{authorizationHeader.UserName}:{authorizationHeader.Realm}:{password}")}:{authorizationHeader.Nonce}:{authorizationHeader.CNonce}");
                        hashA2 = HashHelper.HashByMD5($"{authorizationHeader.RequestMethod}:{authorizationHeader.URI}:{HashHelper.HashByMD5(Context.Request.Body.ToString())}");
                        computedResponse = HashHelper.HashByMD5($"{hashA1}:{authorizationHeader.Nonce}:{authorizationHeader.NC}:{authorizationHeader.CNonce}:{authorizationHeader.Qop}:{hashA2}");
                        break;
                    default:
                        return AuthenticateResult.Fail("qop指定策略无效!必须为\"auth\"或\"auth-int\"");
                }

                if (computedResponse == authorizationHeader.Response)
                {
                    var claim = new Claim(ClaimTypes.Name, validateCredentialsContext.UserName);
                    var identity = new ClaimsIdentity(DigestDefaults.AuthenticationScheme);
                    identity.AddClaim(claim);

                    var ticket = new AuthenticationTicket(new ClaimsPrincipal(identity), Scheme.Name);
                    return AuthenticateResult.Success(ticket);
                }

                return AuthenticateResult.NoResult();
            }
            catch (Exception ex)
            {
                return AuthenticateResult.Fail(ex.Message);
            }

        }

        protected override async Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            var authResult = await HandleAuthenticateOnceSafeAsync();
            var challengeContext = new DigestChallengeContext(Context, Scheme, Options, properties)
            {
                AuthenticateFailure = authResult?.Failure
            };
            await Events.Challenge(challengeContext);
            //质询已处理
            if (challengeContext.Handled) return;

            var challengeValue = GetChallengeValue();
            var error = challengeContext.AuthenticateFailure?.Message;
            if (!string.IsNullOrWhiteSpace(error))
            {
                //将错误信息封装到内部
                challengeValue += $",error=\"{ error }\"";
            }

            Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            Response.Headers.Append(HeaderNames.WWWAuthenticate, challengeValue);
        }

        private AuthorizationHeader GetAuthenticationHeader(HttpRequest request)
        {
            try
            {
                var credentials = GetCredentials(request);
                if (credentials != null)
                {
                    var authorizationHeader = new AuthorizationHeader()
                    {
                        RequestMethod = request.Method,
                    };
                    var nameValueStrs = credentials.Replace("\"", string.Empty).Split(',', StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim());
                    foreach (var nameValueStr in nameValueStrs)
                    {
                        var index = nameValueStr.IndexOf('=');
                        var name = nameValueStr.Substring(0, index);
                        var value = nameValueStr.Substring(index + 1);

                        switch (name)
                        {
                            case AuthenticateHeaderNames.UserName:
                                authorizationHeader.UserName = value;
                                break;
                            case AuthenticateHeaderNames.Realm:
                                authorizationHeader.Realm = value;
                                break;
                            case AuthenticateHeaderNames.Nonce:
                                authorizationHeader.Nonce = value;
                                break;
                            case AuthenticateHeaderNames.CNonce:
                                authorizationHeader.CNonce = value;
                                break;
                            case AuthenticateHeaderNames.NC:
                                authorizationHeader.NC = value;
                                break;
                            case AuthenticateHeaderNames.Qop:
                                authorizationHeader.Qop = value;
                                break;
                            case AuthenticateHeaderNames.Response:
                                authorizationHeader.Response = value;
                                break;
                            case AuthenticateHeaderNames.URI:
                                authorizationHeader.URI = value;
                                break;
                        }
                    }

                    return authorizationHeader;
                }
            }
            catch { }

            return null;
        }

        private string GetCredentials(HttpRequest request)
        {
            string credentials = null;

            string authorization = request.Headers[HeaderNames.Authorization];
            //请求中存在 Authorization 标头且认证方式为 Digest
            if (authorization?.StartsWith(DigestDefaults.AuthenticationScheme, StringComparison.OrdinalIgnoreCase) == true)
            {
                credentials = authorization.Substring(DigestDefaults.AuthenticationScheme.Length).Trim();
            }

            return credentials;
        }

        private string GetChallengeValue()
            => $"{ DigestDefaults.AuthenticationScheme } realm=\"{ Options.Realm }\",qop=\"{ Options.Qop }\",nonce=\"{ Options.Nonce ?? GetRandomNumber(Context.Request.Headers[HeaderNames.ETag])}\"";

        private string GetRandomNumber(string eTag)
        {
            var privateKey = "Test private key";
            var dateTimeStr = DateTimeOffset.UtcNow.ToString();
            return Convert.ToBase64String(Encoding.Default.GetBytes($"{ dateTimeStr } { HashHelper.HashByMD5($"{dateTimeStr} :{eTag} : {privateKey}") }"));
        }

    }
}
