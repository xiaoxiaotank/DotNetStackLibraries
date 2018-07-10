using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AspNetCore.MVC.Redis.Models;
using StackExchange.Redis;
using Microsoft.Extensions.Caching.Distributed;

namespace AspNetCore.MVC.Redis.Controllers
{
    public class HomeController : Controller
    {
        private readonly IDistributedCache _memoryCache;

        public HomeController(IDistributedCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Send(string message)
        {
            string key = Guid.NewGuid().ToString("N");
            string value = message ?? string.Empty;
            _memoryCache.SetString(key, value);
            return Json(key);
        }

        public IActionResult Get(string key)
        {
            return Json(_memoryCache.GetString(key));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
