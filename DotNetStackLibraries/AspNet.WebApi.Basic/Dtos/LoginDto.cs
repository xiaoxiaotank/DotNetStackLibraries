using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AspNet.WebApi.Basic.Dtos
{
    public class LoginDto
    {
        public string UserName { get; set; }

        public string Password { get; set; }
    }
}