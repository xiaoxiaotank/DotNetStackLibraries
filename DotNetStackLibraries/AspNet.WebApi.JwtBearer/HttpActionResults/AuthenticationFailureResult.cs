using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace AspNet.WebApi.JwtBearer.HttpActionResults
{
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