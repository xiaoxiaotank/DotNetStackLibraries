using System;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Hosting;
using Owin;
using Microsoft.Owin.Cors;

namespace SignalRSelfHost
{
    class Program
    {
        static void Main(string[] args)
        {
            // This will *ONLY* bind to localhost, if you want to bind to all addresses
            // use http://*:8088 to bind to all addresses. 
            // See http://msdn.microsoft.com/library/system.net.httplistener.aspx 
            // for more information.
            string url = "http://localhost:8088";
            using (WebApp.Start(url))
            {
                Console.WriteLine($"Server running on {url}");
                Console.ReadLine();
            }
        }
    }
  
}