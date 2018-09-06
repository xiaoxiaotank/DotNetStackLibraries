using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspNet.MVC.SelfCode.MVC
{
    public abstract class ActionResult
    {
        protected ActionResult()
        {

        }

        public abstract void ExecuteResult();
    }
}
