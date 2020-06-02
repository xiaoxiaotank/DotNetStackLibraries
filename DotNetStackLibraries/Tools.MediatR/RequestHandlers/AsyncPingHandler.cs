using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MediatR
{
    /// <summary>
    /// 实现 <see cref="IRequestHandler{T}"/> 的Handler均为单播Handler
    /// --异步有返回值的Handler
    /// <remark>
    /// <br/>同时存在异步与同步的单播Handler，则以异步为准
    /// </remark>
    /// </summary>
    public class AsyncPingHandler : IRequestHandler<Ping, string>
    {
        public Task<string> Handle(Ping request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException(nameof(AsyncPingHandler));
            return Task.FromResult("Pong");
        }
    }
}
