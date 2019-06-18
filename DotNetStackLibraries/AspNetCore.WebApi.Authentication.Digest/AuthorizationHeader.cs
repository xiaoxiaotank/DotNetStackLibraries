using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetCore.Authentication.Digest
{
    public class AuthorizationHeader
    {
        public string UserName { get; set; }

        public string Realm { get; set; }

        public string Nonce { get; set; }

        public string CNonce { get; set; }

        public string NC { get; set; }

        public string Qop { get; set; }

        public string Response { get; set; }

        public string RequestMethod { get; set; }

        public string URI { get; set; }
    }
}
