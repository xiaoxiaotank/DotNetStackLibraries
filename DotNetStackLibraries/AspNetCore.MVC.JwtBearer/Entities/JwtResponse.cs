﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCore.MVC.JwtBearer.Entities
{
    public class JwtResponse
    {
        [JsonProperty("status")]
        public bool Status { get; set; }

        [JsonProperty("accessToken")]
        public string AccessToken { get; set; }

        [JsonProperty("expriesIn")]
        public int ExpiresIn { get; set; }

        [JsonProperty("tokenType")]
        public string TokenType { get; set; }
    }
}
