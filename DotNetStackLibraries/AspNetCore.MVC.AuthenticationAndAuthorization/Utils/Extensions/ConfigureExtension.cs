using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCore.MVC.AuthenticationAndAuthorization.Utils.Extensions
{
    public static class ConfigureExtension
    {
        private static readonly string[] _noAuthorizePath = new[] { "/", "/Account/Login" };

        //使用授权
        public static IApplicationBuilder UseAuthorization(this IApplicationBuilder app)
        {
            return app.Use(async (context, next) =>
            {
                if (_noAuthorizePath.Contains(context.Request.Path.Value))
                {
                    await next();
                }
                else
                {
                    var user = context.User;
                    //该请求的用户是否已被验证
                    if (user?.Identity?.IsAuthenticated ?? false)
                    {
                        await next();
                    }
                    else
                    {
                        //若使用默认的方案，则跳转到登录页面，固定URL:/Account/Login
                        await context.ChallengeAsync();
                    }
                }
            });
        }
    }
}
