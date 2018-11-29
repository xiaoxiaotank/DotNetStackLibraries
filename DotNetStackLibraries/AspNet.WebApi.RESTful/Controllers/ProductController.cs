using AspNet.WebApi.RESTful.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AspNet.WebApi.RESTful.Controllers
{
    /// <summary>
    /// RESTful：Representational state transfer 表述性状态转移
    /// </summary>
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
        public ProductDto GetProductById(int id) => null;

        /// <summary>
        /// Post: api/product
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IHttpActionResult PostProduct(ProductDto dto) => Ok();

        /// <summary>
        /// Delete: api/product/4
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IHttpActionResult DeleteProduct(int id) => Ok();
    }
}
