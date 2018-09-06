using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace AspNet.MVC.SelfCode.MVC
{
    public class ContentResult : ActionResult
    {
        public string Content { get; set; }

        public override void ExecuteResult()
        {
            HttpContext.Current.Response.Write(Content);
        }
    }
}
