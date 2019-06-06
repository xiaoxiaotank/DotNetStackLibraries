using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediatR
{
    public class MessageNotification : INotification
    {
        public MessageNotification(string message)
        {
            Message = message;
        }

        public string Message { get; }
    }
}
