using AspNet.WebApi.JwtBearer.Extensions;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Authentication;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using DotNet.Common;
using AspNet.WebApi.JwtBearer.Entities;
using AspNet.WebApi.JwtBearer.Controllers;

namespace AspNet.WebApi.JwtBearer.Filters
{
    public class ApiAuthorizeAttribute : AuthorizeAttribute
    {
        public string PermissionCode { get; set; }

        public override Task OnAuthorizationAsync(HttpActionContext actionContext, CancellationToken cancellationToken)
        {
            #region 可匿名则直接跳过
            var attrsOfAllowAnonymous = actionContext
                   .ActionDescriptor
                   .GetCustomAttributes<AllowAnonymousAttribute>()
                   .OfType<AllowAnonymousAttribute>();
            if (attrsOfAllowAnonymous.IsNotNullAndEmpty())
            {
                return base.OnAuthorizationAsync(actionContext, cancellationToken);
            }
            #endregion

            var token = string.Empty;
            try
            {
                var authHeader = actionContext.Request.Headers
                               .Where(h => h.Key == "Authorization")
                               .FirstOrDefault();
                var typeAndToken = authHeader.Value.FirstOrDefault().Split();
                var type = typeAndToken[0];
                token = typeAndToken[1];
                //验证token类型和token是否存在
                if (type.Equals(JwtExtension.TokenType, StringComparison.OrdinalIgnoreCase) && JwtExtension.IsTokenExist(token))
                {
                    var tokenHandler = new JwtSecurityTokenHandler();
                    SecurityToken securityToken = null;
                    var user = tokenHandler.ValidateToken(token, JwtExtension.TokenValidationParameters, out securityToken);
                    #region 存储用户信息
                    var controller = actionContext.ControllerContext.Controller as ControllerBase;
                    controller.RequestingUser = new RequestingUser()
                    {
                        Id = int.Parse(user.Claims.Single(c => c.Type.Equals(JwtRegisteredClaimNames.Jti)).Value),
                        Token = token
                    };
                    #endregion
                }
                else
                {
                    var respone = new HttpResponseMessage(HttpStatusCode.Unauthorized)
                    {
                        Content = new StringContent("token类型错误或值无效！"),
                        ReasonPhrase = "token type or value error"
                    };
                    actionContext.Response = respone;
                }
            }
            catch (SecurityTokenExpiredException)
            {
                JwtExtension.RemoveToken(token);
                throw new AuthenticationException("登录已过期！");
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
                throw new AuthenticationException("未授权！");
            }

            return Task.FromResult(0);
        }
    }
}