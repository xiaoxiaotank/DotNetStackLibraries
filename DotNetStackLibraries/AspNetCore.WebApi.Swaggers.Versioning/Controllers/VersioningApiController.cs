using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCore.WebApi.Swaggers.Versioning.Controllers
{
    [Route("api/v{api-version:apiVersion}/[controller]")]
    public class VersioningApiController : ApiController
    {
    }
}