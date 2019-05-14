using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCore.WebApi.Swaggers.Versioning.Controllers.V1.V1_2
{
    [ApiVersion(ApiVersions.Version1_2)]
    public class ValuesController : VersioningApiController
    {
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "v1.2" };
        }

        [ApiVersionNeutral]
        [Route("/api/[controller]")]
        [HttpPost]
        public ActionResult<string> Post()
        {
            return "我是忽略版本控制的，任意版本都可以访问我哦";
        }
    }
}