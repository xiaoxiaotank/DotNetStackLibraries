using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace AspNet.MVC.SelfCode.Routing
{
    public class UrlRoutingModule : IHttpModule
    {
        private static readonly object _contextKey = new Object();

        public void Dispose()
        {
            
        }

        public void Init(HttpApplication application)
        {
            if (application.Context.Items[_contextKey] != null) return;

            application.Context.Items[_contextKey] = _contextKey;

            application.PostResolveRequestCache += Context_PostResolveRequestCache;
        }

        private void Context_PostResolveRequestCache(object sender, EventArgs e)
        {
            var app = sender as HttpApplication;
            HttpContextBase context = new HttpContextWrapper(app.Context);
            var routeData = RouteTable.Routes.GetRouteData(context);
            //无路由则不进行处理
            if (routeData == null) return;

            var requestContext = new RequestContext { RouteData = routeData, HttpContext = context };
            var httphandler = routeData.RouteHandler?.GetHttpHandler(requestContext);
            if (httphandler == null)
            {
                throw new InvalidOperationException();
            }

            //重新映射
            context.RemapHandler(httphandler);
        }
    }
}
