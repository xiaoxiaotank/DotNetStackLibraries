using AspNet.WebApi.JwtBearer.Dtos.Account;
using AspNet.WebApi.JwtBearer.Dtos.Common;
using AspNet.WebApi.JwtBearer.Entities;
using AspNet.WebApi.JwtBearer.Extensions;
using AspNet.WebApi.JwtBearer.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace AspNet.WebApi.JwtBearer.Controllers
{
    [ApiAuthorize]
    public class AccountController : ControllerBase
    {
        [AllowAnonymous]
        [HttpPost]
        public IHttpActionResult Login(LoginDto dto)
        {
            var jwt = JwtExtension.GetJwtResponse(dto.UserName);
            if (!JwtExtension.IsTokenExist(jwt.AccessToken))
            {
                JwtExtension.AddToken(jwt.AccessToken);
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

        [AllowAnonymous]
        [HttpPost]
        public IHttpActionResult Logout()
        {
            var user = RequestingUser;
            if (user != null)
            {
                JwtExtension.RemoveToken(RequestingUser.Token);
            }
            return Ok();
        }
    }
}