using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Diagnostics;

namespace Utils
{
    [DebuggerDisplay(" { DebugInfo } ")]
    public struct Line
    {
        public Point PointStart;
        public Point PointEnd;
        public int x1 { get; }
        public int x2 { get; }
        public int y1 { get; }
        public int y2 { get; }
        public double A { get; private set; }
        public double B { get; private set; }
        public double C { get; private set; }

        public Line(Point start, Point end)
        {
            this.PointStart = start;
            this.PointEnd = end;

            this.x1 = start.X;
            this.y1 = start.Y;
            this.x2 = end.X;
            this.y2 = end.Y;

            A = end.Y - start.Y;
            B = start.X - end.X;
            C = A * start.X + B * start.Y;
        }

        public Line(int x1, int y1, int x2, int y2) : this(new Point(x1, y1), new Point(x2, y2))
        {
        }

        public static Line Empty
        {
            get
            {
                return new Line(Point.Empty, Point.Empty);
            }
        }

        public string DebugInfo
        {
            get
            {
                return string.Format("{0} -> {1}", this.PointStart, this.PointEnd);
            }
        }

        public static bool operator ==(Line a, Line b)
        {
            return Equals(a, b);
        }

        public static bool operator !=(Line a, Line b)
        {
            return !Equals(a, b);
        }

        public override bool Equals(object obj)
        {
            return Equals(this, obj);
        }

        // kn - best approach to equality comparisons

        private static bool Equals(Line a, Line b)
        {
            bool equals;

            if (CompareExtensions.CheckNullEquality(a, b, out equals))
            {
                return equals;
            }
            else
            {
                if (a.PointStart != b.PointStart || a.PointEnd != b.PointEnd)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
    }
}
