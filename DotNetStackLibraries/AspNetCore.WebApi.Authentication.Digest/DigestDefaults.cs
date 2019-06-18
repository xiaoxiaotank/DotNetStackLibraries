using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetCore.Authentication.Digest
{
    public static class DigestDefaults
    {
        public const string AuthenticationScheme = "Digest";
    }

    public static class AuthenticateHeaderNames
    {
        public const string UserName = "username";
        public const string Realm = "realm";
        public const string Nonce = "nonce";
        public const string CNonce = "cnonce";
        public const string NC = "nc";
        public const string Qop = "qop";
        public const string Response = "response";
        public const string URI = "uri";
    }

    public static class QopValues
    {
        public const string Auth = "auth";
        public const string AuthInt = "auth-int";
    }
}
