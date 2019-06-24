using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetCore.Authentication.Digest
{
    public static class DigestDefaults
    {
        public const string AuthenticationScheme = "Digest";
    }

    public static class AuthenticationHeaderNames
    {
        public const string UserName = "username";
        public const string Realm = "realm";
        public const string Nonce = "nonce";
        public const string ClientNonce = "cnonce";
        public const string NonceCounter = "nc";
        public const string Qop = "qop";
        public const string Response = "response";
        public const string Uri = "uri";
        public const string RspAuth = "rspauth";
        public const string Stale = "stale";
    }

    public static class QopValues
    {
        public const string Auth = "auth";
        public const string AuthInt = "auth-int";
    }
}
