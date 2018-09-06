using AspNet.MVC.SelfCode.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace AspNet.MVC.SelfCode.MVC
{
    public class MvcHandler : IHttpHandler
    {
        public bool IsReusable => false;

        public RequestContext RequestContext { get; private set; }

        public MvcHandler(RequestContext requestContext)
        {
            RequestContext = requestContext;
        }

        public void ProcessRequest(HttpContext context)
        {
            HttpContextBase httpContextBase = new HttpContextWrapper(context);

            HttpContext currentContext = HttpContext.Current;
            var routeData = RequestContext.RouteData;

            var controller = new Controller();
            controller.Execute(RequestContext);
        }

    }
}
