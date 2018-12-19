using AspNet.WebApi.JwtBearer.Dtos.Account;
using AspNet.WebApi.JwtBearer.Dtos.Common;
using AspNet.WebApi.JwtBearer.Entities;
using AspNet.WebApi.JwtBearer.Helpers;
using AspNet.WebApi.JwtBearer.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace AspNet.WebApi.JwtBearer.Controllers
{
    public class AccountController : ControllerBase
    {
        //[OverrideAuthentication]
        [AllowAnonymous]
        [HttpPost]
        public IHttpActionResult Login(LoginDto dto)
        {
            var jwt = JwtHelper.GetJwtResponse(dto.UserName);
            if (!JwtHelper.IsTokenExist(jwt.AccessToken))
            {
                JwtHelper.AddToken(jwt.AccessToken);
            }

            return Ok(new ResultT<LoginDto>(
                new LoginDto
                {
                    UserName = dto.UserName,
                    Password = dto.Password,
                    Jwt = jwt
                })
            );
        }

        [HttpPost]
        public IHttpActionResult Logout()
        {
            var user = RequestingUser;
            if (user != null)
            {
                JwtHelper.RemoveToken(RequestingUser.Token);
            }
            return Ok();
        }
    }
}