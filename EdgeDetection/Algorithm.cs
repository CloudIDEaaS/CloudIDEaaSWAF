using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace BitmapedTextures
{
    class Algorithm
    {
        public IList<Point> GenerateLinePoints(Point start, Point end)
        {
            IList<Point> points = new List<Point>();

            float dx = end.X - start.X;
            float dy = end.Y - start.Y;

            if (dy < 0.0000000000000001f)
            {
                for (int i = start.X; i < end.X; i++)
                {
                    points.Add(new Point(i, start.Y));
                }
            }
            else if (dx < 0.0000000000000001f)
            {
                for (int j = start.Y; j < end.Y; j++)
                {
                    points.Add(new Point(start.X, j));
                }
            }
            else
            {
                bool steep = Math.Abs(dy) > Math.Abs(dx);

                if(steep)
                {
                    int tmp = start.X;
                    start.X = start.Y;
                    start.Y = tmp;

                    tmp = end.X;
                    end.X = end.Y;
                    end.Y = tmp;
                }
                if(start.X > end.X)
                {
                    Point tmp = start;
                    start = end;
                    end = tmp;
                }
                
                float deltax = end.X - start.X;
                float deltay = Math.Abs(end.Y - start.Y);

                float error = 0;
                float deltaerr = deltay / deltax;
                
                int ystep = -1;     
                int y = start.Y;

                if (start.Y < end.Y) 
                   ystep = 1;

                for (int x = start.X; x < end.X; x++)
                {
                    if (steep) points.Add(new Point(y, x));
                    else points.Add(new Point(x, y));

                   error = error + deltaerr;
                   if (error >= 0.5)
                   {
                       y = y + ystep;
                       error = error - 1.0f;
                   }
                }
            }
            #region Simple but incorrect
            //else if (dx > dy)
            //{
            //    float m = dy / dx;
            //    int sign = dx > 0 ? 1 : -1;

            //    for (int i = start.X; i != end.X; i += sign)
            //    {
            //        int j = (int)(m * i + start.Y);
            //        points.Add(new Point(i, j));
            //    }
            //}
            //else
            //{
            //    float m = dy / dx;
            //    int sign = dy > 0 ? 1 : -1;

            //    for (int j = start.Y; j != end.Y; j += sign)
            //    {
            //        int i = (int)((j - start.Y) / m);
            //        points.Add(new Point(i, j));
            //    }
            //}
            #endregion

            return points;
        }

        public int GetDistance(Point start, Point end)
        {
            IList<Point> points = new List<Point>();

            float dx = end.X - start.X;
            float dy = end.Y - start.Y;

            return (int)Math.Sqrt(dx * dx + dy * dy);
        }
    }
}
