using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace AspNet.MVC.SelfCode.Routing
{
    public class RequestContext
    {
        public virtual RouteData RouteData { get; set; }

        public virtual HttpContextBase HttpContext { get; set; }

    }
}
