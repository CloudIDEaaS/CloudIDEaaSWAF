using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Utils
{
    public class ConsoleColorizer : IDisposable
    {
        private ConsoleColor oldColor;

        public ConsoleColorizer(ConsoleColor color)
        {
            oldColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
        }

        public static ConsoleColorizer UseColor(ConsoleColor color) 
        {
            return new ConsoleColorizer(color);
        }

        public void Dispose()
        {
            Console.ForegroundColor = oldColor;
        }
    }
}
