using Accord.Math.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdgeDetection
{
    public class LineSegmentPair
    {
        public LineSegment LineSegment1 { get; set; }
        public LineSegment LineSegment2 { get; set; }

        public LineSegmentPair(LineSegment lineSegment1, LineSegment lineSegment2)
        {
            LineSegment1 = lineSegment1;
            LineSegment2 = lineSegment2;
        }
    }
}
