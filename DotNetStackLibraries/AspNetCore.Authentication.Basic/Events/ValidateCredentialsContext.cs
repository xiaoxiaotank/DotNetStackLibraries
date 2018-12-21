﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetCore.Authentication.Basic.Events
{
    public class ValidateCredentialsContext : ResultContext<BasicAuthenticationOptions>
    {

        public string UserName { get; set; }

        public string Password { get; set; }

        public ValidateCredentialsContext(HttpContext context, AuthenticationScheme scheme, BasicAuthenticationOptions options) : base(context, scheme, options)
        {
        }

    }
}
