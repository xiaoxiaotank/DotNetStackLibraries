using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace AspNet.MVC.SelfCode.Routing
{
    public class Route : RouteBase
    {
        public override RouteData GetRouteData(HttpContextBase httpContext)
        {
            var assembly = Assembly.Load("AspNet.MVC.SelfCode.MVC");
            var mvcRouteHanlder = assembly.CreateInstance("AspNet.MVC.SelfCode.MVC.MvcRouteHandler") as IRouteHandler;
            return new RouteData()
            {
                RouteHandler = mvcRouteHanlder
            };
        }
    }
}
