using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Numerics;
using Accord;
using Accord.Math.Geometry;
using EdgeDetection;
using EdgeDetection;
using Point = System.Drawing.Point;

namespace BitmapedTextures
{
    public struct PointPair
    {
        public Point Start;
        public Point End;

        public PointPair(Point start, Point end)
        {
            Start = start;
            End = end;
        }

        public int Length
        {
            get
            {
                var p1 = this.Start;
                var p2 = this.End;
                var distance = Math.Round(Math.Sqrt(Math.Pow((p2.X - p1.X), 2) + Math.Pow((p2.Y - p1.Y), 2)), 1);

                return (int) distance;
            }
        }
    }

    public interface ScanStrategy
    {
        double Scan(int width, int height, int maxedges, int tolerance);
        ColorSubtracter Strategy { get; set; }
    }

    public class HScan : ScanStrategy
    {
        private BitmapAsTexture bmptex;
        private Graphics graphics;
        private Color edgecolor;
        private object syncObj = new object();

        public ColorSubtracter Strategy { get; set; }

        public HScan(BitmapAsTexture b, Graphics g, Color e, ColorSubtracter c)
        {
            lock (syncObj)
            {
                bmptex = b;
                graphics = g;
                edgecolor = e;
                Strategy = c;
            }
        }

        public double Scan(int width, int height, int maxedges, int tolerance)
        {
            lock (syncObj)
            {
                DateTime start = DateTime.Now;

                foreach (PointPair pointpair in GetPointPairs(width, height))
                {
                    Bitmap tex1d = null, onlyedges = null;

                    try
                    {
                        tex1d = bmptex.Get1DTexture(pointpair.Start, pointpair.End);
                        onlyedges = bmptex.ShowEdgePoints(tex1d, maxedges, Strategy, tolerance, edgecolor);
                        graphics.DrawImage(onlyedges, pointpair.Start);
                    }
                    finally
                    {
                        if (tex1d != null)
                        {
                            tex1d.Dispose();
                        }
                        if (onlyedges != null)
                        {
                            onlyedges.Dispose();
                        }
                    }
                }

                return (DateTime.Now - start).TotalMilliseconds;
            }
        }

