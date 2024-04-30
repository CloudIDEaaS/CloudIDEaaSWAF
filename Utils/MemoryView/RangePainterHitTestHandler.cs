using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Utils.MemoryView
{
    [Flags]
    public enum HitTestArea
    {
        None,
        Interior = 1,
        LeftEdge = 1 << 1,
        TopEdge = 1 << 2,
        RightEdge = 1 << 3,
        BottomEdge = 1 << 4,
        LeftTopCorner = LeftEdge | TopEdge,
        RightTopCorner = RightEdge | TopEdge,
        RightBottomCorner = RightEdge | BottomEdge,
        LeftBottomCorner = LeftEdge | BottomEdge
    }

    public delegate void RangePainterHitTestHandler(object sender, RangePainterHitTestEventArgs e);

    public class RangePainterHitTestEventArgs
    {
        public Point Point { get; private set; }
        public HitTestArea HitTestArea { get; private set; }

        public RangePainterHitTestEventArgs(Point point, HitTestArea hitTestArea)
        {
            this.Point = point;
            this.HitTestArea = hitTestArea;
        }
    }
}
