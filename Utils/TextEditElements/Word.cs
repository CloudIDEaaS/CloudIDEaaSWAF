using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.TextEditElements
{
    public class Word : TextElement
    {
        public LinkedList<Character> Characters { get; }

        public Word(string text, Rectangle rectangle, LinkedList<Character> chars)
        {
            this.Characters = chars;
            this.Rect = rectangle;
        }
    }
}
