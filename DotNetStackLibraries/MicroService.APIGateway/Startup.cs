using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Ocelot.Administration;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;

namespace MicroService.Ocelot.APIGateway
{
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
            void options(IdentityServerAuthenticationOptions o)
            {
                o.Authority = "http://localhost:8801";
                o.RequireHttpsMetadata = false;
                o.ApiName = "api1";
            }

            //配置管理分为外部和内部Identity Server，只需要使用一种即可，本例使用内部
            //Administration一共提供了3组Api：
            //根据客户端信息，生成Token：post http://localhost:8800/administration/connect/token 
            //                              body form-data{ client_id:"client", secret:"secret", scope:"api1", grant_type:"client_credentials" }，内部模式时，除了secret外，其他任意
            //配置管理：
            //      获取配置：get  http://localhost:8800/administration/configuration {"Authorization":"Bearer token"}
            //      修改配置：post http://localhost:8800/administration/configuration { "Authorization": "Bearer token"}, body{修改后的json配置}
            //      当设置configuration.json为“始终复制时”，重新调试时会将Debug中的配置替换为最初始的状态
            //缓存管理：

            services.AddOcelot(Configuration)
                .AddConsul()
                //配置管理：外部IdentityServer，需要AddIdentityServerAuthentication
                //.AddAdministration("/administration", options);
                //配置管理：内部IdentityServer
                .AddAdministration("/administration", "secret");

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddIdentityServerAuthentication("TestKey", options);
        }

        /// <summary>
        /// 必须返回 void
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public async void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();
            await app.UseOcelot();
        }
    }
}
