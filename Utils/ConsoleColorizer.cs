using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Utils
{
    public class ConsolePositioner : IDisposable
    {
        private (int left, int top) oldPosition;

        public ConsolePositioner((int left, int top) position)
        {
            oldPosition = position;
            Console.SetCursorPosition(position.left, position.top);
        }

        public static ConsolePositioner Set((int left, int top) position) 
        {
            return new ConsolePositioner(position);
        }

        public static ConsolePositioner Set(int left, int top)
        {
            return new ConsolePositioner((left, top));
        }

        public void Dispose()
        {
            Console.SetCursorPosition(oldPosition.left, oldPosition.top);
        }
    }
}
