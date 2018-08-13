﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCore.MVC.AuthenticationAndAuthorization.Models.Account
{
    public class LoginViewModel
    {
        public string Name { get; set; }

        public string Password { get; set; }

        public string ReturnUrl { get; set; } = "/";
    }
}
