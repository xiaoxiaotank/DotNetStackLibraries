using AspNet.WebApi.JwtBearer.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace AspNet.WebApi.JwtBearer
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API 配置和服务
            config.Filters.Add(new AuthenticationFilter());
            //异常过滤器
            config.Filters.Add(new GlobalApiExceptionFilterAttribute());

            // Web API 路由
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
