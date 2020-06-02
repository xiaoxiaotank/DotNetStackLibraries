using MediatR.Pipeline;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Tools.MediatR.Processors
{
    public class CommonRequestPostProcessor<TRequest, TResponse> : IRequestPostProcessor<TRequest, TResponse>
    {
        public Task Process(TRequest request, TResponse response, CancellationToken cancellationToken)
        {
            MyConsole.WriteLine("Processor Done.............", ConsoleColor.Gray);
            return Task.CompletedTask;
        }
    }
}
