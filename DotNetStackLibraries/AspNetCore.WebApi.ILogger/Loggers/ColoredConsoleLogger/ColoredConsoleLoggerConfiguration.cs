﻿using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCore.WebApi.ILogger.Loggers.ColoredConsoleLogger
{
    public class ColoredConsoleLoggerConfiguration
    {
        public LogLevel LogLevel { get; set; } = LogLevel.Warning;
        public ConsoleColor Color { get; set; } = ConsoleColor.Yellow;
    }
}
