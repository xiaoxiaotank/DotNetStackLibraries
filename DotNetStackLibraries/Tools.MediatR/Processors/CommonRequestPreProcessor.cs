using MediatR.Pipeline;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Tools.MediatR.Processors
{
    public class CommonRequestPreProcessor<TRequest> : IRequestPreProcessor<TRequest>
    {
        public Task Process(TRequest request, CancellationToken cancellationToken)
        {
            MyConsole.WriteLine("Start Processor.............", ConsoleColor.Gray);
            return Task.CompletedTask;
        }
    }
}
