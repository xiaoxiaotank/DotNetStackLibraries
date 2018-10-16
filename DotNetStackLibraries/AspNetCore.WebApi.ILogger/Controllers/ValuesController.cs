using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AspNetCore.WebApi.ILogger.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly ILogger<ValuesController> _logger;
        
        public ValuesController(ILogger<ValuesController> logger)
        {
            _logger = logger;
        }

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            //记录对调试有帮助的信息，例如“开始分析XX”、“技术分析XX”，避免生产环境中使用
            _logger.LogTrace("我是 Trace");
            //记录调试信息，例如变量的值等，除非为了临时排错，否则应避免在生产环境中使用
            _logger.LogDebug("我是 Debug");
            //记录一些流程信息，例如记录客户端调用api的过程
            _logger.LogInformation("我是 Info");
            //记录一些警告信息，例如“文件未找到”、“信息不完整”等
            _logger.LogWarning("我是 Warning");
            //记录一些异常信息
            _logger.LogError("我是 Error");
            //记录需要立刻修复的问题，例如数据丢失、磁盘空间不足等
            _logger.LogCritical("我是 Critical");
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
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
