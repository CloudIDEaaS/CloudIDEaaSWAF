using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Utils.GlyphDrawing
{
    public class ControlGlyph
    {
        public string Name { get; set; }
        public event EventHandler Click;
        public char ImageChar { get; set; }
        public Font Font { get; set; }
        public Size Size { get; set; }
        public Point Location { get; set; }
        public Brush Brush { get; set; }
        public Point Offset { get; set; }
        public Point TextOffset { get; set; }
        public bool Hovered { get; set; }
        public bool Clicked { get; set; }
        public bool ShowOnFocusHoverOnly { get; set; }
        public bool FocusedOrHovered { get; set; }

        internal void PerformClick()
        {
            if (Click != null)
            {
                Click(this, EventArgs.Empty);
            }
        }

    }
}
