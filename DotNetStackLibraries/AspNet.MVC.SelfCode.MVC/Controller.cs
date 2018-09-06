using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspNet.MVC.SelfCode.MVC
{
    public class Controller : ControllerBase
    {
        private Type _type;

        public IActionInvoker ActionInvoker { get; set; }

        public Controller()
        {
            _type = GetType();
            ActionInvoker = new ControllerActionInvoker();
        }

        protected override void ExecuteCore(string actionName)
        {
            ActionInvoker.InvokeAction(_type, actionName);
        }

        public ContentResult Content(string msg)
        {
            return new ContentResult() { Content = msg };
        }
    }
}
