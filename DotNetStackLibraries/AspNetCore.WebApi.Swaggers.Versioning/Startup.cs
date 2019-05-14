using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCore.WebApi.Swaggers.Versioning.Filters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;

[assembly: ApiController]
namespace AspNetCore.WebApi.Swaggers.Versioning
{
    public class Startup
    {
        private IApiVersionDescriptionProvider _apiVersionDescriptionProvider;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddApiVersioning(options =>
            {
                //是否启用在响应中报告Api版本兼容性信息
                options.ReportApiVersions = true;
                //用户请求不指定版本时是否使用默认版本
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
            })
            .AddVersionedApiExplorer(options => {
                //Api版本组名称格式
                options.GroupNameFormat = "'v'VVV";
            });
            _apiVersionDescriptionProvider = services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();

            //注册Swagger生成器，定义一个或多个Swagger文档
            services.AddSwaggerGen(options =>
            {
                foreach (var desc in _apiVersionDescriptionProvider.ApiVersionDescriptions)
                {
                    options.SwaggerDoc(desc.GroupName, new Info
                    {
                        Title = "My API",
                        Version = desc.ApiVersion.ToString(),
                        Description = "A simple demo about multi versioning for asp .net core web api",
                        TermsOfService = "None",
                        Contact = new Contact
                        {
                            Name = "贾建军",
                            Email = "jiajianj2057@qq.com",
                            Url = "http://www.jjj.com"
                        },
                        License = new License
                        {
                            Name = "license",
                            Url = "http://www.jjj.com/license"
                        }
                    });
                }

                //添加注释文档
                options.IncludeXmlComments(Constants.SwaggerCommontXmlPath);
                options.IgnoreObsoleteActions();
                options.IgnoreObsoleteProperties();
                options.OperationFilter<RemoveVersionFromParameterFilter>();
                options.DocumentFilter<ReplaceVersionWithExactValueInPathFilter>();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //启用中间件以将生成的Swagger作为JSON端点提供服务
            app.UseSwagger();
            //启用中间件以服务于swagger-ui(HTML/JS/CSS/etc.)
            app.UseSwaggerUI(options =>
            {
                foreach (var desc in _apiVersionDescriptionProvider.ApiVersionDescriptions)
                {
                    options.SwaggerEndpoint($"/swagger/{desc.GroupName}/swagger.json", desc.GroupName.ToUpperInvariant());
                }
                options.RoutePrefix = string.Empty;
            });
            app.UseMvc();
        }
    }
}
