using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace AspNet.MVC.SelfCode.MVC
{
    public class ControllerContext
    {
        public virtual HttpContextBase HttpContext { get; set; }
    }
}
