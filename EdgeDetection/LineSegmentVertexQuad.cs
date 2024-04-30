using Accord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdgeDetection
{
    public class LineSegmentVertexQuad
    {
        public LineSegmentPair LineSegmentPair1 { get; set; }
        public LineSegmentPair LineSegmentPair2 { get; set; }
        public Point JoiningPoint { get; set; }
    }
}
