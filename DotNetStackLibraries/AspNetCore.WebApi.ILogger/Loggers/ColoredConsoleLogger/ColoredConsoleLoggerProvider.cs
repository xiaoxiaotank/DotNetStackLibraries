using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCore.WebApi.ILogger.Loggers.ColoredConsoleLogger
{
    public class ColoredConsoleLoggerProvider : ILoggerProvider
    {
        private readonly ColoredConsoleLoggerConfiguration _config;

        public ColoredConsoleLoggerProvider(ColoredConsoleLoggerConfiguration config)
        {
            _config = config;
        }

        public Microsoft.Extensions.Logging.ILogger CreateLogger(string categoryName)
        {
            return new ColoredConsoleLogger(categoryName, _config);
        }

        public void Dispose()
        {
            
        }
    }
}
