using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AspNetCore.Authentication.Basic;
using AspNetCore.Authentication.Digest;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AspNetCore.Authentications
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
            #region Basic
            //services.AddAuthentication(BasicDefaults.AuthenticationScheme)
            //    .AddBasic(options =>
            //    {
            //        options.Realm = "http://localhost:44550";
            //        options.Events = new BasicEvents
            //        {
            //            OnValidateCredentials = context =>
            //            {
            //                var user = UserService.Authenticate(context.UserName, context.Password);
            //                if (user != null)
            //                {
            //                    var claim = new Claim(ClaimTypes.Name, context.UserName);
            //                    var identity = new ClaimsIdentity(BasicDefaults.AuthenticationScheme);
            //                    identity.AddClaim(claim);

            //                    context.Principal = new ClaimsPrincipal(identity);
            //                    context.Success();
            //                }
            //                return Task.CompletedTask;
            //            },
            //            //OnChallenge = context =>
            //            //{
            //            //    //跳过默认认证逻辑
            //            //    context.HandleResponse();
            //            //    return Task.CompletedTask;
            //            //}
            //        };
            //    });

            //services.AddAuthentication("jjj")
            //    .AddBasic("jjj", options =>{ }); 
            #endregion

            #region Digest
            services.AddAuthentication(DigestDefaults.AuthenticationScheme)
                .AddDigest(options =>
                {
                    options.Realm = "http://localhost:44550";
                    options.PrivateKey = "test private key";
                    options.Events = new DigestEvents(context => Task.FromResult(context.UserName));
                });

            #endregion

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseAuthentication();

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
