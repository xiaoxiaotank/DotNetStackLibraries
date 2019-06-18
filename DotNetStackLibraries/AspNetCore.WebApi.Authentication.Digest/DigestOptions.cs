using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetCore.Authentication.Digest
{
    public class DigestOptions : AuthenticationSchemeOptions
    {
        public string Realm { get; set; }

        public string Qop { get; set; } = QopValues.Auth;

        public string Nonce { get; set; }

        public new DigestEvents Events
        {
            get => (DigestEvents)base.Events;
            set => base.Events = value;
        }
    }
}
