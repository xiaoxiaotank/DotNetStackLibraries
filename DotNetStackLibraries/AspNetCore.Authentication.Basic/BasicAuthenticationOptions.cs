using AspNetCore.Authentication.Basic.Events;
using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetCore.Authentication.Basic
{
    public class BasicAuthenticationOptions : AuthenticationSchemeOptions
    {
        public string Realm { get; set; }

        //为了使调用者能够通过接口来调用
        public new BasicAuthenticationEvents Events
        {
            get { return (BasicAuthenticationEvents)base.Events; }
            set { base.Events = value; }
        }
    }
}
