using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace AspNetCore.WebApi.ILogger
{
    /// <summary>
    /// 本程序分别在 startup和此处 配置了日志，所以会输出两遍
    /// </summary>
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            //这里默认添加了 Console 和 Debug 两种形式的日志提供器
            WebHost.CreateDefaultBuilder(args)
                //Logger配置方式一：还可以在Startup中的Configure方法中配置
                .ConfigureLogging((hostContext, logging) =>
                {
                    //一共有 Console、Debug、EventSource、EventLog、TraceSource、Azure App Service六种
                    //这里重复进行Console和Debug的配置会覆盖上方的默认配置，所以不用担心重复配置的情况
                    logging.AddConsole()
                        .AddEventSourceLogger();
                })
                .UseStartup<Startup>();
    }
}