        public List<LineSegmentPair> GetEdges(int width, int height, int maxedges, int tolerance)
        {
            var lineSegments = new List<LineSegment>();
            var lineSegmentsCopy = new List<LineSegment>();
            var lineSegmentPairs = new List<LineSegmentPair>();
            var allPointColorPairs = new List<PointColorPair>();
            var midPoint = new Accord.Point(((float)width) / 2f, ((float)height) / 2f);
            IEnumerable <IGrouping<int, PointColorPair>> groupings;
            List<List<PointColorPair>> colorPairLists;
            Color color;

            lock (syncObj)
            {
                DateTime start = DateTime.Now;
                var y = 0;

                color = Color.Black;

                foreach (PointPair pointPair in GetPointPairs(width, height))
                {
                    Bitmap tex1d = null, onlyedges = null;

                    try
                    {
                        List<PointColorPair> pointColorPairs;

                        tex1d = bmptex.Get1DTexture(pointPair.Start, pointPair.End);
                        pointColorPairs = bmptex.GetEdgePoints(tex1d, maxedges, Strategy, tolerance, color, y);

                        allPointColorPairs.AddRange(pointColorPairs);
                    }
                    finally
                    {
                        if (tex1d != null)
                        {
                            tex1d.Dispose();
                        }
                        if (onlyedges != null)
                        {
                            onlyedges.Dispose();
                        }
                    }

                    y++;
                }

                groupings = allPointColorPairs.Where(p => p.Color1.GetBrightness() < .005 || p.Color2.GetBrightness() < .005).GroupBy(p => p.Point.X).ToList();
                colorPairLists = groupings.Select(g => new { X = g.Key, Groups = g.GroupWhile((a, b) => b.Point.Y - a.Point.Y == 1) }).Select(a => new { x = a.X, Groups = a.Groups.OrderByDescending(g => g.Key) }).Where(g => g.Groups.Count() > 0).OrderByDescending(g => g.Groups.Max(g2 => g2.Key)).Take(20).Select(g => g.Groups).SelectMany(g => g.ToList()).SelectMany(g => g.ToList()).OrderByDescending(p => p.Count).Take(20).ToList();

                // turn points into line segments

                foreach (var colorPairList in colorPairLists)
                {
                    var first = colorPairList.First();
                    var last = colorPairList.Last();
                    var pointFirst = new Accord.Point(first.Point.X, first.Point.Y);
                    var pointLast = new Accord.Point(last.Point.X, last.Point.Y);
                    var lineSegment = new LineSegment(pointFirst, pointLast);

                    lineSegments.Add(lineSegment);
                }

                // combine line segments on the same line

                while (lineSegments.Count > 0)
                {
                    foreach (var lineSegment in lineSegments.ToList())
                    {
                        var sameLineSegments = lineSegments.Where(s => s.Start.X == lineSegment.Start.X).ToList();

                        if (sameLineSegments.Count > 1)
                        {
                            var orderedSegments = sameLineSegments.OrderBy(s => Math.Min(s.Start.Y, s.End.Y)).ToList();
                            var minimumSegment = orderedSegments.First();
                            var maximumSegment = orderedSegments.Last();
                            var minimumPoint = new Accord.Point(minimumSegment.Start.X, Math.Min(minimumSegment.Start.Y, minimumSegment.End.Y));
                            var maximumPoint = new Accord.Point(maximumSegment.Start.X, Math.Max(maximumSegment.Start.Y, maximumSegment.End.Y));
                            var combinedSegment = new LineSegment(minimumPoint, maximumPoint);

                            lineSegmentsCopy.Add(combinedSegment);

                            lineSegments.RemoveAll(s => sameLineSegments.Contains(s));
                        }
                        else
                        {
                            lineSegments.Remove(lineSegment);
                            lineSegmentsCopy.Add(lineSegment);
                        }

                        break;
                    }
                }

                // find matching lines from midpoint

                lineSegments.AddRange(lineSegmentsCopy);

                while (lineSegments.Count > 0)
                {
                    foreach (var lineSegment in lineSegments.ToList())
                    {
                        var line = lineSegment.To<Line>();
                        var distance = line.DistanceToPoint(midPoint);
                        var pairSegments = lineSegments.Where(l => l != lineSegment && Math.Abs(l.To<Line>().DistanceToPoint(midPoint) - distance) < 3).ToList();
                        var pairSegment = pairSegments.FirstOrDefault();

                        if (pairSegment != null)
                        {
                            var lineSegmentPair = new LineSegmentPair(lineSegment, pairSegment);

                            lineSegments.Remove(pairSegment);

                            lineSegmentPairs.Add(lineSegmentPair);
                        }

                        lineSegments.Remove(lineSegment);

                        break;
                    }
                }

                return lineSegmentPairs;
            }
        }

        public IList<PointPair> GetPointPairs(int width, int height)
        {
            IList<PointPair> result = new List<PointPair>();

            for (int i = 0; i < height; i++)
            {
                result.Add(new PointPair(new Point(0, i), new Point(width, i)));
            }

            return result;
        }
    }

    public class VScan : ScanStrategy
    {
        private BitmapAsTexture bmptex;
        private Graphics graphics;
        private Color edgecolor;
        private object syncObj = new object();
        public ColorSubtracter Strategy { get; set; }

        public VScan(BitmapAsTexture b, Graphics g, Color e, ColorSubtracter c)
        {
            lock (syncObj)
            {
                bmptex = b;
                graphics = g;
                edgecolor = e;
                Strategy = c;
            }
        }

        public double Scan(int width, int height, int maxedges, int tolerance)
        {
            lock (syncObj)
            {
                DateTime start = DateTime.Now;

                foreach (PointPair pointpair in GetPointPairs(width, height))
                {
                    Bitmap tex1d = null, onlyedges = null;

                    try
                    {
                        tex1d = bmptex.Get1DTexture(pointpair.Start, pointpair.End);
                        onlyedges = bmptex.ShowEdgePoints(tex1d, maxedges, Strategy, tolerance, edgecolor);
                        onlyedges.RotateFlip(RotateFlipType.Rotate90FlipNone);
                        graphics.DrawImage(onlyedges, pointpair.Start);
                    }
                    finally
                    {
                        if (tex1d != null)
                        {
                            tex1d.Dispose();
                        }
                        if (onlyedges != null)
                        {
                            onlyedges.Dispose();
                        }
                    }
                }

                return (DateTime.Now - start).TotalMilliseconds;
            }
        }

