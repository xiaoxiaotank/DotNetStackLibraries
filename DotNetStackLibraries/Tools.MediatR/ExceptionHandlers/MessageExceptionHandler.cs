using MediatR;
using MediatR.Pipeline;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Tools.MediatR.ExceptionHandlers
{
    public class MessageExceptionHandler : IRequestExceptionAction<PublishMessage, NotImplementedException>
    {
        public Task Execute(PublishMessage request, NotImplementedException exception, CancellationToken cancellationToken)
        {
            MyConsole.WriteLine("MessageExceptionHandler");
            //throw new NotImplementedException();
            return Task.CompletedTask;
        }
    }
}
