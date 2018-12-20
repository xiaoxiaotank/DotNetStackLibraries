using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Filters;
using System.Web.Security;

namespace AspNet.WebApi.Basic
{
    public class AuthenticationFilter : IAuthenticationFilter
    {
        public bool AllowMultiple => false;

        public Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            var authHeader = context.Request.Headers.Authorization;
            
            if(authHeader == null)
            {
                context.ErrorResult = new AuthenticationFailureResult("无票据", context.Request);
                return Task.FromResult(0);
            }

            var type = authHeader.Scheme;
            var encryptTicket = authHeader.Parameter;
            if(!"Basic".Equals(type, StringComparison.OrdinalIgnoreCase) || string.IsNullOrWhiteSpace(encryptTicket))
            {
                context.ErrorResult = new AuthenticationFailureResult("票据无效", context.Request);
                return Task.FromResult(0);
            }

            try
            {
                var ticket = FormsAuthentication.Decrypt(encryptTicket);
                var user = ticket.UserData.Split(',');
                if(ticket.Expired || !new UserService().Login(user[0], user[1]))
                {
                    context.ErrorResult = new AuthenticationFailureResult("票据无效", context.Request);
                }
            }
            catch(ArgumentException)
            {
                context.ErrorResult = new AuthenticationFailureResult("票据无效", context.Request);
            }

            return Task.FromResult(0);
        }

        public Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            return Task.FromResult(0);   
        }
    }

    public class AuthenticationFailureResult : IHttpActionResult
    {
        public string Content { get; private set; }

        public HttpRequestMessage Request { get; private set; }


        public AuthenticationFailureResult(string content, HttpRequestMessage request)
        {
            Content = content;
            Request = request;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(Execute());
        }

        private HttpResponseMessage Execute()
        {
            var response = Request.CreateErrorResponse(HttpStatusCode.Unauthorized, Content);
            return response;
        }
    }
}