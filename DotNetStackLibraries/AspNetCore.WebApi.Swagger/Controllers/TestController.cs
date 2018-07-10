using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCore.WebApi.Swagger.Controllers
{
    [Produces("application/json")]
    [ApiVersion("1.0", Deprecated = false)]
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        /// <summary>
        /// 获取值
        /// </summary>
        /// <returns></returns>
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        /// <summary>
        /// 根据id获取值
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///     Post/Test
        ///         "":1
        /// </remarks>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return $"id={id}";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
