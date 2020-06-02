using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tools.MediatR.Requests;

namespace Tools.MediatR.RequestHandlers
{
    /// <summary>
    /// 实现 <see cref="RequestHandler{T}"/> 的Handler均为单播Handler
    /// --同步无返回值的Handler
    /// </summary>
    public class SyncOneWayHandler : RequestHandler<OneWay>
    {
        protected override void Handle(OneWay request)
        {
           
        }
    }
}
