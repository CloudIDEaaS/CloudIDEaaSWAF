using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils
{
    public static class Math_
    {
        public static int AbsoluteDiff(int n, int other)
        {
            return Math.Abs(n - other);
        }

        public static double Average(int a, int b)
        {
            return (double)(a + b) / 2;
        }

        public static decimal Average(decimal a, decimal b)
        {
            return (a + b) / 2;
        }

        public static double Average(double a, double b)
        {
            return (double)(a + b) / 2;
        }

        public static double StandardDeviation<T>(this IEnumerable<T> enumerable, Func<T, double> selector)
        {
            double sum = 0;
            double average = enumerable.Average(selector);
            int n = 0;

            foreach (var item in enumerable)
            {
                double diff = selector(item) - average;
                sum += diff * diff;
                n++;
            }

            return n == 0 ? 0 : Math.Sqrt(sum / n);
        }

        public static IEnumerable<T> SkipOutliers<T>(this IEnumerable<T> enumerable, double k, Func<T, double> selector)
        {
            // Duplicating a SD code to avoid calculating an average twice.
            double sum = 0;
            double average = enumerable.Average(selector);
            int n = 0;

            foreach (var item in enumerable)
            {
                double diff = selector(item) - average;
                sum += diff * diff;
                n++;
            }

            double SD = n == 0 ? 0 : Math.Sqrt(sum / n);
            double delta = k * SD;

            foreach (var item in enumerable)
            {
                if (Math.Abs(selector(item) - average) <= delta)
                {
                    yield return item;
                }
            }
        }
    }
}
