using AspNetCore.MVC.AuthenticationAndAuthorization.Entities;
using IdentityModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AspNetCore.MVC.AuthenticationAndAuthorization.Utils.Extensions
{
    public static class AccountExtension
    {
        public static ClaimsPrincipal ToClaimsPrincipal(this User user)
        {
            //认证类型为Cookie
            var identity = new ClaimsIdentity("Cookie");
            //微软自带的ClaimTypes中的值都特别长，为了易读，可以使用JwtClaimTypes进行替代
            identity.AddClaims(new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.MobilePhone, user.PhoneNumber),
                new Claim(ClaimTypes.DateOfBirth, user.Birthday.ToString()),
            });

            #region jwtClaimTypes使用，需要nuget:IdentityModel
            //var identityByJwt = new ClaimsIdentity("Cookie", JwtClaimTypes.Name, JwtClaimTypes.Role);
            //identityByJwt.AddClaims(new List<Claim>()
            //{
            //    new Claim(JwtClaimTypes.Id,user.Id.ToString()),
            //    new Claim(JwtClaimTypes.Name, user.Name),
            //    new Claim(JwtClaimTypes.Email, user.Email),
            //    new Claim(JwtClaimTypes.PhoneNumber, user.PhoneNumber),
            //    new Claim(JwtClaimTypes.BirthDate, user.Birthday.ToString()),
            //});
    #endregion
            var principal = new ClaimsPrincipal(identity);
            return principal;
        }
}
}
