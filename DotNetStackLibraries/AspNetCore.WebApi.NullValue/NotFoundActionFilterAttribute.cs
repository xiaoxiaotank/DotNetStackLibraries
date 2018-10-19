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
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
        }

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

        public override void OnResultExecuted(ResultExecutedContext context)
        {
            base.OnResultExecuted(context);
        }

        //异步与同步的方法只能取其一，要么异步，要么同步的，否则只会调用异步的

        //public override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        //{
        //    //return base.OnActionExecutionAsync(context, next);
        //    return Task.FromResult(0);
        //}

        //public override Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        //{
        //    //基类内部主动调用了OnResultExecuting，所以看起来好像重写了异步方法也会执行同步方法
        //    //return base.OnResultExecutionAsync(context, next);
        //    return Task.FromResult(0);
        //}


    }
}
