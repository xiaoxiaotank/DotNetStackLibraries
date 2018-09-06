using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Compilation;
using AspNet.MVC.SelfCode.Routing;

namespace AspNet.MVC.SelfCode.MVC
{
    public abstract class ControllerBase : IController
    {
        private static readonly ICollection<Type> _types;

        static ControllerBase()
        {
            _types = new Collection<Type>();
            var assms = BuildManager.GetReferencedAssemblies();
            foreach (Assembly assembly in assms)
            {
                var types = assembly.GetTypes();
                foreach (var type in types)
                {
                    _types.Add(type);
                }
            }
        }

        protected abstract void ExecuteCore(string actionName);

        public void Execute(RequestContext requestContext)
        {
            var routes = requestContext.HttpContext.Request.AppRelativeCurrentExecutionFilePath
                .Substring(2)
                .Split('/');
            var control = string.Empty;
            var action = string.Empty;
            if (routes.Length == 2)
            {
                control = routes[0];
                action = routes[1];
            }
            else
            {
                control = "Home";
                action = "Index";
            }
           
            var controlType = _types.SingleOrDefault(o => o.Name == control + "Controller");
            if (controlType != null)
            {
                var baseControl = Activator.CreateInstance(controlType) as ControllerBase;
                if (baseControl != null) baseControl.ExecuteCore(action);
            }
        }
    }
}
