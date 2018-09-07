using AspNet.WebApi.JwtBearer.Dtos.Common;
using AspNet.WebApi.JwtBearer.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AspNet.WebApi.JwtBearer.Dtos.Account
{
    public class LoginDto : DtoBase
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("userName")]
        public string UserName { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonProperty("jwt")]
        public JwtResponse Jwt { get; set; }
    }
}