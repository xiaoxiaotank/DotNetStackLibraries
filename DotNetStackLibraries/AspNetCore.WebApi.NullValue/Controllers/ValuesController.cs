using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCore.WebApi.NullValue.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [NotFoundActionFilter]
    public class ValuesController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            //这里与 .net 不同的是：core版本遇到null会返回204(NoConent)，而老版本会返回200，并且会将null序列化为json
            //原因是core 使用了 HttpNoContentOutputFormatter 这种格式化器，会将null值转化为204
            //如果还是想要使用老版本的返回状态，则可以在 ConfigureServices 中进行配置
            return null;
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
