using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Owin;

[assembly: OwinStartup(typeof(SignalRSelfHost.Startup))]

namespace SignalRSelfHost
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.Map("/signalr", map =>
             {
                 map.UseCors(CorsOptions.AllowAll);
                 map.RunSignalR();
             });
        }
    }
}
