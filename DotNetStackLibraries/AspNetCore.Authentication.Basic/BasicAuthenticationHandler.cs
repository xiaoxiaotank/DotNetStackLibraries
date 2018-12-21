using AspNetCore.Authentication.Basic.Events;
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
    public class BasicAuthenticationHandler : AuthenticationHandler<BasicAuthenticationOptions>
    {
        public BasicAuthenticationHandler(IOptionsMonitor<BasicAuthenticationOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
        {
        }
        protected new BasicAuthenticationEvents Events
        {
            get { return (BasicAuthenticationEvents)base.Events; }
            set { base.Events = value; }
        }
            
        protected override Task<object> CreateEventsAsync()
        {
            return Task.FromResult<object>(new BasicAuthenticationEvents());
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            string authorization = Request.Headers[HeaderNames.Authorization];
            if (string.IsNullOrWhiteSpace(authorization))
            {
                return AuthenticateResult.NoResult();
            }

            var credentials = string.Empty;
            var scheme = $"{BasicAuthenticationDefaults.AuthenticationScheme} ";
            if (authorization.StartsWith(scheme, StringComparison.OrdinalIgnoreCase))
            {
                credentials = authorization.Substring(scheme.Length).Trim();
            }

            if (string.IsNullOrEmpty(credentials))
            {
                return AuthenticateResult.NoResult();
            }

            try
            {
                credentials = Encoding.UTF8.GetString(Convert.FromBase64String(credentials));
                var data = credentials.Split(':');
                if(data.Length != 2)
                {
                    return AuthenticateResult.Fail("Invalid credentials, error format.");
                }

                var userName = data[0];
                var password = data[1];
                var validateCredentialsContext = new ValidateCredentialsContext(Context, Scheme, Options)
                {
                    UserName = userName,
                    Password = password
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
                return AuthenticateResult.Fail("Invalid credentials, error format.");
            }
            catch(Exception ex)
            {
                return AuthenticateResult.Fail(ex.Message);
            }
        }

        protected override Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            Response.Headers.Append(HeaderNames.WWWAuthenticate, $"{BasicAuthenticationDefaults.AuthenticationScheme} realm={Options.Realm}");

            return Task.CompletedTask;
        }
    }
}
