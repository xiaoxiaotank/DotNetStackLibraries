using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MediatR
{
    public class RedMessageConsoleHandler : INotificationHandler<MessageNotification>
    {
        public Task Handle(MessageNotification notification, CancellationToken cancellationToken)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(notification.Message);

            return Task.CompletedTask;
        }
    }
}
