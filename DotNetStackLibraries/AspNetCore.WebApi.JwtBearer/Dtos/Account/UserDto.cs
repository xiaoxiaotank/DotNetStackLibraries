using AspNetCore.WebApi.JwtBearer.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCore.WebApi.JwtBearer.Dtos.Account
{
    public class UserDto
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("jwtResponse")]
         public JwtResponse JwtResponse { get; set; }
    }
}
