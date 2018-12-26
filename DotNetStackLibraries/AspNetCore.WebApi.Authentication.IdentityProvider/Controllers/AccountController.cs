using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using AspNetCore.WebApi.Authentication.IdentityProvider.Dtos;
using IdentityModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace AspNetCore.WebApi.Authentication.IdentityProvider.Controllers
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

        // GET api/account
        [HttpPost]
        public ActionResult<string> Login([FromBody]LoginDto dto)
        {
            if (!dto.UserName.Equals(dto.Password))
            {
                return string.Empty;
            }

            var user = new User
            {
                Id = 1,
                Name = dto.UserName,
                Email = "jjj@my.me",
                Birthday = DateTime.Now.AddYears(-10),
                PhoneNumber = "18888888888"
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var rsaPrivateKey = System.IO.File.ReadAllText(Path.Combine(_env.ContentRootPath, "private.key"));
            var rSAParameters = JsonConvert.DeserializeObject<RSAParameters>(rsaPrivateKey);
            var rsaSecurityKey = new RsaSecurityKey(rSAParameters);

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new Claim[] {
                    new Claim(JwtClaimTypes.Id, user.Id.ToString()),
                    new Claim(JwtClaimTypes.Name, user.Name),
                    new Claim(JwtClaimTypes.Email, user.Email),
                    new Claim(JwtClaimTypes.PhoneNumber, user.PhoneNumber),
                }),
                Expires = DateTime.UtcNow.AddMinutes(15),
                SigningCredentials = new SigningCredentials(rsaSecurityKey, SecurityAlgorithms.RsaSha256Signature),
                Audience = "aspnetcoreweb",
                Issuer = "jjj",
            };
            var token = tokenHandler.CreateEncodedJwt(tokenDescriptor);

            return token;
        }

        [HttpGet]
        public IActionResult GenerateAndSaveKey()
        {
            RSAParameters publicKeys, privateKeys;

            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                try
                {
                    privateKeys = rsa.ExportParameters(true);
                    publicKeys = rsa.ExportParameters(false);
                }
                finally
                {
                    rsa.PersistKeyInCsp = false;
                }
            }

            string dir = _env.ContentRootPath;

            System.IO.File.WriteAllText(Path.Combine(dir, "private.key"), JsonConvert.SerializeObject(privateKeys));
            System.IO.File.WriteAllText(Path.Combine(dir, "public.key"), JsonConvert.SerializeObject(publicKeys));

            return Ok();
        }
    }

    public class User
    {
        public int Id { get; internal set; }
        public string Name { get; internal set; }
        public string Email { get; internal set; }
        public DateTime Birthday { get; internal set; }
        public string Password { get; internal set; }
        public string PhoneNumber { get; internal set; }
    }
}
