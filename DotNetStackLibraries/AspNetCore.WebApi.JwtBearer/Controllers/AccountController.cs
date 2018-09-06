using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AspNetCore.MVC.AuthenticationAndAuthorization.Storages.Interfaces;
using AspNetCore.WebApi.JwtBearer.Dtos.Account;
using AspNetCore.WebApi.JwtBearer.Utils.Extensions;
using IdentityModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace AspNetCore.WebApi.JwtBearer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IUserStore _userStore;

        public AccountController(IConfiguration configuration, IUserStore userStore)
        {
            _configuration = configuration;
            _userStore = userStore;
        }

        [HttpPost("[action]")]
        public IActionResult Login([FromBody]LoginDto dto)
        {
            var user = _userStore.GetUser(dto.UserName, dto.Password);
            if(user == null)
            {
                return Unauthorized();
            }

            var jwtResponse = user.ToJwtResponse(_configuration);
            if(jwtResponse == null)
            {
                throw new Exception("服务器内部错误");
            }

            var result = new UserDto()
            {
                Id = user.Id,
                Name = user.Name,
                JwtResponse = jwtResponse
            };
            return Ok(result);
        }
    }
}