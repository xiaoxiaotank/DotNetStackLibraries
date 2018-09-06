using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace AspNet.MVC.SelfCode.Routing
{
    public class RouteCollection : Collection<RouteBase>
    {
        public RouteData GetRouteData(HttpContextBase httpContext)
        {
            foreach (var route in this)
            {
                RouteData routeData = route.GetRouteData(httpContext);
                if (routeData != null)
                {
                    return routeData;
                }
            }
            return null;
        }
    }
}
