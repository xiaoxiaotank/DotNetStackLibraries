using MediatR.Pipeline;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Tools.MediatR.Requests;

namespace Tools.MediatR.ExceptionActions
{
    #region 通过实现接口
    public class OneWayRequestExceptionAction : IRequestExceptionAction<OneWay, NotImplementedException>
    {
        public Task Execute(OneWay request, NotImplementedException exception, CancellationToken cancellationToken)
        {
            MyConsole.WriteLine(nameof(OneWayRequestExceptionAction));
            return Task.CompletedTask;
        }
    }

    public class OneWayRequestAllExceptionAction : IRequestExceptionAction<OneWay>
    {
        public Task Execute(OneWay request, Exception exception, CancellationToken cancellationToken)
        {
            MyConsole.WriteLine(nameof(OneWayRequestAllExceptionAction));
            return Task.CompletedTask;
        }
    }
    #endregion

    #region 通过继承抽象类
    public class AsyncOneWayRequestAllExceptionAction : AsyncRequestExceptionAction<OneWay>
    {
        protected override Task Execute(OneWay request, Exception exception, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }

    public class SyncOneWayRequestExceptionAction : RequestExceptionAction<OneWay, NotImplementedException>
    {
        protected override void Execute(OneWay request, NotImplementedException exception)
        {
        }
    }

    public class SyncOneWayRequestAllExceptionAction : RequestExceptionAction<OneWay>
    {
        protected override void Execute(OneWay request, Exception exception)
        {
        }
    }
    #endregion
}
