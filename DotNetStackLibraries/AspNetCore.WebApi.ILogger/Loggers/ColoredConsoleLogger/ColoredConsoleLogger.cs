using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace AspNetCore.WebApi.ILogger.Loggers.ColoredConsoleLogger
{
    public class ColoredConsoleLogger : Microsoft.Extensions.Logging.ILogger
    {
        private readonly string _name;
        private readonly ColoredConsoleLoggerConfiguration _config;

        public ColoredConsoleLogger(string name, ColoredConsoleLoggerConfiguration config)
        {
            _name = name;
            _config = config;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel == _config.LogLevel;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel)) return;

            var color = Console.ForegroundColor;
            Console.ForegroundColor = _config.Color;
            Console.WriteLine($"{logLevel} - {_name} - {formatter(state, exception)}");
            Console.ForegroundColor = color;
        }
    }
}
