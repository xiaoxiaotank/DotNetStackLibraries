using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCore.MVC.AuthenticationAndAuthorization.Storages;
using AspNetCore.MVC.AuthenticationAndAuthorization.Storages.Implements;
using AspNetCore.MVC.AuthenticationAndAuthorization.Storages.Interfaces;
using AspNetCore.MVC.AuthenticationAndAuthorization.Utils.Extensions;
using AspNetCore.MVC.AuthenticationAndAuthorization.Utils.Validators;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace AspNetCore.MVC.AuthenticationAndAuthorization
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            #region 注册认证服务

            //单一服务中用Cookie
            //我服务中使用Auth2.0和OpenId Connect
            services.AddAuthentication(options =>
            {
                //对于未明显设置的都应用该方案，即Cookie
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                //默认认证方案，由于与DefaultScheme相同，可不写
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                //默认登录方案，由于与DefaultScheme相同，可不写
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;

                #region Challenge 默认质询方案
                //方案设置为OAuth
                //options.DefaultChallengeScheme = OAuthDefaults.DisplayName;
                //方案设置为OpenId
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
                #endregion
            })
            //添加Cookie
            .AddCookie(options => 
            {
                //在此设置登录路径，默认是“/Account/Login”
                options.LoginPath = "/Account/Login";
                //减少Cookie长度的终极解决方案就是参考Session的原理，把Claims信息则保存在服务端，并为其设置一个ID，Cookie中则只保存该ID，
                //这样就可以在服务端通过该ID来检索出完整的Claims信息。不过注意，这并不是在使用 ASP.NET Core 中的Session，只是参考其存储方式。
                options.SessionStore = new MemoryCacheTicketStore();
                //自定义Principal验证器，该验证通常会查询数据库，损耗较大，可以通过设置验证周期来提高性能，如：每5分钟执行验证一次
                options.Events = new CookieAuthenticationEvents()
                {
                    OnValidatePrincipal = LastChangedValidator.ValidateAsync
                };
            })
            //一个授权协议，不建议用来认证
            //.AddOAuth(OAuthDefaults.DisplayName,options =>
            //{
            //    options.ClientId = "oauth.code";
            //    options.ClientSecret = "secret";
            //    options.AuthorizationEndpoint = "https://oidc.faasx.com/connect/authorize";
            //    options.TokenEndpoint = "https://oidc.faasx.com/connect/token";
            //    options.CallbackPath = "/signin-oauth";
            //    options.Scope.Add("openid");
            //    options.Scope.Add("profile");
            //    options.Scope.Add("email");
            //    //设置是否将OAuth服务器返回的Token信息保存到AuthenticationProperties中
            //    options.SaveTokens = true;
            //    // 事件执行顺序 ：
            //    // 1.创建Ticket之前触发
            //    options.Events.OnCreatingTicket = context => Task.CompletedTask;
            //    // 2.创建Ticket失败时触发
            //    options.Events.OnRemoteFailure = context => Task.CompletedTask;
            //    // 3.Ticket接收完成之后触发
            //    options.Events.OnTicketReceived = context => Task.CompletedTask;
            //    // 4.Challenge时触发，默认跳转到OAuth服务器
            //    // options.Events.OnRedirectToAuthorizationEndpoint = context => context.Response.Redirect(context.RedirectUri);

            //})
            .AddOpenIdConnect(o => 
            {
                //设置不启用https
                o.RequireHttpsMetadata = false;
                o.ClientId = "oidc.hybrid";
                o.ClientSecret = "secret";
                //若不设置Authority，就必须指定MetadataAddress
                o.Authority = "https://oidc.faasx.com/";
                // 默认为$"{Authority}.well-known/openid-configuration"
                //o.MetadataAddress = "https://oidc.faasx.com/.well-known/openid-configuration";
                //使用混合流
                o.ResponseType = OpenIdConnectResponseType.CodeIdToken;
                // 是否将Tokens保存到AuthenticationProperties中
                o.SaveTokens = true;
                // 是否从UserInfoEndpoint获取Claims
                o.GetClaimsFromUserInfoEndpoint = true;
                // 在本示例中，使用的是IdentityServer，而它的ClaimType使用的是JwtClaimTypes
                o.TokenValidationParameters.NameClaimType = "name";//JwtClaimTypes.Name
            });

            #endregion

            #region DI
            //1、Transient：每次从容器 （IServiceProvider）中获取的时候都是一个新的实例
            //2、Singleton：每次从同根容器(相当于父类、接口)中（同根 IServiceProvider）获取的时候都是同一个实例
            //3、Scoped：每次从同一个容器（相当于具体的实现类）中获取的实例是相同的
            services.AddScoped<IUserStore, UserStore>();
            #endregion

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            //app.Run(async (context) =>
            //{
            //    await context.Response.WriteAsync("Hello World!");
            //});

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            
            app.UseStaticFiles();
            app.UseCookiePolicy();

            //注册认证中间件
            app.UseAuthentication();
            //注册授权中间件（自定义）
            app.UseAuthorization();
            
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
