using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils.MemoryView
{
    public static class TextExtensions
    {
        public static string Sanitize(this string text)
        {
            var builder = new StringBuilder();

            for (var x = 0; x < text.Length; x++)
            {
                var _byte = text[x];
                char _char = (char)_byte;
                string append;

                append = (_byte < 0x20 ? "." : new string(_char, 1));
                builder.Append(append);
            }

            return builder.ToString();
        }
    }
}
