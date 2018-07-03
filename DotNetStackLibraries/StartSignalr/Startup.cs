using System;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Owin;
using StartSignalr.PipelineModule;

[assembly: OwinStartup(typeof(StartSignalr.Startup))]

namespace StartSignalr
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //注册异常处理模块
            GlobalHost.HubPipeline.AddModule(new ErrorHandlingPipelineModule());
            //注册日志模块
            GlobalHost.HubPipeline.AddModule(new LoggingPipelineModule());

            //注册SignalR中间件，默认路径“/signalr”，等同于
            //app.MapSignalR("/signalr",new HubConfiguration());
            app.MapSignalR();



            //更多Hub配置
#if false
            var hubConfiguration = new HubConfiguration
            {
                EnableDetailedErrors = true,    //启用详细错误
                EnableJavaScriptProxies = false //启用js代理
            };
            app.MapSignalR(hubConfiguration);
#endif
        }
    }
}
