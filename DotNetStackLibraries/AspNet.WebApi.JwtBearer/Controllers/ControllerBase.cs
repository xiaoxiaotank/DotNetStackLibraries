using AspNet.WebApi.JwtBearer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace AspNet.WebApi.JwtBearer.Controllers
{
    public class ControllerBase : ApiController
    {
        /// <summary>
        /// 当前发出请求的用户信息
        /// </summary>
        public RequestingUser RequestingUser { get; set; }
    }
}
