using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCore.MVC.AuthenticationAndAuthorization.Models.Account;
using AspNetCore.MVC.AuthenticationAndAuthorization.Storages.Interfaces;
using AspNetCore.MVC.AuthenticationAndAuthorization.Utils.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity.UI.Pages.Account.Internal;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCore.MVC.AuthenticationAndAuthorization.Controllers
{
    /// <summary>
    /// 省略了应用层，直接使用领域层接口
    /// </summary>
    public class AccountController : Controller
    {
        private readonly IUserStore _userStore;

        public AccountController(IUserStore userStore)
        {
            _userStore = userStore;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromForm]LoginViewModel model)
        {
            var user = _userStore.GetUser(model.Name, model.Password);
            var principal = user.ToClaimsPrincipal();
            await HttpContext.SignInAsync(principal);

            return Redirect(model.ReturnUrl);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return Redirect("/");
        }
    }
}