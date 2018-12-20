using AspNet.WebApi.Basic.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Security;

namespace AspNet.WebApi.Basic.Controllers
{
    public class AccountController : ApiController
    {
        [OverrideAuthentication]
        public IHttpActionResult Login(LoginDto dto)
        {
            if(new UserService().Login(dto.UserName, dto.Password))
            {
                var ticket = new FormsAuthenticationTicket(
                    0,
                    dto.UserName,
                    DateTime.UtcNow,
                    DateTime.UtcNow.AddHours(1),
                    true,
                    $"{dto.UserName},{dto.Password}",
                    FormsAuthentication.FormsCookiePath);
                return Ok(FormsAuthentication.Encrypt(ticket));
            }

            return Ok("登录失败！");
        }
    }
}