        public List<LineSegmentPair> GetEdges(int width, int height, int maxedges, int tolerance)
        {
            var lineSegments = new List<LineSegment>();
            var lineSegmentsCopy = new List<LineSegment>();
            var lineSegmentPairs = new List<LineSegmentPair>();
            var allPointColorPairs = new List<PointColorPair>();
            var midPoint = new Accord.Point(((float)width) / 2f, ((float)height) / 2f);
            IEnumerable<IGrouping<int, PointColorPair>> groupings;
            List<List<PointColorPair>> colorPairLists;
            Color color;

            lock (syncObj)
            {
                DateTime start = DateTime.Now;
                var y = 0;

                color = Color.White;

                foreach (var pointPair in GetPointPairs(width, height))
                {
                    Bitmap tex1d = null, onlyedges = null;

                    try
                    {
                        List<PointColorPair> pointColorPairs;

                        tex1d = bmptex.Get1DTexture(pointPair.Start, pointPair.End);
                        pointColorPairs = bmptex.GetEdgePoints(tex1d, maxedges, Strategy, tolerance, color, y);

                        allPointColorPairs.AddRange(pointColorPairs);

                    }
                    finally
                    {
                        if (tex1d != null)
                        {
                            tex1d.Dispose();
                        }
                        if (onlyedges != null)
                        {
                            onlyedges.Dispose();
                        }
                    }

                    y++;
                }

                var test = allPointColorPairs.Where(p => p.Point.X == 850).ToList();

                groupings = allPointColorPairs.Where(p => p.Color1.GetBrightness() < .005 || p.Color2.GetBrightness() < .005).GroupBy(p => p.Point.X).ToList();
                var test2 = groupings.Where(g => g.Key == 850).ToList();

                colorPairLists = groupings.Select(g => new { X = g.Key, Groups = g.GroupWhile((a, b) => b.Point.Y - a.Point.Y == 1) }).Select(a => new { x = a.X, Groups = a.Groups.OrderByDescending(g => g.Key) }).Where(g => g.Groups.Count() > 0).OrderByDescending(g => g.Groups.Max(g2 => g2.Key)).Take(20).Select(g => g.Groups).SelectMany(g => g.ToList()).SelectMany(g => g.ToList()).OrderByDescending(p => p.Count).Take(20).ToList();

                // turn points into line segments

                foreach (var colorPairList in colorPairLists)
                {
                    var first = colorPairList.First();
                    var last = colorPairList.Last();
                    var pointFirst = new Accord.Point(first.Point.X, first.Point.Y);
                    var pointLast = new Accord.Point(last.Point.X, last.Point.Y);
                    var lineSegment = new LineSegment(pointFirst, pointLast);

                    lineSegments.Add(lineSegment);
                }

                // go from image laying on side, to portrait (normal view)

                lineSegments = lineSegments.Select(l => new LineSegment(new Accord.Point(l.Start.Y, l.Start.X), new Accord.Point(l.End.Y, l.End.X))).ToList();

                // combine line segments on the same line

                while (lineSegments.Count > 0)
                {
                    foreach (var lineSegment in lineSegments.ToList())
                    {
                        var sameLineSegments = lineSegments.Where(s => s.Start.Y == lineSegment.Start.Y).ToList();

                        if (sameLineSegments.Count > 1)
                        {
                            var orderedSegments = sameLineSegments.OrderBy(s => Math.Min(s.Start.X, s.End.X)).ToList();
                            var minimumSegment = orderedSegments.First();
                            var maximumSegment = orderedSegments.Last();
                            var minimumPoint = new Accord.Point(Math.Min(minimumSegment.Start.X, minimumSegment.End.X), minimumSegment.Start.Y);
                            var maximumPoint = new Accord.Point(Math.Max(maximumSegment.Start.X, maximumSegment.End.X), maximumSegment.Start.Y);
                            var combinedSegment = new LineSegment(minimumPoint, maximumPoint);

                            lineSegmentsCopy.Add(combinedSegment);

                            lineSegments.RemoveAll(s => sameLineSegments.Contains(s));
                        }
                        else
                        {
                            lineSegments.Remove(lineSegment);
                            lineSegmentsCopy.Add(lineSegment);
                        }

                        break;
                    }
                }

                // find matching lines from midpoint

                lineSegments.AddRange(lineSegmentsCopy);

                while (lineSegments.Count > 0)
                {
                    foreach (var lineSegment in lineSegments.ToList())
                    {
                        var line = lineSegment.To<Line>();
                        var distance = line.DistanceToPoint(midPoint);
                        var pairSegments = lineSegments.Where(l => l != lineSegment && Math.Abs(l.To<Line>().DistanceToPoint(midPoint) - distance) < 3).ToList();
                        var pairSegment = pairSegments.FirstOrDefault();

                        if (pairSegment != null)
                        {
                            var lineSegmentPair = new LineSegmentPair(lineSegment, pairSegment);

                            lineSegments.Remove(pairSegment);

                            lineSegmentPairs.Add(lineSegmentPair);
                        }

                        lineSegments.Remove(lineSegment);

                        break;
                    }
                }

                return lineSegmentPairs;
            }
        }

