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
        private static readonly Encoding _encoding = Encoding.UTF8;

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
        protected override Task<object> CreateEventsAsync() => throw new NotImplementedException($"{nameof(Events)} must be created");

        protected async override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var authorizationHeader = GetAuthenticationHeader(Context.Request);
            if (authorizationHeader == null)
            {
                return AuthenticateResult.NoResult();
            }

            try
            {
                var isValid = ValidateNonce(authorizationHeader.Nonce);
                //随机数过期
                if (isValid == null)
                {
                    var properties = new AuthenticationProperties();
                    properties.SetParameter(AuthenticationHeaderNames.Stale, true);
                    return AuthenticateResult.Fail(string.Empty, properties);
                }
                else if (isValid == true)
                {
                    var getPasswordContext = new GetPasswordContext(Context, Scheme, Options)
                    {
                        UserName = authorizationHeader.UserName
                    };
                    var password = await Events.GetPassword(getPasswordContext);
                    string computedResponse = null;
                    switch (authorizationHeader.Qop)
                    {
                        case QopValues.Auth:
                            computedResponse = GetComputedResponse(authorizationHeader, password);
                            break;
                        default:
                            return AuthenticateResult.Fail($"qop指定策略必须为\"{QopValues.Auth}\"");
                    }

                    if (computedResponse == authorizationHeader.Response)
                    {
                        var claim = new Claim(ClaimTypes.Name, getPasswordContext.UserName);
                        var identity = new ClaimsIdentity(DigestDefaults.AuthenticationScheme);
                        identity.AddClaim(claim);

                        var ticket = new AuthenticationTicket(new ClaimsPrincipal(identity), Scheme.Name);
                        AddAuthorizationInfo(Context.Response, authorizationHeader, password);
                        return AuthenticateResult.Success(ticket);
                    }
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
                AuthenticateFailure = authResult.Failure,
                Stale = authResult.Properties?.GetParameter<bool>(AuthenticationHeaderNames.Stale) ?? false
            };
            await Events.Challenge(challengeContext);
            //质询已处理
            if (challengeContext.Handled) return;

            var challengeValue = GetChallengeValue(challengeContext.Stale);
            var error = challengeContext.AuthenticateFailure?.Message;
            if (!string.IsNullOrWhiteSpace(error))
            {
                //将错误信息封装到内部
                challengeValue += $", error=\"{ error }\"";
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
                            case AuthenticationHeaderNames.UserName:
                                authorizationHeader.UserName = value;
                                break;
                            case AuthenticationHeaderNames.Realm:
                                authorizationHeader.Realm = value;
                                break;
                            case AuthenticationHeaderNames.Nonce:
                                authorizationHeader.Nonce = value;
                                break;
                            case AuthenticationHeaderNames.ClientNonce:
                                authorizationHeader.ClientNonce = value;
                                break;
                            case AuthenticationHeaderNames.NonceCounter:
                                authorizationHeader.NonceCounter = value;
                                break;
                            case AuthenticationHeaderNames.Qop:
                                authorizationHeader.Qop = value;
                                break;
                            case AuthenticationHeaderNames.Response:
                                authorizationHeader.Response = value;
                                break;
                            case AuthenticationHeaderNames.Uri:
                                authorizationHeader.Uri = value;
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

        /// <summary>
        /// 验证Nonce是否有效
        /// </summary>
        /// <param name="nonce"></param>
        /// <returns>true:验证通过;false:验证失败;null:随机数过期</returns>
        private bool? ValidateNonce(string nonce)
        {
            try
            {
                var plainNonce = _encoding.GetString(Convert.FromBase64String(nonce));
                var timestamp = DateTimeOffset.Parse(plainNonce.Substring(0, plainNonce.LastIndexOf(' ')));
                //验证Nonce是否被篡改
                var isValid = nonce == GetNonce(timestamp);

                //验证是否过期
                if (Math.Abs((timestamp - DateTimeOffset.UtcNow).TotalSeconds) > Options.MaxNonceAgeSeconds)
                {
                    return isValid ? (bool?)null : false;
                }

                return isValid;
            }
            catch
            {
                return false;
            }
        }

        private static string GetComputedResponse(AuthorizationHeader authorizationHeader, string password)
        {
            var a1Hash = $"{authorizationHeader.UserName}:{authorizationHeader.Realm}:{password}".ToMD5Hash();
            var a2Hash = $"{authorizationHeader.RequestMethod}:{authorizationHeader.Uri}".ToMD5Hash();
            return $"{a1Hash}:{authorizationHeader.Nonce}:{authorizationHeader.NonceCounter}:{authorizationHeader.ClientNonce}:{authorizationHeader.Qop}:{a2Hash}".ToMD5Hash();
        }


        private void AddAuthorizationInfo(HttpResponse response, AuthorizationHeader authorizationHeader, string password)
        {
            var partList = new List<ValueTuple<string, string, bool>>()
            {
                (AuthenticationHeaderNames.Qop, authorizationHeader.Qop, true),
                (AuthenticationHeaderNames.RspAuth, GetRspAuth(authorizationHeader, password), true),
                (AuthenticationHeaderNames.ClientNonce, authorizationHeader.ClientNonce, true),
                (AuthenticationHeaderNames.NonceCounter, authorizationHeader.NonceCounter, false)
            };
            response.Headers.Append("Authorization-Info", string.Join(", ", partList.Select(part => FormatHeaderPart(part))));
        }

        private string GetChallengeValue(bool stale)
        {
            var partList = new List<ValueTuple<string, string, bool>>()
            {
                (AuthenticationHeaderNames.Realm, Options.Realm, true),
                (AuthenticationHeaderNames.Qop, Options.Qop, true),
                (AuthenticationHeaderNames.Nonce, GetNonce(), true),
            };

            var value = $"{DigestDefaults.AuthenticationScheme} {string.Join(", ", partList.Select(part => FormatHeaderPart(part)))}";
            if (stale)
            {
                value += $", {FormatHeaderPart((AuthenticationHeaderNames.Stale, "true", false))}";
            }
            return value;
        }

        private string GetRspAuth(AuthorizationHeader authorizationHeader, string password)
        {
            var a1Hash = $"{authorizationHeader.UserName}:{authorizationHeader.Realm}:{password}".ToMD5Hash();
            var a2Hash = $":{authorizationHeader.Uri}".ToMD5Hash();
            return $"{a1Hash}:{authorizationHeader.Nonce}:{authorizationHeader.NonceCounter}:{authorizationHeader.ClientNonce}:{authorizationHeader.Qop}:{a2Hash}".ToMD5Hash();
        }

        private string GetNonce(DateTimeOffset? timestamp = null)
        {
            var privateKey = Options.PrivateKey;
            var timestampStr = timestamp?.ToString() ?? DateTimeOffset.UtcNow.ToString();
            return Convert.ToBase64String(_encoding.GetBytes($"{ timestampStr } {$"{timestampStr} : {privateKey}".ToMD5Hash()}"));
        }

        private string FormatHeaderPart((string Name, string Value, bool ShouldQuote) part)
            => part.ShouldQuote ? $"{part.Name}=\"{part.Value}\"" : $"{part.Name}={part.Value}";
    }
}
