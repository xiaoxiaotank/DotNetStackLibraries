using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetCore.Authentication.Digest
{
    public class DigestChallengeContext : PropertiesContext<DigestOptions>
    {
        public DigestChallengeContext(
            HttpContext context, 
            AuthenticationScheme scheme, 
            DigestOptions options, 
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
