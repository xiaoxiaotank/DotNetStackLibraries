using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetCore.Authentication.Basic
{
    public class BasicOptions : AuthenticationSchemeOptions
    {
        public string Realm { get; set; }

        public new BasicEvents Events
        {
            get => (BasicEvents)base.Events; 
            set => base.Events = value; 
        }
    }
}
