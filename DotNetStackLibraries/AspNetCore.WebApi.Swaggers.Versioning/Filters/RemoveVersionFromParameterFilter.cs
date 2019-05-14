using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCore.WebApi.Swaggers.Versioning.Filters
{
    public class RemoveVersionFromParameterFilter : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            var apiVersionParameter = operation.Parameters.SingleOrDefault(p => p.Name == Constants.ApiVersionParameterName);
            if(apiVersionParameter != null)
            {
                operation.Parameters.Remove(apiVersionParameter);
            }
        }
    }
}
