using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.TextEditElements
{
    public class Character : TextElement
    {
        public char Char { get; }

        public Character(char ch, Rectangle rectangle)
        {
            this.Char = ch;
            this.Rect = rectangle;
        }
    }
}
