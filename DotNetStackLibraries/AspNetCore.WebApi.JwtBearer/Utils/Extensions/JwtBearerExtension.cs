using AspNetCore.MVC.AuthenticationAndAuthorization.Entities;
using AspNetCore.WebApi.JwtBearer.Entities;
using IdentityModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AspNetCore.WebApi.JwtBearer.Utils.Extensions
{
    public static class JwtBearerExtension
    {
        #region 从配置中读取参数值
        public const string Config_Root_Name = "JwtBearer";
        public const string Issuer_Name = "Issuer";
        public const string Audience_Name = "Audience";
        public const string Secret_Key_Name = "SecretKey";
        public const string Expiration_Name = "ExpiresIn";
        #endregion

        private static readonly Encoding _encoding = Encoding.ASCII;

        /// <summary>
        /// 添加JwtBearer认证
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void AddJwtBearerAuthentication(this IServiceCollection services,IConfiguration configuration)
        {
            if (services == null || configuration == null)
            {
                throw new ArgumentNullException("services与configuration不能为null");
            }

            var root = configuration.GetSection(Config_Root_Name);
            //发行方
            var issuer = root[Issuer_Name];
            //接收方
            var audience = root[Audience_Name];
            //密钥
            var secretKey = root[Secret_Key_Name];
            //签名密钥
            var issuerSigningKey = new SymmetricSecurityKey(_encoding.GetBytes(secretKey));

            var tokenValidationParameters = new TokenValidationParameters()
            {
                //配置为jwt声明类型，默认是微软的ClaimTypes(名称太长，推荐使用jwt版本)
                NameClaimType = JwtClaimTypes.Name,
                RoleClaimType = JwtClaimTypes.Role,

                //验证发行方,默认true
                ValidateIssuer = true,
                ValidIssuer = issuer,

                //验证接收方,默认true
                ValidateAudience = true,
                ValidAudience = audience,

                //验证发行方签名密钥,默认true
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = issuerSigningKey,

                //验证生命周期（即是否过期或能否使用）,默认true
                //使用'当前时间'与Token的Claims中的 NotBefore 和 Expires 对比
                ValidateLifetime = true,
                //时钟漂移
                //这定义了最大允许时钟偏差 - 即在验证生命周期时提供令牌到期时间的容差。
                //当我们在本地创建令牌并在具有同步时间的相同机器上验证它们时，可以将其设置为零。 在使用外部令牌的情况下，这里设置一些余地可能有用。
                ClockSkew = TimeSpan.Zero
            };

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = tokenValidationParameters;
                //options.Events = new JwtBearerEvents()
                //{
                //    OnMessageReceived = context =>
                //    {
                //        //默认从Authorization头中获取Token，在此可以设置成别的
                //        context.Token = context.Request.Query["access_token"];
                //        return Task.CompletedTask;
                //    }
                //};
            });
        }

        /// <summary>
        /// 转为Jwt响应数据格式
        /// </summary>
        /// <remarks>
        /// iss: jwt签发者
        /// sub: jwt所面向的用户
        /// aud: 接收jwt的一方
        /// exp: jwt的过期时间，这个过期时间必须要大于签发时间
        /// nbf: 定义在什么时间之前，该jwt是不可用的.
        /// iat: jwt的签发时间
        /// jti: jwt的唯一身份标识，主要用来作为一次性token,从而回避重放攻击。
        /// </remarks>
        /// <param name="user"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static JwtResponse ToJwtResponse(this User user,IConfiguration configuration)
        {
            if (user == null || configuration == null)
            {
                throw new ArgumentNullException("services与configuration不能为null");
            }

            var now = DateTime.UtcNow;
            var root = configuration.GetSection(Config_Root_Name);
            //发行方
            var issuer = root[Issuer_Name];
            //接收方
            var audience = root[Audience_Name];
            //密钥
            var secretKey = root[Secret_Key_Name];
            //签名密钥
            var issuerSigningKey = new SymmetricSecurityKey(_encoding.GetBytes(secretKey));
            //过期时间段
            var expiresIn = TimeSpan.Parse(root[Expiration_Name]);
            //过期时间
            var expires = now.Add(expiresIn);

            var tokenHandler = new JwtSecurityTokenHandler();
            #region 创建Token方法1
            //var claims = new Claim[]
            //    {
            //    //签发方
            //    new Claim(JwtClaimTypes.Issuer,issuer),
            //    //面向用户
            //    new Claim(JwtClaimTypes.Subject, user.Id.ToString()),
            //    //接收方
            //    new Claim(JwtClaimTypes.Audience,audience),
            //    //过期时间
            //    new Claim(JwtClaimTypes.Expiration, expires.ToUnixTimeSeconds().ToString()),
            //    //该时间之前jwt不可用
            //    new Claim(JwtClaimTypes.NotBefore,now.ToUnixTimeSeconds().ToString()),
            //    //签发时间
            //    new Claim(JwtClaimTypes.IssuedAt, now.ToUnixTimeSeconds().ToString()),
            //    //身份标识
            //    new Claim(JwtClaimTypes.JwtId, Guid.NewGuid().ToString()),
            //    //用户名
            //    new Claim(JwtClaimTypes.Name,user.Name),
            //    //角色,这里设置为固定的了
            //    new Claim(JwtClaimTypes.Role,"User"),
            //    //Id
            //    new Claim(JwtClaimTypes.Id,user.Id.ToString()),
            //    //Email
            //    new Claim(JwtClaimTypes.Email,user.Email),
            //    //电话号码
            //    new Claim(JwtClaimTypes.PhoneNumber,user.PhoneNumber),
            //    };
            //var jwt = new JwtSecurityToken(
            //    claims: claims,
            //    signingCredentials: new SigningCredentials(issuerSigningKey, SecurityAlgorithms.HmacSha256Signature)
            //);

            //var jwtResponse = new JwtResponse()
            //{
            //    Status = true,
            //    AccessToken = tokenHandler.WriteToken(jwt),
            //    ExpiresIn = (int)expiresIn.TotalSeconds,
            //    TokenType = "Bearer"
            //};
            #endregion

            #region 创建Token方法二（推荐）
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Issuer = issuer,
                Subject = new ClaimsIdentity(new Claim[]
                {
                    //Id
                    new Claim(JwtClaimTypes.Id,user.Id.ToString()),
                    //用户名
                    new Claim(JwtClaimTypes.Name,user.Name),
                     //Email
                    new Claim(JwtClaimTypes.Email,user.Email),
                    //电话号码
                    new Claim(JwtClaimTypes.PhoneNumber,user.PhoneNumber),
                }),
                Audience = audience,
                IssuedAt = now,
                NotBefore = now,
                Expires = expires,
                SigningCredentials = new SigningCredentials(issuerSigningKey, SecurityAlgorithms.HmacSha256Signature)
            };
            var jwt = tokenHandler.CreateEncodedJwt(tokenDescriptor);

            var jwtResponse = new JwtResponse()
            {
                Status = true,
                AccessToken = jwt,
                ExpiresIn = (int)expiresIn.TotalSeconds,
                TokenType = JwtBearerDefaults.AuthenticationScheme
            };
            #endregion


            return jwtResponse;
        }
    }
}
