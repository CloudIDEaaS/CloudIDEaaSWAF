using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EdgeDetection
{
    public static class ExtensionMethods
    {
        public static List<IGrouping<int, List<T>>> GroupWhile<T>(this IEnumerable<T> seq, Func<T, T, bool> condition)
        {
            T prev = seq.First();
            List<T> list = new List<T>() { prev };
            var outerList = new List<List<T>>();
            List<IGrouping<int, List<T>>> result;

            foreach (T item in seq.Skip(1))
            {
                if (condition(prev, item) == false)
                {
                    outerList.Add(list);
                    list = new List<T>();
                }

                list.Add(item);
                prev = item;
            }

            if (!outerList.Contains(list))
            {
                outerList.Add(list);
            }

            result = outerList.Where(l => l.Count > 1).GroupBy(l => l.Count).ToList();

            return result;
        }
    }
}
