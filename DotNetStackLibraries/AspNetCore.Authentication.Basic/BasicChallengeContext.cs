using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetCore.Authentication.Basic
{
    public class BasicChallengeContext : PropertiesContext<BasicOptions>
    {
        public BasicChallengeContext(
            HttpContext context,
            AuthenticationScheme scheme,
            BasicOptions options,
            AuthenticationProperties properties)
            : base(context, scheme, options, properties)
        {
        }

        /// <summary>
        /// 在认证期间出现的异常
        /// </summary>
        public Exception AuthenticateFailure { get; set; }

        /// <summary>
        /// 指定是否已被处理，如果已处理，则跳过默认认证逻辑
        /// </summary>
        public bool Handled { get; private set; }

        /// <summary>
        /// 跳过默认认证逻辑
        /// </summary>
        public void HandleResponse() => Handled = true;
    }
}
