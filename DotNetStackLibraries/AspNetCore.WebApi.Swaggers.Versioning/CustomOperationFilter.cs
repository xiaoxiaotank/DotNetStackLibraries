using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCore.WebApi.Swaggers.Versioning
{
    public class CustomOperationFilter : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {

        }
    }
}
