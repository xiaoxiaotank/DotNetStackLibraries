using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AspNet.WebApi.Basic
{
    public class UserService
    {
        public bool Login(string userName, string password)
        {
            return !(string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(password));
        }
    }
}