using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public class Emoji
    {
        public string Name { get; set; }
        public string Chars { get; set; }
        public string Src { get; set; }
        public Image Image { get; set; }
        public string Category { get; internal set; }
    }
}
