using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspNet.MVC.SelfCode.MVC
{
    public interface IActionInvoker
    {
        bool InvokeAction(Type type, string actionName);
    }
}
