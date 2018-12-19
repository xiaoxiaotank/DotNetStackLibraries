using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AspNet.WebApi.JwtBearer.Entities
{
    public class Token
    {
        public string Type { get; set; }

        public string AccessToken { get; set; }
    }
}