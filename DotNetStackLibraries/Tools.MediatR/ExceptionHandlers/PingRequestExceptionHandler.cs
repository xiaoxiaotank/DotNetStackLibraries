using MediatR;
using MediatR.Pipeline;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Tools.MediatR.ExceptionHandlers
{
    #region 通过实现接口的方式
    /// <summary>
    /// （相对于处理全部异常的Handler，该Handler会优先执行）异步针对请求参数继承于（包括自身） <see cref="Ping"/> 的单播请求进行指定异常处理
    /// </summary>
    public class PingRequestExceptionHandler : IRequestExceptionHandler<Ping, string, NotImplementedException>
    {
        public Task Handle(Ping request, NotImplementedException exception, RequestExceptionHandlerState<string> state, CancellationToken cancellationToken)
        {
            MyConsole.WriteLine($"PingRequestExceptionHandler:{exception.Message}");
            // 标志异常已处理，不再向上传递
            //state.SetHandled("Pong!!! 被Exception处理了");
            return Task.CompletedTask;
        }
    }

    /// <summary>
    /// 异步也可以针对所有异常进行处理，实际上继承于 IRequestExceptionHandler{TRequest, TResponse, Exception}
    /// </summary>
    public class PingRequestAllExceptionHandler : IRequestExceptionHandler<Ping, string>
    {
        public Task Handle(Ping request, Exception exception, RequestExceptionHandlerState<string> state, CancellationToken cancellationToken)
        {
            MyConsole.WriteLine($"PingRequestAllExceptionHandler:{exception.Message}");
            state.SetHandled("Pong!!! 被AllException处理了");
            return Task.CompletedTask;
        }
    }
    #endregion

    #region 通过实现抽象类的方式

    /// <summary>
    /// 异步针对请求参数继承于（包括自身） <see cref="Ping"/> 的单播请求进行全部异常处理
    /// </summary>
    public class AsyncPingRequestAllExceptionHandler : AsyncRequestExceptionHandler<Ping, string>
    {
        protected override Task Handle(Ping request, Exception exception, RequestExceptionHandlerState<string> state, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }

    /// <summary>
    /// 同步针对请求参数继承于（包括自身） <see cref="Ping"/> 的单播请求进行指定异常处理
    /// </summary>
    public class SyncPingRequestExceptionHandler : RequestExceptionHandler<Ping, string, NotImplementedException>
    {
        protected override void Handle(Ping request, NotImplementedException exception, RequestExceptionHandlerState<string> state)
        {

        }
    }

    /// <summary>
    /// 同步针对请求参数继承于（包括自身） <see cref="Ping"/> 的单播请求进行全部异常处理
    /// </summary>
    public class SyncPingRequestAllExceptionHandler : RequestExceptionHandler<Ping, string>
    {
        protected override void Handle(Ping request, Exception exception, RequestExceptionHandlerState<string> state)
        {
           
        }
    }


    #endregion

}
