using AspNet.WebApi.JwtBearer.Helpers;
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
using System.Web.Http.Filters;
using AspNet.WebApi.JwtBearer.Extensions;
using AspNet.WebApi.JwtBearer.HttpActionResults;

namespace AspNet.WebApi.JwtBearer.Filters
{
    public class AuthenticationFilter : IAuthenticationFilter
    {
        public bool AllowMultiple => false;

        public Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            if (context.ActionContext.AllowAnyonemous())
            {
                return Task.FromResult(0);
            }

            var accessToken = string.Empty;
            try
            {
                var token = context.Request.GetToken();
                var type = token.Type;
                accessToken = token.AccessToken;

                #region 验证token类型和token是否不存在
                var tokenHandler = new JwtSecurityTokenHandler();
                SecurityToken securityToken = null;
                if (type.Equals(JwtHelper.TokenType, StringComparison.OrdinalIgnoreCase) && JwtHelper.IsTokenExist(accessToken))
                {
                    tokenHandler.ValidateToken(accessToken, JwtHelper.TokenValidationParameters, out securityToken);
                }
                else
                {
                    context.ErrorResult = new AuthenticationFailureResult("token类型错误或值无效！", context.Request);
                }
                #endregion
            }
            catch (SecurityTokenExpiredException)
            {
                JwtHelper.RemoveToken(accessToken);
                context.ErrorResult = new AuthenticationFailureResult("登录已过期！", context.Request);
            }
            catch (Exception)
            {
                context.ErrorResult = new AuthenticationFailureResult("身份验证未通过！", context.Request);
            }

            return Task.FromResult(0);
        }

        public Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            context.Result = new AddChallengeOnUnauthorizedResult(JwtHelper.TokenType, context.Result);
            return Task.FromResult(0);
        }    
    }
}