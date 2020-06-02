using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tools.MediatR.RequestHandlers
{
    /// <summary>
    /// 实现 <see cref="RequestHandler{T,T}"/> 的Handler均为单播Handler
    /// --同步有返回值的Handler
    /// <remark>
    /// <br/>同时存在异步与同步的单播Handler，则以异步为准
    /// </remark>
    /// </summary>
    public class SyncPingHandler : RequestHandler<Ping, string>
    {
        protected override string Handle(Ping request)
        {
            return string.Empty;
        }
    }
}
