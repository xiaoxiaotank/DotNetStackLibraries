using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MediatR
{
    /// <summary>
    /// 实现 <see cref="INotificationHandler{T}"/> 的Handler均为多播处理
    /// </summary>
    public class YellowMessageConsoleHandler : INotificationHandler<PublishMessage>
    {
        public Task Handle(PublishMessage notification, CancellationToken cancellationToken)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(notification.Message);

            return Task.CompletedTask;
        }
    }
}
