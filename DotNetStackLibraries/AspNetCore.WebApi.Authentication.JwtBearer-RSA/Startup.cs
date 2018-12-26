using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using IdentityModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace AspNetCore.WebApi.Authentication.JwtBearer_RSA
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            Configuration = configuration;
            HostingEnvironment = hostingEnvironment;
        }

        public IConfiguration Configuration { get; }

        public IHostingEnvironment HostingEnvironment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    //var key = Encoding.ASCII.GetBytes(Consts.Secret);

                    var rsaPublicKey = File.ReadAllText(Path.Combine(HostingEnvironment.ContentRootPath, "public.key"));
                    var rsaParameters = JsonConvert.DeserializeObject<RSAParameters>(rsaPublicKey);
                    var rsaSecurityKey = new RsaSecurityKey(rsaParameters);

                    options.TokenValidationParameters.IssuerSigningKey = rsaSecurityKey;
                    options.TokenValidationParameters.ValidAudience = "aspnetcoreweb";
                    options.TokenValidationParameters.ValidIssuer = "jjj";
                    options.TokenValidationParameters.NameClaimType = JwtClaimTypes.Name;
                    //如果要使用微软基于角色的身份验证，一定要使用微软的ClaimTypes
                    options.TokenValidationParameters.RoleClaimType = ClaimTypes.Role;
                });
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
