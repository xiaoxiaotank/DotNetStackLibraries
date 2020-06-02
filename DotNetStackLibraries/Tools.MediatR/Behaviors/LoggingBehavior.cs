using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Tools.MediatR.Behaviors
{
    /// <summary>
    /// <see cref="IPipelineBehavior{TRequest,TResponse}"/> 仅能与 <see cref="IRequestHandler{T,T}"/> 配合使用，对其他无效
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            MyConsole.WriteLine($"Log：{typeof(TRequest)}", ConsoleColor.Green);
            return await next();
        }
    }
}
