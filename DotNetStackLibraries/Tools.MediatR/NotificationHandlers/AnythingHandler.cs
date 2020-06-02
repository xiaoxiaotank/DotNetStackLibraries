using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Tools.MediatR.NotificationHandlers
{
    /// <summary>
    /// 处理所有的多播请求
    /// </summary>
    public class AnythingHandler : INotificationHandler<INotification>
    {
        public Task Handle(INotification notification, CancellationToken cancellationToken)
        {
            Console.WriteLine("Anything...");
            return Task.CompletedTask;
        }
    }
}
