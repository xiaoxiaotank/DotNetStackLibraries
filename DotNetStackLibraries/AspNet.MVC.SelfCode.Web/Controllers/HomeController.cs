using AspNet.MVC.SelfCode.MVC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AspNet.MVC.SelfCode.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return Content("Hello World!");
        }
    }
}