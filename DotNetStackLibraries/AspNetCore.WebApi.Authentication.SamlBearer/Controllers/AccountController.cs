using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Tokens.Saml2;
using Newtonsoft.Json;

namespace AspNetCore.WebApi.Authentication.SamlBearer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IHostingEnvironment _env;

        public AccountController(IHostingEnvironment env)
        {
            _env = env;
        }


        [HttpGet]
        [Route("login")]
        public ActionResult<string> Login()
        {
            var user = new User
            {
                Id = 1,
                Name = "jjj",
                Email = "jjj@j.me",
                Birthday = DateTime.Now.AddYears(-10),
                Password = "123456",
                PhoneNumber = "18888888888"
            };

            var tokenHandler = new Saml2SecurityTokenHandler();
            var privateKey = System.IO.File.ReadAllText(Path.Combine(_env.ContentRootPath, "private.key"));
            var rsaParameters = JsonConvert.DeserializeObject<RSAParameters>(privateKey);
            var rsaSecurityKey = new RsaSecurityKey(rsaParameters);

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Issuer = "https://www.jjj.me",
                Audience = "https://api.jjj.me",
                NotBefore = DateTime.Now,
                Expires = DateTime.UtcNow.AddMinutes(15),
                SigningCredentials = new SigningCredentials(rsaSecurityKey, SecurityAlgorithms.RsaSha256Signature, SecurityAlgorithms.Sha256Digest),
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.MobilePhone, user.PhoneNumber),
                    new Claim(ClaimTypes.Role, "Manager")
                })
            };

            var token = tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));

            return token;
        }

        [HttpPost]
        [Route("Validate")]
        public ActionResult<bool> Validate([FromHeader]string token)
        {
            var tokenHandler = new Saml2SecurityTokenHandler();
            var publicKey = System.IO.File.ReadAllText(Path.Combine(_env.ContentRootPath, "public.key"));
            var rsaParameters = JsonConvert.DeserializeObject<RSAParameters>(publicKey);
            var rsaSecurityKey = new RsaSecurityKey(rsaParameters);
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidIssuer = "https://www.jjj.me",
                ValidAudience = "https://api.jjj.me",
                IssuerSigningKey = rsaSecurityKey
            };

            ClaimsPrincipal retVal = null;

            try
            {
                retVal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken validatedToken);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return retVal != null && retVal.Identity.IsAuthenticated;
        }

    }
}