using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetCore.Authentication.Digest
{
    public class DigestOptions : AuthenticationSchemeOptions
    {
        public const string DefaultQop = QopValues.Auth;
        public const int DefaultMaxNonceAgeSeconds = 10;

        public string Realm { get; set; }

        public string Qop { get; set; } = DefaultQop;

        public int MaxNonceAgeSeconds { get; set; } = DefaultMaxNonceAgeSeconds;

        public string PrivateKey { get; set; }

        public new DigestEvents Events
        {
            get => (DigestEvents)base.Events;
            set => base.Events = value;
        }
    }
}
