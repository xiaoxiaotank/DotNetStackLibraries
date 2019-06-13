using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetCore.Authentication.Basic
{
    /// <summary>
    /// 封装认证参数信息上下文
    /// </summary>
    public class ValidateCredentialsContext : ResultContext<BasicOptions>
    {
        public ValidateCredentialsContext(HttpContext context, AuthenticationScheme scheme, BasicOptions options) 
            : base(context, scheme, options)
        {
        }

        public string UserName { get; set; }

        public string Password { get; set; }
    }
}
