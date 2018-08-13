using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCore.MVC.AuthenticationAndAuthorization.Models.Account;
using AspNetCore.MVC.AuthenticationAndAuthorization.Models.Special;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCore.MVC.AuthenticationAndAuthorization.Controllers
{
    public class SpecialController : Controller
    {
        public IActionResult Index()
        {
            var user = HttpContext.User;
            var model = new IndexViewModel()
            {
                UserName = user.Identity.Name,
                AuthenticationType = user.Identity.AuthenticationType
            };
            return View(model);
        }
    }
}