using AspNet.WebApi.JwtBearer.Entities;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Jwt;
using Owin;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Web;

namespace AspNet.WebApi.JwtBearer.Helpers
{
    public static class JwtHelper
    {
        #region 从配置中读取参数值

        private const string IssuerName = "Issuer";
        private const string AudienceName = "Audience";
        private const string SecretKeyName = "SecretKey";
        private const string ExpirationName = "Expiration";
        public const string TokenType = "Bearer";

        private static readonly Encoding _encoding = Encoding.ASCII;
        private static readonly string _issuer = ConfigurationManager.AppSettings[IssuerName];
        private static readonly string _audience = ConfigurationManager.AppSettings[AudienceName];
        private static readonly string _secretKey = ConfigurationManager.AppSettings[SecretKeyName];
        private static readonly TimeSpan _expiration = TimeSpan.Parse(ConfigurationManager.AppSettings[ExpirationName]);
        private static readonly SymmetricSecurityKey _issuerSigningKey = new SymmetricSecurityKey(_encoding.GetBytes(_secretKey));

        public static readonly TokenValidationParameters TokenValidationParameters = new TokenValidationParameters()
        {
            #region 验证发行方
            ValidateIssuer = true,
            ValidIssuer = _issuer,
            #endregion

            #region 验证接收方
            ValidateAudience = true,
            ValidAudience = _audience,
            #endregion

            #region 验证发行方签名密钥
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = _issuerSigningKey,
            #endregion

            //验证生命周期（即是否超时）
            ValidateLifetime = true,
            //时钟漂移
            //这定义了最大允许时钟偏差 - 即在验证生命周期时提供令牌到期时间的容差。
            //当我们在本地创建令牌并在具有同步时间的相同机器上验证它们时，可以将其设置为零。 在使用外部令牌的情况下，这里设置一些余地可能有用。
            ClockSkew = TimeSpan.Zero
        };

        /// <summary>
        /// 有效（已登录未过期、未被注销）Token缓存
        /// </summary>
        private static Dictionary<string, string> _tokenCacheDic = new Dictionary<string, string>();

        /// <summary>
        /// 注册JwtBearer
        /// </summary>
        /// <param name="app"></param>
        #endregion
        public static void UseJwtBearer(this IAppBuilder app)
        {
            app.UseJwtBearerAuthentication(
                new JwtBearerAuthenticationOptions()
                {
                    TokenValidationParameters = TokenValidationParameters,
                    AuthenticationMode = AuthenticationMode.Active,
                }
            );
        }

        /// <summary>
        /// 获取Jwt响应数据
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        #warning 这里最好传入User对象
        public static JwtResponse GetJwtResponse(string userName)
        {
            //获取当前计算机时间并转化为世界时
            var now = DateTime.UtcNow;

            /*  iss: jwt签发者
                sub: jwt所面向的用户
                aud: 接收jwt的一方
                exp: jwt的过期时间，这个过期时间必须要大于签发时间
                nbf: 定义在什么时间之前，该jwt都是不可用的.
                iat: jwt的签发时间
                jti: jwt的唯一身份标识，主要用来作为一次性token,从而回避重放攻击。
            */
            var claims = new Claim[]
            {
                //用户
                new Claim(JwtRegisteredClaimNames.Sub, userName),
                //身份标识
                new Claim(JwtRegisteredClaimNames.Jti, "1",ClaimValueTypes.Integer),
                //签发时间
                new Claim(JwtRegisteredClaimNames.Iat, now.ToString(),ClaimValueTypes.DateTime),
                //用户名
                new Claim(ClaimTypes.Name,userName),
                //角色
                new Claim(ClaimTypes.Role,"User"),
            };

            var signingCredentials = new SigningCredentials(_issuerSigningKey, SecurityAlgorithms.HmacSha256Signature);
            var jwt = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims: claims,
                notBefore: now,
                expires: now.Add(_expiration),
                signingCredentials: signingCredentials
            );

            var response = new JwtResponse()
            {
                Status = true,
                AccessToken = new JwtSecurityTokenHandler().WriteToken(jwt),
                ExpiresIn = (int)_expiration.TotalSeconds,
                TokenType = TokenType
            };

            return response;
        }

        public static void AddToken(string token)
        {
            _tokenCacheDic.Add(token, null);
        }

        public static void RemoveToken(string token)
        {
            if (token != null && _tokenCacheDic.ContainsKey(token))
            {
                _tokenCacheDic.Remove(token);
            }
        }

        public static bool IsTokenExist(string token)
        {
            return token != null && _tokenCacheDic.ContainsKey(token);
        }
    }
}