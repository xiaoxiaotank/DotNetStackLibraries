using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Tools.MediatR.NotificationHandlers
{
    public class ErrorMessageConsoleHandler : INotificationHandler<PublishMessage>
    {
        public Task Handle(PublishMessage notification, CancellationToken cancellationToken)
        {
            //throw new NotImplementedException();
            return Task.CompletedTask;
        }
    }
}
