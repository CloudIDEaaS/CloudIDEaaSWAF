using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace EdgeDetection
{
    public class PointColorPair
    {
        public Point Point { get; set; }
        public Color Color1 { get; set; }
        public Color Color2 { get; set; }
        public double Diff { get; set; }
        public double AbsDiff { get; set; }
        public double StepDiff { get; set; }

        public PointColorPair(Point point, Color c1, Color c2, double diff, double absDiff, double stepDiff)
        {
            Point = point;
            Diff = diff;
            AbsDiff = absDiff;
            StepDiff = stepDiff;
        }
    }
}
