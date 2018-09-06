using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspNet.MVC.SelfCode.MVC
{
    public class ControllerActionInvoker : IActionInvoker
    {
        public bool InvokeAction(Type type, string actionName)
        {
            var control = Activator.CreateInstance(type) as Controller;
            var controlType = control.GetType();
            var rst = controlType.GetMethod(actionName).Invoke(control, null) as ActionResult;
            rst.ExecuteResult();

            return true;
        }
    }
}
