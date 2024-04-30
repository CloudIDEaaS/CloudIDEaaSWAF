using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils
{
    public class TextMatrix
    {
        private StringBuilder builder;
        private int nWindowCharsX;

        public TextMatrix(int capacity, int nWindowCharsX)
        {
            builder = new StringBuilder('\0'.Repeat(capacity));
            this.nWindowCharsX = nWindowCharsX;
        }

        public string Text
        {
            get
            {
                return builder.ToString();
            }
        }

        public char this[int x, int y]
        {
            get
            {
                return builder[(y * nWindowCharsX) + x];
            }

            set
            {
                builder[(y * nWindowCharsX) + x] = value;
            }
        }

        public string GetString(int x, int y)
        {
            var startPos = (y * nWindowCharsX) + x;

            return builder.ToString(startPos, builder.Length - startPos);
        }

        public static implicit operator string(TextMatrix matrix)
        {
            if (matrix != null)
            {
                return matrix.Text;
            }

            return string.Empty;
        }
    }
}
