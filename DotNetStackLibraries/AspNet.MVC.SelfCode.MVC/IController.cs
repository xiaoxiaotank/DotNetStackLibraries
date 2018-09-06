using AspNet.MVC.SelfCode.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace AspNet.MVC.SelfCode.MVC
{
    public interface IController
    {
        void Execute(RequestContext requestContext);
    }
}
