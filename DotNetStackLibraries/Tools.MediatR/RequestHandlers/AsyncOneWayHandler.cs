using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Tools.MediatR.Requests;

namespace Tools.MediatR.RequestHandlers
{
    /// <summary>
    /// 实现 <see cref="AsyncRequestHandler{T}"/> 的Handler均为单播Handler
    /// --异步无返回值的Handler
    /// </summary>
    public class AsyncOneWayHandler : AsyncRequestHandler<OneWay>
    {
        protected override Task Handle(OneWay request, CancellationToken cancellationToken)
        {
            MyConsole.WriteLine("AsyncOneWayHandler-------------", ConsoleColor.Cyan);
            throw new NotImplementedException(nameof(AsyncOneWayHandler));
            return Task.CompletedTask;
        }
    }
}
