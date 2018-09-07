using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(AspNet.WebApi.JwtBearer.Startup))]

namespace AspNet.WebApi.JwtBearer
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            
        }
    }
}
