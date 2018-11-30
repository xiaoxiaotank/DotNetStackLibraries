using AspNet.WebApi.RESTful.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace AspNet.WebApi.RESTful.Controllers
{
    /// <summary>
    /// RESTful：Representational state transfer 表述性状态转移
    /// 在方法上使用 ~ 来覆盖RoutePrefix
    /// </summary>
    [RoutePrefix("api/product")]
    public class ProductController : ApiController
    {
        /// <summary>
        /// 可以使用 Get 开头表示[HttpGet]
        /// Get: api/product
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ProductDto> GetAllProducts() => null;

        /// <summary>
        /// Get: api/product/4
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("{id:int}")]
        public ProductDto GetProductById(int id) => null;

        /// <summary>
        /// Get: api/product/xyz
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("{name}")]
        public ProductDto GetProductByName(string name) => null;

        /// <summary>
        /// Post: api/product
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult UpdateProduct(ProductDto dto) => Ok();

        /// <summary>
        /// Delete: api/product/4
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("~/api/product/deleteproduct/{id}")]
        [Route("id")]
        public IHttpActionResult DeleteProduct(int id) => Ok();

        /// <summary>
        /// 加 通配符（*）是为了使得参数可以跨越多个URI片段，因为要求时间格式为yyyy/MM/dd
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="stopTime"></param>
        /// <returns></returns>
        [Route("{pubdate:datetime:regex(\\d{4}-\\d{2}-\\d{2})}")]
        [Route("{*pubDate:datetime:regex(\\d{4}/\\d{2}/\\d{2})}")]
        //仅仅是为了使 ApiExplorer 生成  ApiDescription 的时候得到类型
        [ResponseType(typeof(IEnumerable<ProductDto>))]
        public IHttpActionResult GetProducts(DateTime pubDate) => Ok(pubDate);

        /// <summary>
        /// 约束声明不能有空格
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="stopTime"></param>
        /// <returns></returns>
        [Route("{startTime:datetime?}/{stopTime:datetime?}")]
        public IHttpActionResult GetProducts(DateTime? startTime, DateTime? stopTime) => Ok(new { startTime, stopTime });
    }
}