        public IList<PointPair> GetPointPairs(int width, int height)
        {
            IList<PointPair> result = new List<PointPair>();

            for (int i = 0; i < width; i++)
            {
                result.Add(new PointPair(new Point(i, 0), new Point(i, height)));
            }

            return result;
        }
    }

    public class InclinedScan : ScanStrategy
    {
        private BitmapAsTexture bmptex;
        private Graphics graphics;
        private Color edgecolor;
        private int angle;
        private object syncObj = new object();
        private const double DEGREE_TO_RADIAN = 0.017453292519943295769236907684883;

        public ColorSubtracter Strategy { get; set; }

        public InclinedScan(int a, BitmapAsTexture b, Graphics g, Color e, ColorSubtracter c)
        {
            lock (syncObj)
            {
                angle = a;
                bmptex = b;
                graphics = g;
                edgecolor = e;
                Strategy = c;
            }
        }

        public double Scan(int width, int height, int maxedges, int tolerance)
        {
            lock (syncObj)
            {
                //graphics.SmoothingMode = SmoothingMode.AntiAlias;
                graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                DateTime start = DateTime.Now;

                foreach (PointPair pointpair in GetPoints(width, height))
                {
                    Bitmap tex1d = null, onlyedges = null;

                    try
                    {
                        tex1d = bmptex.Get1DTexture(pointpair.Start, pointpair.End);
                        onlyedges = bmptex.ShowEdgePoints(tex1d, maxedges, Strategy, tolerance, edgecolor);

                        ObjectSpaceRotateTransform(pointpair, angle); // rotate modelmatrix (obj-trans-mat * world-trans-mat)
                        graphics.DrawImage(onlyedges, pointpair.Start);
                        ObjectSpaceRotateTransform(pointpair, -angle); // -rotate viewmatrix (reset world transform matrix)
                    }
                    finally
                    {
                        if (tex1d != null)
                        {
                            tex1d.Dispose();
                        }
                        if (onlyedges != null)
                        {
                            onlyedges.Dispose();
                        }
                    }
                }

                return (DateTime.Now - start).TotalMilliseconds;
            }
        }

        private void ObjectSpaceRotateTransform(PointPair axisPoint, int angle)
        {
            graphics.TranslateTransform(axisPoint.Start.X, axisPoint.Start.Y);
            graphics.RotateTransform(angle);
            graphics.TranslateTransform(-axisPoint.Start.X, -axisPoint.Start.Y);
        }

        private IList<PointPair> GetPoints(int width, int height)
        {
            IList<PointPair> result = new List<PointPair>();

            double angle1 = Math.Tan((90 - angle) * DEGREE_TO_RADIAN);
            double angle2 = Math.Tan(angle * DEGREE_TO_RADIAN);

            int start = (int)(angle1 * height);
            int end = width - start;

            for (int i = 0; i < end; i++)
            {
                result.Add(new PointPair(new Point(i, 0), new Point(i + start, height)));
            }

            for (int i = 1; (i < height); i++)
            {
                result.Add(new PointPair(new Point(0, i), new Point((int)(angle1 * (height - i)), height)));
            }

            for (int i = end; i < width; i++)
            {
                result.Add(new PointPair(new Point(i, 0), new Point(width, (int)(angle2 * (width - i)))));
            }

            return result;
        }
    }
}
