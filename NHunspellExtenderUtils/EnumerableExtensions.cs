using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    internal static class EnumerableExtensions
    {
        public static string ToDelimitedList(this IEnumerable list, string delimiter)
        {
            var text = string.Empty;
            var count = list.Cast<object>().Count();

            for (var x = 0; x < count; x++)
            {
                var item = list.Cast<object>().ElementAt(x).ToString();

                if (x < count - 1)
                {
                    item += delimiter;
                }

                text += item;
            }

            return text;
        }
    }
}
