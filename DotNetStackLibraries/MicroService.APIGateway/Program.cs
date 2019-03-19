using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MicroService.Ocelot.Common.Helpers;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace MicroService.Ocelot.APIGateway
{
    /// <summary>
    /// https://www.jianshu.com/p/c967eda8b04d
    /// </summary>
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
               .UseStartup<Startup>()
               .UseUrls($"http://127.0.0.1:8800")
               .ConfigureAppConfiguration((hostingContext, builder) =>
               {
                   builder.AddJsonFile("configuration.json", false, true);
               });
        }
           
    }
}
