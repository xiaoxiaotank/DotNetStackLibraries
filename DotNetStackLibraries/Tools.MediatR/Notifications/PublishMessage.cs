using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediatR
{
    /// <summary>
    /// 实现 <see cref="INotification"/> 接口的类均作为发布者（多播）的请求参数
    /// </summary>
    public class PublishMessage : INotification
    {
        public PublishMessage(string message)
        {
            Message = message;
        }

        public string Message { get; }
    }
}
