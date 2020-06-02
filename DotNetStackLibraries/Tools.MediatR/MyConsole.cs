using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tools.MediatR
{
    public static class MyConsole
    {
        public static void WriteLine(string value, ConsoleColor color = ConsoleColor.Blue)
        {
            var origColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(value);
            Console.ForegroundColor = origColor;
        }
    }
}
