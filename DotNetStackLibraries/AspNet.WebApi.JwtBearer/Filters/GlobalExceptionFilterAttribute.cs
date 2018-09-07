using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Authentication;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Filters;

namespace AspNet.WebApi.JwtBearer.Filters
{
    public class GlobalApiExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override Task OnExceptionAsync(HttpActionExecutedContext actionExecutedContext, CancellationToken cancellationToken)
        {
            var exception = actionExecutedContext.Exception;
            HttpResponseMessage response = null;

            if (exception is ArgumentNullException)
            {
                response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent(actionExecutedContext.Exception.Message),
                    ReasonPhrase = "Argument null exception"
                };
            }
            else if (exception is TimeoutException)
            {
                response = new HttpResponseMessage(HttpStatusCode.RequestTimeout)
                {
                    Content = new StringContent(actionExecutedContext.Exception.Message),
                    ReasonPhrase = "Request Timeout"
                };
            }
            else if (exception is AuthenticationException)
            {
                var respone = new HttpResponseMessage(HttpStatusCode.Unauthorized)
                {
                    Content = new StringContent(exception.Message),
                    //未登录、登录已过期或token错误！
                    ReasonPhrase = "No login,or expired,and or error token"
                };
                actionExecutedContext.Response = respone;
                return base.OnExceptionAsync(actionExecutedContext, cancellationToken);
            }
            else
            {
                response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent(actionExecutedContext.Exception.Message),
                    ReasonPhrase = actionExecutedContext.Exception.Message
                };
            }
            throw new HttpResponseException(response);
        }
    }
}