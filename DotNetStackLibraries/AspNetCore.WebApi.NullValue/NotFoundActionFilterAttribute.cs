using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCore.WebApi.NullValue
{
    /// <summary>
    /// 使用404 Not Found代替204 No Content
    /// 下方两个重写只需要一个就可以
    /// 
    /// 
    /// 
    /// ASP.NET Core Mvc中过滤器的短路机制(即在任何一个过滤器中对Result赋值都会导致程序跳过管道中剩余的过滤器)
    /// 例如：在Action上配置了A过滤器和B过滤器，如果A中对Result赋值了并且是A先执行，那么B过滤器就不会被执行
    /// </summary>
    public class NotFoundActionFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Result is ObjectResult objectResult && objectResult.Value == null)
            {
                context.Result = new NotFoundResult();
            }
        }

        public override void OnResultExecuting(ResultExecutingContext context)
        {
            if (context.Result is ObjectResult objectResult && objectResult.Value == null)
            {
                context.Result = new NotFoundResult();
            }
        }
    }
}
