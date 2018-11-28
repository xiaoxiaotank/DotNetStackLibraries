using AspNet.WebApi.ConnectDbByEF.MySql.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AspNet.WebApi.ConnectDbByEF.MySql.Controllers
{
    /// <summary>
    /// 准备：安装MySql.Data.Entity，会同时安装依赖MySql.Data和EntityFramework，
    ///     把Data和EntityFramework.SqlServer引用移除，并删除web.config中的相关节点
    /// 1.创建ADO.NET数据模型DbModel，选择空的CodeFirst
    /// 2.在DbModel上添加特性：[DbConfigurationType(typeof(MySqlEFConfiguration))]
    /// 3.在程序包管理控制台输入“enable-migrations” 创建迁移目录，生成Configuration类
    /// 4.在程序包管理控制台输入“add-migraiton [name]”，[name]任意起名，如“Init”，创建迁移文件
    /// 5.在程序包管理控制台输入“update-database”，生成数据库
    /// 
    /// 注意：连接字符串节点中一定要注明 providerName="MySql.Data.MySqlClient"
    /// 
    /// </summary>
    public class ValuesController : ApiController
    {
        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        public IHttpActionResult Get(int id)
        {
            DbModel dbModel = new DbModel();
            var dto = dbModel.MyEntities.First();
            return Ok(dto);
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
