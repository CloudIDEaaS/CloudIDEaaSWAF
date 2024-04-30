using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    internal static class StringExtensions
    {
        private static string BytOrderMarkUtf8;

        static StringExtensions()
        {
            BytOrderMarkUtf8 = Encoding.UTF8.GetString(Encoding.UTF8.GetPreamble());
        }

        public static string Substitute(this string text, int insertionPoint, int length, string substitute)
        {
            var left = text.Left(insertionPoint);
            var right = text.Right(text.Length - (insertionPoint + length));

            return left + substitute + right;
        }

        public static string Left(this string text, int count)
        {
            if (text.Length > count)
            {
                return text.Substring(0, count);
            }
            else
            {
                return text;
            }
        }

        public static string Right(this string text, int count)
        {
            if (text.Length > count)
            {
                return text.Substring(text.Length - count, count);
            }
            else
            {
                return text;
            }
        }

        public static void RemoveEnd(this StringBuilder builder, int length)
        {
            builder.Remove(builder.Length - length, length);
        }

        public static string Repeat(this string value, int count)
        {
            if (value.Length == 1)
            {
                return Repeat(value[0], count);
            }
            else
            {
                var builder = new StringBuilder();

                count.Countdown(n =>
                {
                    builder.Append(value);
                });

                return builder.ToString();
            }
        }

        public static string Repeat(this char value, int count)
        {
            return new string(value, count);
        }

        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }

        public static string RemoveStart(this string text, int count)
        {
            return text.Remove(0, count);
        }

        public static string RemoveBOM(this string text)
        {
            if (text.StartsWith(BytOrderMarkUtf8))
            {
                text = text.RemoveStart(BytOrderMarkUtf8.Length);
            }

            return text;
        }
    }
}
