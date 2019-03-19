using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MicroService.Ocelot.ProductService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountersController : ControllerBase
    {
        private static int _count = 0;

        [HttpGet("Count")]
        public string Count()
        {
            return $"Count {++_count} from ProductService";
        }
    }
}