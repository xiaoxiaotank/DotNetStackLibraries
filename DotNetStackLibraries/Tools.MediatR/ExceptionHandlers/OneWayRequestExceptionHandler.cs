using MediatR;
using MediatR.Pipeline;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Tools.MediatR.Requests;

namespace Tools.MediatR.ExceptionHandlers
{
    public class OneWayRequestExceptionHandler : IRequestExceptionHandler<OneWay, Unit, NotImplementedException>
    {
        public Task Handle(OneWay request, NotImplementedException exception, RequestExceptionHandlerState<Unit> state, CancellationToken cancellationToken)
        {
            MyConsole.WriteLine("OneWayRequestExceptionHandler");
            state.SetHandled();
            return Task.CompletedTask;
        }
    }
}
