using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace AspNetCore.Authentication.Basic
{
    public class BasicHandler : AuthenticationHandler<BasicOptions>
    {
        public BasicHandler(IOptionsMonitor<BasicOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
        {
        }

        protected new BasicEvents Events
        {
            get => (BasicEvents)base.Events; 
            set => base.Events = value; 
        }

        /// <summary>
        /// 确保创建的 Event 类型是 BasicEvents
        /// </summary>
        /// <returns></returns>
        protected override Task<object> CreateEventsAsync() => Task.FromResult<object>(new BasicEvents());

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var credentials = GetCredentials(Request);
            if(credentials == null)
            {
                return AuthenticateResult.NoResult();
            }

            try
            {
                credentials = Encoding.UTF8.GetString(Convert.FromBase64String(credentials));
                var data = credentials.Split(':');
                if(data.Length != 2)
                {
                    return AuthenticateResult.Fail("Invalid credentials: error format.");
                }

                var validateCredentialsContext = new ValidateCredentialsContext(Context, Scheme, Options)
                {
                    UserName = data[0],
                    Password = data[1]
                };
                await Events.ValidateCredentials(validateCredentialsContext);

                if(validateCredentialsContext.Result?.Succeeded == true)
                {
                    var ticket = new AuthenticationTicket(validateCredentialsContext.Principal, Scheme.Name);
                    return AuthenticateResult.Success(ticket);
                }

                return AuthenticateResult.NoResult();
            }
            catch(FormatException)
            {
                return AuthenticateResult.Fail("Invalid credentials: error format.");
            }
            catch(Exception ex)
            {
                return AuthenticateResult.Fail(ex.Message);
            }
        }

        protected override async Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            var authResult = await HandleAuthenticateOnceSafeAsync();
            var challengeContext = new BasicChallengeContext(Context, Scheme, Options, properties)
            {
                AuthenticateFailure = authResult?.Failure
            };
            await Events.Challenge(challengeContext);
            //质询已处理
            if (challengeContext.Handled) return;

            var challengeValue = $"{ BasicDefaults.AuthenticationScheme } realm=\"{ Options.Realm }\"";
            var error = challengeContext.AuthenticateFailure?.Message;
            if(!string.IsNullOrWhiteSpace(error))
            {
                //将错误信息封装到内部
                challengeValue += $" error={ error }";
            }

            Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            Response.Headers.Append(HeaderNames.WWWAuthenticate, challengeValue);
        }

        private string GetCredentials(HttpRequest request)
        {
            string credentials = null;

            string authorization = request.Headers[HeaderNames.Authorization];
            if (authorization != null)
            {
                var scheme = BasicDefaults.AuthenticationScheme;
                if (authorization.StartsWith(scheme, StringComparison.OrdinalIgnoreCase))
                {
                    credentials = authorization.Substring(scheme.Length).Trim();
                }
            }

            return credentials;
        }
    }
}
