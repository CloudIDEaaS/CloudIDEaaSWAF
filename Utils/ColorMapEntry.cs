using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    [DebuggerDisplay("{ DebugInfo }")]
    public class ColorMapEntry
    {
        public int Row { get; }
        public int Column { get; }
        public Color Color { get; }

        public ColorMapEntry(int row, int col, Color color)
        {
            Row = row;
            Column = col;
            Color = color;
        }

        public string DebugInfo
        {
            get
            {
                return string.Format("{0} [{1},{2}]", this.Color, this.Row, this.Column);
            }
        }
    }
}
