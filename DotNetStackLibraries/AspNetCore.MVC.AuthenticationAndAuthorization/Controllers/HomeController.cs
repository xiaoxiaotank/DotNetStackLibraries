using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AspNetCore.MVC.AuthenticationAndAuthorization.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

namespace AspNetCore.MVC.AuthenticationAndAuthorization.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [NonAction]
        private void CreateToken()
        {
            var principal = CreateClaimsPrincipal();
            var properties = new AuthenticationProperties()
            {
                //过期时间
                ExpiresUtc = DateTimeOffset.Now.AddHours(0.5)
            };
            var ticket = new AuthenticationTicket(principal, properties, "AuthenticationScheme(验证策略)");
            //然后将ticket加密序列化生成token
        }


        [NonAction]
        private ClaimsPrincipal CreateClaimsPrincipal()
        {
            var identity = CreateClaimsIdentity();
            //主角
            var principal = new ClaimsPrincipal(identity);
            return principal;
        }

        [NonAction]
        private ClaimsIdentity CreateClaimsIdentity()
        {
            var identity = new ClaimsIdentity("AuthenticationType");
            //添加声明
            identity.AddClaims(new List<Claim>()
            {
                new Claim(ClaimTypes.Name,"贾建军"),
                new Claim(ClaimTypes.Email,"jiajianj2057@qq.com"),
                new Claim(ClaimTypes.MobilePhone,"13823456711")
            });
            return identity;
        }
    }
}
