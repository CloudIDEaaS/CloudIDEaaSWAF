using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    internal static class NumberExtensions
    {
        public static void Countdown(this int start, Action<int> loop)
        {
            for (var x = start; x != 0; x--)
            {
                loop(x);
            }
        }

        public static void Countdown(this int start, int target, Action<int> loop)
        {
            for (var x = target; x != start; x--)
            {
                loop(x);
            }
        }

    }
}
