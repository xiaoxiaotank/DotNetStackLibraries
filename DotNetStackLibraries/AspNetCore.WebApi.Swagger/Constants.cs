using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace AspNetCore.WebApi.Swagger
{
    public static class Constants
    {
        /// <summary>
        /// Swagger 注释文档路径
        /// </summary>
        public static string SwaggerCommontXmlPath
        {
            get
            {
                var basePath = AppContext.BaseDirectory;
                var fileName = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                return Path.Combine(basePath, fileName);
            }
        }
    }
}
