using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AspNet.WebApi.Swagger.Controllers
{
    /// <summary>
    /// 启动时进入 http://localhost:port/swagger/ui/index/
    /// </summary>
    public class TestController : ApiController
    {
        /// <summary>
        /// 获取全部
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        /// <summary>
        /// 根据id获取
        /// </summary>
        /// <param name="id"></param>
        /// <remarks>
        ///     这只是一个示例
        ///     {
        ///         "id"：1
        ///     }
        /// </remarks>
        /// <response code="200">获取成功</response>
        /// <response code="404">未找到</response>
        /// <returns></returns>
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Test
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Test/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Test/5
        public void Delete(int id)
        {
        }
    }
}
