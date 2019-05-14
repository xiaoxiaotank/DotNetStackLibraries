using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCore.WebApi.Swaggers.Versioning.Controllers.V1.V1_0
{
    [ApiVersion(ApiVersions.Version1_0, Deprecated = true)]
    public class ValuesController : VersioningApiController
    {
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "v1.0" };
        }

        [MapToApiVersion(ApiVersions.Version1_2)]
        [HttpPut]
        public ActionResult<string> Put()
        {
            return "v1.0";
        }
    }
}
