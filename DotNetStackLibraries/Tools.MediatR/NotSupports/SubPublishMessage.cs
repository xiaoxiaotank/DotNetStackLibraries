using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tools.MediatR.Notifications
{
    /// <summary>
    /// 使用子类SubPublishMessage时，PublishMessage的Handler以及AnythingHandler都不会对其进行处理
    /// </summary>
    public class SubPublishMessage : PublishMessage
    {
        public SubPublishMessage(string message) : base(message)
        {
        }

        public string SubMessage { get; set; }
    }
}
