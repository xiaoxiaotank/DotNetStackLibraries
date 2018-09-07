using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AspNet.WebApi.JwtBearer.Entities
{
    /// <summary>
    /// 系统中发出请求的用户信息
    /// </summary>
    public class RequestingUser
    {
        public int Id { get; set; }

        public string Token { get; set; }

    }
}