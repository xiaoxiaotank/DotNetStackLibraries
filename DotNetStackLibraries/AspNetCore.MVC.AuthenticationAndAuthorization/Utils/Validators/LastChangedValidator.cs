using AspNetCore.MVC.AuthenticationAndAuthorization.Repositories.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCore.MVC.AuthenticationAndAuthorization.Utils.Validators
{
    //对于认证系统，身份令牌都会有一个有效期的概念，而Cookie认证中默认有效期是14天，因此只要浏览器没有清除Cookie，并且Cookie没有过期，便么就一直可以验证通过。但是，如果用户修改了密码，我们希望该Cookie失效，或者是用户更新了Claims的信息时，我们希望重新生成Cookie，否则我们取到的还是旧的Claims信息。那么，该怎么做呢？
    //对此，网上比较流行的做法是在用户数据库中添加一个安全字段，当用户修改了一些安全性信息时，便更新该字段，并在Claim中加入此字段，一起写入到Cookie中，验证时便可以判断该字段是否与数据库一致，若不一致则验证失败或重新生成：
    public static class LastChangedValidator
    {
        public static async Task ValidateAsync(CookieValidatePrincipalContext context)
        {
            var userRepository = context.HttpContext.RequestServices.GetRequiredService<IUserRepository>();
            var userPrincipal = context.Principal;
            var lastChanged = (from c in userPrincipal.Claims
                               where c.Type == "LastUpdated"
                               select c.Value)
                              .FirstOrDefault();
            if(string.IsNullOrEmpty(lastChanged) || !userRepository.ValidateLastChanged(userPrincipal, lastChanged))
            {
                #region 两种方式只能选择一种，不能同时存在
                //1.验证失败
                context.RejectPrincipal();

                //2.验证通过,并重新生成Cookie
                context.ShouldRenew = true; 
                #endregion
            }
        }
    }
}
