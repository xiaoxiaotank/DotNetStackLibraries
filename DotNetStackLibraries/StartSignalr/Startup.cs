using System;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
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
            //app.MapSignalR();



            //更多Hub配置
#if false
            var hubConfiguration = new HubConfiguration
            {
                EnableDetailedErrors = true,    //启用详细错误
                EnableJavaScriptProxies = false //启用js代理
            };
            app.MapSignalR(hubConfiguration);
#endif



            //跨域配置,使用Map和RunSignalR替代MapSignalR
            app.Map("/signalr", map =>
             {
                 map.UseCors(CorsOptions.AllowAll);
                 var hubConfiguration = new HubConfiguration
                 {
                     EnableDetailedErrors = true
                 };
                 map.RunSignalR(hubConfiguration);
             });

            //重连超时
            GlobalHost.Configuration.ConnectionTimeout = TimeSpan.FromSeconds(110);

            //断开连接时间，即当连接丢失时，超过30秒后再终止Signalr连接并触发Disconnected方法
            GlobalHost.Configuration.DisconnectTimeout = TimeSpan.FromSeconds(30);

            //非长连接情况下，没10秒发送一个包
            //这个值不能超过DiscconnectTimeout的1/3
            GlobalHost.Configuration.KeepAlive = TimeSpan.FromSeconds(10);

        }
    }
}
