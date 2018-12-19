using AspNet.WebApi.JwtBearer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace AspNet.WebApi.JwtBearer.Extensions
{
    public static class AppExtensions
    {
        public static bool AllowAnyonemous(this HttpActionContext context)
        {
            return context.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any()
                || context.ControllerContext.ControllerDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any();
        }

        public static Token GetToken(this HttpRequestMessage request)
        {
            var authHeader = request.Headers.Authorization;
            return new Token
            {
                Type = authHeader.Scheme,
                AccessToken = authHeader.Parameter
            };
        }
    }
}