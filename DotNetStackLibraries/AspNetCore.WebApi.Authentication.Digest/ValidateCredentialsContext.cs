using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetCore.Authentication.Digest
{
    public class ValidateCredentialsContext : ResultContext<DigestOptions>
    {
        public ValidateCredentialsContext(
            HttpContext context, 
            AuthenticationScheme scheme, 
            DigestOptions options) 
            : base(context, scheme, options)
        {
        }

        public string UserName { get; set; }
    }
}
