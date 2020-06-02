using MediatR.Pipeline;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Tools.MediatR.ExceptionActions
{
    public class LogExceptionAction<TRequest, TException> : IRequestExceptionAction<TRequest, TException>
        where TException : Exception
    {
        public Task Execute(TRequest request, TException exception, CancellationToken cancellationToken)
        {
            MyConsole.WriteLine($"Exception Log: {exception.Message} 发生了异常，异常类型：{typeof(TException)}");
            return Task.CompletedTask;
        }
    }
}
