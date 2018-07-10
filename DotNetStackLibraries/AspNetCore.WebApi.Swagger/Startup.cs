using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.PlatformAbstractions;
using Swashbuckle.AspNetCore.Swagger;

namespace AspNetCore.WebApi.Swagger
{
    /// <summary>
    /// Install-Package Swashbuckle.AspNetCore
    /// </summary>
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            //注册Swagger生成器，定义一个或多个Swagger文档
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Info
                {
                    Title = "My API",
                    Version = "v1",
                    Description = "A simple demo for asp .net core web api",
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

                //添加注释文档
                options.IncludeXmlComments(Constants.SwaggerCommontXmlPath);
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
                //指定SwaggerJson端点
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                options.RoutePrefix = string.Empty;
            });

            app.UseMvc();


        }
    } 
}
