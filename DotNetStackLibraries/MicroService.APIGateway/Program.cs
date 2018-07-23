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
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            //var config = new ConfigurationBuilder()
            //    .AddCommandLine(args)
            //    .Build();

            //var ip = config["ip"];
            //var port = Convert.ToInt32(config["port"]);

            //if (string.IsNullOrEmpty(ip))
            //{
            //    ip = NetworkHelper.LocalIPAddress;
            //}
            //if (port == 0)
            //{
            //    port = NetworkHelper.GetRandomAvaliablePort();
            //}

            var ip = "127.0.0.1";
            var port = "8830";

            return WebHost.CreateDefaultBuilder(args)
               .UseStartup<Startup>()
               .UseUrls($"http://{ip}:{port}")
               .ConfigureAppConfiguration((hostingContext, builder) =>
               {
                   builder.AddJsonFile("configuration.json", false, true);
               });
        }
           
    }
}
