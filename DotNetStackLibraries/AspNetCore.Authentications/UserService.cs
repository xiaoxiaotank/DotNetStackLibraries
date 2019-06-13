using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCore.Authentications
{
    public class UserService
    {
        public static User Authenticate(string userName, string password)
        {
            if(userName == password)
            {
                return new User()
                {
                    UserName = userName,
                    Password = password
                };
            }

            return null;
        }
    }

    public class User
    {
        public string UserName { get; set; }

        public string Password { get; set; }
    }
}
