using MediatR.Pipeline;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Tools.MediatR.ExceptionHandlers
{
    /// <summary>
    /// 处理所有Handler的指定异常，优先级最高，所有Handler发生异常后首先会进入该方法
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    /// <typeparam name="TException"></typeparam>
    public class CommonExceptionHandler<TRequest, TResponse, TException> : IRequestExceptionHandler<TRequest, TResponse, TException>
        where TException : Exception
    {
        public Task Handle(TRequest request, TException exception, RequestExceptionHandlerState<TResponse> state, CancellationToken cancellationToken)
        {
            MyConsole.WriteLine($"Common Spec Exception：{typeof(TRequest)}");
            return Task.CompletedTask;
        }
    }
}
