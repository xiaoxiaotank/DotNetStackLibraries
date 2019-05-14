using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCore.WebApi.Swaggers.Versioning.Filters
{
    public class ReplaceVersionWithExactValueInPathFilter : IDocumentFilter
    {
        public void Apply(SwaggerDocument swaggerDoc, DocumentFilterContext context)
        {
            swaggerDoc.Paths = swaggerDoc.Paths
                .ToDictionary(
                    path => path.Key.Replace($"{{{Constants.ApiVersionParameterName}}}", swaggerDoc.Info.Version),
                    path => path.Value
                );
        }
    }
}
