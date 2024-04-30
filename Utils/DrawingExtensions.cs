using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Windows.Forms;
#if !NOCOLORMINE
using ColorMine.ColorSpaces;
using ColorMine.ColorSpaces.Comparisons;
#endif
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Numerics;
using Accord.Math.Geometry;
#if !NOEDGEDETECTION
using EdgeDetection;
using BitmapedTextures;
#endif

namespace Utils
{
    public enum MetaProperty
    {
        Title = 40091,
        Comment = 40092,
        Author = 40093,
        Keywords = 40094,
        Subject = 40095,
        Copyright = 33432,
        Software = 11,
        DateTime = 36867
    }

    public enum ColorCompareOption
    {
        CIE2000
    }

    [Flags]
    public enum EdgeStyle : uint
    {
        RaisedOuter = 0x0001,
        SunkenOuter = 0x0002,
        RaisedInner = 0x0004,
        SunkenInner = 0x0008,
        Raised = 0x0005,
        Sunken = 0x000a,
        EdgeRaised = (RaisedOuter | RaisedInner),
        EdgeSunken = (SunkenOuter | SunkenInner),
        EdgeEtched = (SunkenOuter | RaisedInner),
        EdgeBump = (RaisedOuter | SunkenInner)
    }

    [Flags]
    public enum BorderFlags : uint
    {
        Left = 0x0001,
        Top = 0x0002,
        Right = 0x0004,
        Bottom = 0x0008,
        Adjust = 0x2000,
        Flat = 0x4000,
        Middle = 0x0800,
        BottomLeft = Bottom | Left,
        BottomRight = Bottom | Right,
        Diagonal = 0x10,
        DiagonalEndBottomLeft = Diagonal | Bottom | Left,
        DiagonalEndBottomRight = Diagonal | Bottom | Right,
        DiagonalEndTopLeft = Diagonal | Top | Left,
        DiagonalEndTopRight = Diagonal | Top | Right,
        Mono = 0x8000,
        Rect = Left | Top | Right | Bottom,
        Soft = 0x1000,
        TopLeft = Top | Left,
        TopRight = Top | Right    
    }

    [Flags]
    public enum RegionFlags
    {
        ERROR = 0,
        NULLREGION = 1,
        SIMPLEREGION = 2,
        COMPLEXREGION = 3,
    }

    public static partial class DrawingExtensions
    {
        [DllImport("gdi32.dll")]
        private static extern bool GetTextExtentPoint32(IntPtr hdc, string lpString, int cbString, ref Size lpSize);
        [DllImport("user32.dll")]
        private static extern bool DrawEdge(IntPtr hdc, ref RECT rc, EdgeStyle edge, BorderFlags grfFlags);
        [DllImport("GDI32.dll")]
        public static extern bool DeleteObject(IntPtr objectHandle);
        [DllImport("gdi32.dll", EntryPoint = "SelectObject")]
        public static extern IntPtr SelectObject([In] IntPtr hdc, [In] IntPtr hgdiobj);
        [DllImport("gdi32.dll")]
        private static extern RegionFlags SelectClipRgn(IntPtr hdc, IntPtr hrgn);
        [DllImport("gdi32.dll")]
        private static extern int GetClipRgn(IntPtr hdc, IntPtr hrgn);
        [DllImport("gdi32.dll")]
        private static extern RegionFlags ExcludeClipRect(IntPtr hdc, int nLeftRect, int nTopRect, int nRightRect, int nBottomRect);
        private const double DegToRad = Math.PI / 180;

        [DllImport("gdi32.dll", CharSet = CharSet.Auto)]
        public static extern int SetTextCharacterExtra(IntPtr hdc, int nCharExtra);

        public static Rectangle Round(this RectangleF rect)
        {
            return new Rectangle((int) Math.Round(rect.X), (int)Math.Round(rect.Y), (int)Math.Round(rect.Width), (int)Math.Round(rect.Height));
        }

        public static float GetContrastRatio(this Color color1, Color color2)
        {
            var luminence1 = ((HSLColor)color1).Luminosity;
            var luminence2 = ((HSLColor)color2).Luminosity;
            var brightest = Math.Max(luminence1, luminence2);
            var darkest = Math.Min(luminence1, luminence2);

            return (float)((brightest + 0.05) / (darkest + 0.05));
        }

        public static Image GetThumbnailImage(this ImageResult imageResult)
        {
            Image image;

            if (imageResult.ThumbnailImageBase64 == null)
            {
                image = Bitmap.FromFile(imageResult.ThumbnailFileName);

                using (var memoryStream = new MemoryStream())
                {
                    image.Save(memoryStream, image.RawFormat);

                    memoryStream.Rewind();

                    imageResult.ThumbnailImageBase64 = memoryStream.ToBase64();
                    imageResult.ImageFormat = image.RawFormat;
                }
            }
            else
            {
                using (var stream = imageResult.ThumbnailImageBase64.FromBase64().ToMemory())
                {
                    image = Bitmap.FromStream(stream);
                }
            }

            return image;
        }

        public static Image GetImage(this ImageResult imageResult)
        {
            Image image;

            if (imageResult.ImageBase64 == null)
            {
                image = Bitmap.FromFile(imageResult.FileName);

                using (var memoryStream = new MemoryStream())
                {
                    image.Save(memoryStream, image.RawFormat);

                    memoryStream.Rewind();

                    imageResult.ImageBase64 = memoryStream.ToBase64();
                    imageResult.ImageFormat = image.RawFormat;
                }
            }
            else
            {
                using (var stream = imageResult.ImageBase64.FromBase64().ToMemory())
                {
                    image = Bitmap.FromStream(stream);
                }
            }

            return image;
        }
#if !NOEDGEDETECTION
        public static Bitmap ReduceToBlackAndWhite(this Bitmap bitmap)
        {
            var fastBitmap = new FastBitmap(bitmap);

            fastBitmap.LockBitmap();

            for (var x = 0; x < bitmap.Width; x++)
            {
                for (var y = 0; y < bitmap.Height; y++)
                {
                    var color = fastBitmap.GetPixel(x, y).ToColor();
                    var brightness = color.GetBrightness();

                    if (brightness >= .1)
                    {
                        color = Color.White;
                    }

                    fastBitmap.SetPixel(x, y, new PixelData(color));
                }
            }

            fastBitmap.UnlockBitmap();

            return fastBitmap.Bitmap;
        }

        public static IList<LineSegmentPair> DetectHorizontalEdges(this Bitmap bitmap)
        {
            var bitmapAsTexture = new BitmapAsTexture(bitmap);
            var edgeColor = Color.Black;
            var subtractor = new BrightnessSubtractor();
            var width = bitmap.Width;
            var height = bitmap.Height;
            var hscan = new HScan(bitmapAsTexture, null, edgeColor, subtractor);

            return hscan.GetEdges(width, height, 100, 50);
        }

        public static IList<LineSegmentPair> DetectVerticalEdges(this Bitmap bitmap)
        {
            var bitmapAsTexture = new BitmapAsTexture(bitmap);
            var edgeColor = Color.Black;
            var subtractor = new BrightnessSubtractor();
            var width = bitmap.Width;
            var height = bitmap.Height;
            var vscan = new VScan(bitmapAsTexture, null, edgeColor, subtractor);

            return vscan.GetEdges(width, height, 100, 50);
        }
#endif
        public static Bitmap ToGrayscale(this Bitmap original)
        {
            Bitmap newBitmap = new Bitmap(original.Width, original.Height);
            Graphics g = Graphics.FromImage(newBitmap);
            ColorMatrix colorMatrix = new ColorMatrix(new float[][]
              {
                 new float[] {.3f, .3f, .3f, 0, 0},
                 new float[] {.59f, .59f, .59f, 0, 0},
                 new float[] {.11f, .11f, .11f, 0, 0},
                 new float[] {0, 0, 0, 1, 0},
                 new float[] {0, 0, 0, 0, 1}
              });

            ImageAttributes attributes = new ImageAttributes();
            attributes.SetColorMatrix(colorMatrix);

            g.DrawImage(original, new Rectangle(0, 0, original.Width, original.Height),
               0, 0, original.Width, original.Height, GraphicsUnit.Pixel, attributes);

            g.Dispose();

            return newBitmap;
        }

        public static void SetMetaValue(this Bitmap sourceBitmap, MetaProperty property, string value)
        {
            var prop = sourceBitmap.PropertyItems[0];
            var iLen = value.Length + 1;
            var bTxt = new Byte[iLen];

            for (int i = 0; i < iLen - 1; i++)
            {
                bTxt[i] = (byte)value[i];
            }

            bTxt[iLen - 1] = 0x00;
            
            prop.Id = (int)property;
            prop.Type = 2;
            prop.Value = bTxt;
            prop.Len = iLen;
            
            sourceBitmap.SetPropertyItem(prop);
        }

        public static string GetMetaValue(this Bitmap sourceBitmap, MetaProperty property)
        {
            PropertyItem[] propItems = sourceBitmap.PropertyItems;
            var prop = propItems.FirstOrDefault(p => p.Id == (int)property);
            if (prop != null)
            {
                return Encoding.UTF8.GetString(prop.Value);
            }
            else
            {
                return null;
            }
        }

        public static Bitmap ChangeColor(this Bitmap sourceBitmap, Color sourceColor, Color newColor, int threshold = 10)
        {
            var filter = new ColorChangeFilter { SourceColor = sourceColor, NewColor = newColor, ThresholdValue = threshold };

            return sourceBitmap.ChangeColor(filter);
        }

        public static Bitmap ProcessImage(this Bitmap sourceBitmap, Func<Color, Color> colorProcessor)
        {
            var bitmap = (Bitmap) sourceBitmap.Clone();
            var bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height),ImageLockMode.ReadWrite, bitmap.PixelFormat);
            var ptr = bitmapData.Scan0;
            var bytes = bitmapData.Stride * bitmapData.Height;
            var rgbValues = new byte[bytes];

            Marshal.Copy(ptr, rgbValues, 0, bytes);

            for (var c = 0; c < rgbValues.Length; c += 4)
            {
                var color = Color.FromArgb(rgbValues[c + 3], rgbValues[c + 2], rgbValues[c + 1], rgbValues[c]);

                color = colorProcessor(color);

                rgbValues[c] = color.B;
                rgbValues[c + 1] = color.G;
                rgbValues[c + 2] = color.R;
                rgbValues[c + 3] = color.A;
            }
            
            Marshal.Copy(rgbValues, 0, ptr, bytes);

            bitmap.UnlockBits(bitmapData);

            return bitmap;
        }

        public static IEnumerable<Color> GetColors(this Bitmap sourceBitmap, int sampleSkipSize = 1)
        {
            var bitmap = (Bitmap)sourceBitmap.Clone();
            var bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, bitmap.PixelFormat);
            var ptr = bitmapData.Scan0;
            var bytes = bitmapData.Stride * bitmapData.Height;
            var rgbValues = new byte[bytes];

            Marshal.Copy(ptr, rgbValues, 0, bytes);

            for (var c = 0; c < rgbValues.Length; c += (3 * sampleSkipSize))
            {
                var color = Color.FromArgb(rgbValues[c + 2], rgbValues[c + 1], rgbValues[c]);

                yield return color;
            }

            bitmap.UnlockBits(bitmapData);
        }

        public static IEnumerable<ColorMapEntry> GetColorMap(this Bitmap sourceBitmap, int sampleSkipSize = 1)
        {
            var bitmap = (Bitmap)sourceBitmap.Clone();
            var bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, bitmap.PixelFormat);
            var ptr = bitmapData.Scan0;
            var bytes = bitmapData.Stride * bitmapData.Height;
            var rgbValues = new byte[bytes];

            Marshal.Copy(ptr, rgbValues, 0, bytes);

            for (var c = 0; c < rgbValues.Length; c += (3 * sampleSkipSize))
            {
                var color = Color.FromArgb(rgbValues[c + 2], rgbValues[c + 1], rgbValues[c]);
                var row = (c / 3) / bitmapData.Stride;
                var col = (c / 3) % bitmapData.Stride;

                yield return new ColorMapEntry(row, col, color);
            }

            bitmap.UnlockBits(bitmapData);
        }

        public static int GetGreatestCommonDivisor(int a, int b)
        {
            int Remainder;

            while( b != 0 )
            {
                Remainder = a % b;
                a = b;
                b = Remainder;
            }

            return a;
        }

        public static float GetAspectRatio(this Image image)
        {
            var width = image.Width.As<float>();
            var height = image.Height.As<float>();

            return width / height;
        }

        public static string GetAspectRatioText(this Image image)
        {
            var width = image.Width;
            var height = image.Height;

            return string.Format("{0}:{1}", width / GetGreatestCommonDivisor(width, height), height / GetGreatestCommonDivisor(width, height));
        }

        public static Color GetDominantColor(this Bitmap sourceBitmap, int sampleSkipSize = 1)
        {
            var bitmap = (Bitmap) sourceBitmap.Clone();
            var colors = bitmap.GetColors(sampleSkipSize).ToList();
            var distinctColors = colors.Distinct().ToList();
            var color = distinctColors.OrderBy(c => colors.Count(c2 => c == c2)).Last();

            return color;
        }

        public static int GetColorDiff(this Color color)
        {
            var diffRG = Math.Abs(color.R - color.G);
            var diffRB = Math.Abs(color.R - color.B);
            var diffGB = Math.Abs(color.G - color.B);

            return diffRG + diffRB + diffGB;
        }

        public static Color[] FilterGrays(this Color[] colors, int threshold = 20)
        {
            return colors.Where(c => c.GetColorDiff() > threshold).ToArray(); 
        }

        public static void DrawWarningSquiggly(this Graphics graphics, string text, Font font, RectangleF rect)
        {
            var pixelsPerSample = 1.2f;
            var pointCount = (int)(rect.Width / pixelsPerSample);
            var metrics = graphics.GetTextMetrics(font);
            var amplitude = 1.5f;
            var fYOffset = rect.Y + metrics.tmAscent + metrics.tmDescent * 0.5f + 1.0f;
            var xOffset = 0.0f;
            var points = new List<Point>();
            var pen = new Pen(Color.Green);

            if (pointCount == 0)
            {
                return;
            }

            for (var x = 0; x < pointCount; x++)
            {
                var pt = new Point();
                float value;

                pt.X = (int)(rect.X + xOffset++ * rect.Width / (pointCount - 1));
                value = (float)(amplitude * Math.Sin((pt.X / pixelsPerSample) * (Math.PI / 3.0)));
                pt.Y = (int)(fYOffset + value);

                points.Add(pt);
            }

            graphics.DrawLines(pen, points.ToArray());
        }

        public static void DrawErrorSquiggly(this Graphics graphics, string text, Font font, RectangleF rect)
        {
            var pixelsPerSample = 1.2f;
            var pointCount = (int)(rect.Width / pixelsPerSample);
            var metrics = graphics.GetTextMetrics(font);
            var amplitude = 1.5f;
            var fYOffset = rect.Y + metrics.tmAscent + metrics.tmDescent * 0.5f + 1.0f;
            var xOffset = 0.0f;
            var points = new List<Point>();
            var pen = new Pen(Color.Red);

            if (pointCount == 0)
            {
                return;
            }

            for (var x = 0; x < pointCount; x++)
            {
                var pt = new Point();
                float value;

                pt.X = (int) (rect.X + xOffset++ * rect.Width / (pointCount - 1));
                value = (float)(amplitude * Math.Sin((pt.X / pixelsPerSample) * (Math.PI / 3.0)));
                pt.Y = (int) (fYOffset + value);

                points.Add(pt);
            }

            graphics.DrawLines(pen, points.ToArray());
        }

        public static Bitmap ChangeColor(this Bitmap sourceBitmap, ColorChangeFilter filterData)
        {
            Bitmap resultBitmap = new Bitmap(sourceBitmap.Width, sourceBitmap.Height, PixelFormat.Format32bppArgb);
            BitmapData sourceData = sourceBitmap.LockBits(new Rectangle(0, 0, sourceBitmap.Width, sourceBitmap.Height),
                                                          ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            BitmapData resultData = resultBitmap.LockBits(new Rectangle(0, 0, resultBitmap.Width, resultBitmap.Height),
                                                          ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);

            byte[] resultBuffer = new byte[resultData.Stride * resultData.Height];
            Marshal.Copy(sourceData.Scan0, resultBuffer, 0, resultBuffer.Length);

            sourceBitmap.UnlockBits(sourceData);

            byte sourceRed = 0, sourceGreen = 0, sourceBlue = 0, sourceAlpha = 0;
            int resultRed = 0, resultGreen = 0, resultBlue = 0;
            byte newRedValue = filterData.NewColor.R;
            byte newGreenValue = filterData.NewColor.G;
            byte newBlueValue = filterData.NewColor.B;
            byte redFilter = filterData.SourceColor.R;
            byte greenFilter = filterData.SourceColor.G;
            byte blueFilter = filterData.SourceColor.B;
            byte minValue = 0;
            byte maxValue = 255;

            for (int k = 0; k < resultBuffer.Length; k += 4)
            {
                sourceAlpha = resultBuffer[k + 3];

                if (sourceAlpha != 0)
                {
                    sourceBlue = resultBuffer[k];
                    sourceGreen = resultBuffer[k + 1];
                    sourceRed = resultBuffer[k + 2];

                    if ((sourceBlue < blueFilter + filterData.ThresholdValue &&
                            sourceBlue > blueFilter - filterData.ThresholdValue) &&
                        (sourceGreen < greenFilter + filterData.ThresholdValue &&
                            sourceGreen > greenFilter - filterData.ThresholdValue) &&
                        (sourceRed < redFilter + filterData.ThresholdValue &&
                            sourceRed > redFilter - filterData.ThresholdValue))
                    {
                        resultBlue = blueFilter - sourceBlue + newBlueValue;

                        if (resultBlue > maxValue)
                        { resultBlue = maxValue; }
                        else if (resultBlue < minValue)
                        { resultBlue = minValue; }

                        resultGreen = greenFilter - sourceGreen + newGreenValue;

                        if (resultGreen > maxValue)
                        { resultGreen = maxValue; }
                        else if (resultGreen < minValue)
                        { resultGreen = minValue; }

                        resultRed = redFilter - sourceRed + newRedValue;

                        if (resultRed > maxValue)
                        { resultRed = maxValue; }
                        else if (resultRed < minValue)
                        { resultRed = minValue; }

                        resultBuffer[k] = (byte)resultBlue;
                        resultBuffer[k + 1] = (byte)resultGreen;
                        resultBuffer[k + 2] = (byte)resultRed;
                        resultBuffer[k + 3] = sourceAlpha;
                    }
                }
            }

            Marshal.Copy(resultBuffer, 0, resultData.Scan0, resultBuffer.Length);
            resultBitmap.UnlockBits(resultData);

            return resultBitmap;
        }

        public static void DrawRoundedRectangle(this Graphics graphics, Pen pen, Rectangle bounds, int cornerRadius)
        {
            if (graphics == null)
                throw new ArgumentNullException("graphics");
            if (pen == null)
                throw new ArgumentNullException("pen");

            using (GraphicsPath path = bounds.ToRoundedRect(cornerRadius))
            {
                graphics.DrawPath(pen, path);
            }
        }

        public static void DrawBorder(this Graphics graphics, Pen pen, Control control)
        {
            var rect = control.ClientRectangle;

            if (graphics == null)
            {
                throw new ArgumentNullException("graphics");
            }

            if (pen == null)
            {
                throw new ArgumentNullException("pen");
            }

            rect.Inflate(-1, -1);

            graphics.DrawRectangle(pen, rect);
        }

        public static void FillRoundedRectangle(this Graphics graphics, Brush brush, Rectangle bounds, int cornerRadius)
        {
            if (graphics == null)
                throw new ArgumentNullException("graphics");
            if (brush == null)
                throw new ArgumentNullException("brush");

            using (GraphicsPath path = bounds.ToRoundedRect(cornerRadius))
            {
                graphics.FillPath(brush, path);
            }
        }

        public static GraphicsPath ToRoundedRect(this Rectangle bounds, int radius)
        {
            int diameter = radius * 2;
            Size size = new Size(diameter, diameter);
            Rectangle arc = new Rectangle(bounds.Location, size);
            GraphicsPath path = new GraphicsPath();

            if (radius == 0)
            {
                path.AddRectangle(bounds);
                return path;
            }

            // top left arc  
            path.AddArc(arc, 180, 90);

            // top right arc  
            arc.X = bounds.Right - diameter;
            path.AddArc(arc, 270, 90);

            // bottom right arc  
            arc.Y = bounds.Bottom - diameter;
            path.AddArc(arc, 0, 90);

            // bottom left arc 
            arc.X = bounds.Left;
            path.AddArc(arc, 90, 90);

            path.CloseFigure();
            return path;
        }

        public static IDisposable SetTextSpacing(this Graphics graphics, int pixels)
        {
            var hdc = graphics.GetHdc();
            int previous;

            previous = SetTextCharacterExtra(hdc, pixels);
            graphics.ReleaseHdc(hdc);

            return graphics.CreateDisposable(() =>
            {
                hdc = graphics.GetHdc();

                SetTextCharacterExtra(hdc, previous);
                graphics.ReleaseHdc(hdc);
            });
        }

        public static Bitmap ResizeImage(this Image image, int width, int height = -1)
        {
            Rectangle destRect;
            Bitmap destImage;

            if (height == -1)
            {
                var imageWidth = image.Width;
                var imageHeight = image.Height;
                var aspect = imageHeight.As<float>() / imageWidth.As<float>();

                height = (int) (width.As<float>() * aspect);
            }

            destRect = new Rectangle(0, 0, width, height);
            destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.Default;
                graphics.InterpolationMode = InterpolationMode.Default;
                graphics.SmoothingMode = SmoothingMode.Default;
                graphics.PixelOffsetMode = PixelOffsetMode.Default;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

        public static Rectangle GetCenteredRect(this Rectangle outerRect, Size innerRectSize)
        {
            var rect = new Rectangle(Point.Empty, innerRectSize);
            var x = outerRect.X + (outerRect.Width / 2 - innerRectSize.Width / 2);
            var y = outerRect.Y + (outerRect.Height / 2 - innerRectSize.Height / 2);
            
            rect.Location = new Point(x, y);

            return rect;
        }

        public static Point GetCenterPoint(this Rectangle outerRect)
        {
            var x = outerRect.X + (outerRect.Width / 2);
            var y = outerRect.Y + (outerRect.Height / 2);
            var point = new Point(x, y);

            return point;
        }

        public static bool AdjacentWith(this Rectangle a, Rectangle b)
        {
            if (a.X == b.X)
            {
                return true;
            }
            else if (a.Y == b.Y)
            {
                return true;
            }
            else if (a.X + a.Width == b.X)
            {
                return true;
            }
            else if (a.Y + a.Height == b.Y)
            {
                return true;
            }
            else if (b.X + b.Width == a.X)
            {
                return true;
            }
            else if (b.Y + b.Height == a.Y)
            {
                return true;
            }

            return false;
        }

        public static Point Rotate(this Point point, double degrees)
        {
            var vector = new System.Windows.Vector(point.X, point.Y);

            vector = vector.RotateRadians(degrees * DegToRad);

            return new Point((int)vector.X, (int)vector.Y);
        }

        public static System.Windows.Vector Rotate(this System.Windows.Vector vector, double degrees)
        {
            return vector.RotateRadians(degrees * DegToRad);
        }

        public static System.Windows.Vector RotateRadians(this System.Windows.Vector vector, double radians)
        {
            var cos = Math.Cos(radians);
            var sin = Math.Sin(radians);

            return new System.Windows.Vector(cos * vector.X - sin * vector.Y, sin * vector.X + cos * vector.Y);
        }

        public static Color HexStringToColor(string text, bool leadingIdentifier = false)
        {
            var n = (int) text.FromHexString(leadingIdentifier);

            return Color.FromArgb(n);
       }

        public static string ToHexString(this Color color, bool leadingIdentifier = false)
        {
            var r = color.R.ToHexString();
            var g = color.G.ToHexString();
            var b = color.B.ToHexString();

            return r + g + b; 
        }

        public static Point GetCenteredRectPosition(this Rectangle outerRect, Size innerRectSize)
        {
            var x = outerRect.Width / 2 - innerRectSize.Width / 2;
            var y = outerRect.Height / 2 - innerRectSize.Height / 2;

            return new Point(x, y);
        }

        public static Rectangle OffsetNew(this Rectangle rect, int x, int y)
        {
            rect.Offset(x, y);

            return rect;
        }

        public static void Offset(this List<Point> points, int dx, int dy)
        {
            for (var x = 0; x < points.Count; x++)
            {
                var point = points[x];

                point.Offset(dx, dy);

                points[x] = point;
            }
        }

        public static void LineToOffset(this List<Point> points, int dx, int dy)
        {
            var point = points.Last();

            point.Offset(dx, dy);

            points.Add(point);
        }

        public static void LineToXOffset(this List<Point> points, int dx)
        {
            var point = points.Last();

            point.Offset(dx, 0);

            points.Add(point);
        }

        public static void LineToYOffset(this List<Point> points, int dy)
        {
            var point = points.Last();

            point.Offset(0, dy);

            points.Add(point);
        }

        public static void LineTo(this List<Point> points, int x, int y)
        {
            var point = new Point(x, y);

            points.LineTo(point);
        }

        public static void LineTo(this List<Point> points, Point point)
        {
            points.Add(point);
        }

        public static void LineToX(this List<Point> points, int x)
        {
            var point = points.Last();
            
            point = new Point(x, point.Y);

            points.Add(point);
        }

        public static void LineToY(this List<Point> points, int y)
        {
            var point = points.Last();

            point = new Point(point.X, y);

            points.Add(point);
        }

        public static Size GetTextExtentPoint(this Graphics graphics, Font font, string text)
        {
            using (var hdc = graphics.GetDisposableHdc())
            {
                var hFont = font.ToHfont();

                using (var oldHFont = hdc.SelectObject(hFont))
                {
                    var size = Size.Empty;
                    var result = GetTextExtentPoint32(hdc, text, text.Length, ref size);

                    return size;
                }
            }
        }

        public static bool GetTextExtentPoint(this Graphics graphics, Font font, string text, ref Size size)
        {
            using (var hdc = graphics.GetDisposableHdc())
            {
                var hFont = font.ToHfont();

                using (var oldHFont = hdc.SelectObject(hFont))
                {
                    var result = GetTextExtentPoint32(hdc, text, text.Length, ref size);

                    return result;
                }
            }
        }

        public static bool GetTextExtentPoint(this Control control, string text, ref Size size)
        {
            using (var graphics = control.CreateGraphics())
            {
                var font = control.Font;

                using (var hdc = graphics.GetDisposableHdc())
                {
                    var hFont = font.ToHfont();

                    using (var oldHFont = hdc.SelectObject(hFont))
                    {
                        var result = GetTextExtentPoint32(hdc, text, text.Length, ref size);

                        return result;
                    }
                }
            }
        }

        public static DisposableHandle SelectObject(this Graphics graphics, Font font)
        {
            var hFont = font.ToHfont();

            return graphics.SelectObject(hFont);
        }

        public static DisposableHandle SelectObject(this DisposableHandle graphicsHandle, IntPtr gdiObject)
        {
            var hdc = graphicsHandle;
            var oldObject = SelectObject((IntPtr) hdc, gdiObject);
            var disposableHandle = new DisposableHandle(oldObject);

            disposableHandle.Disposed += (sender, e) =>
            {
                SelectObject(hdc, oldObject);
                DeleteObject(gdiObject);
            };

            return disposableHandle;
        }

        public static DisposableHandle SelectObject(this DisposableHandle graphicsHandle, Font font)
        {
            var hFont = font.ToHfont();

            return graphicsHandle.SelectObject(hFont);
        }

        public static DisposableHandle SelectObject(this Graphics graphics, IntPtr gdiObject)
        {
            var hdc = graphics.GetDisposableHdc();
            var oldObject = SelectObject(hdc, gdiObject);
            var disposableHandle = new DisposableHandle(oldObject);

            disposableHandle.Disposed += (sender, e) =>
            {
                SelectObject(hdc, oldObject);
                DeleteObject(gdiObject);

                hdc.Dispose();
            };

            return disposableHandle;
        }

        public static bool DrawEdge(this Graphics graphics, Rectangle rc, EdgeStyle edge, BorderFlags flags, Rectangle? _exclude = null)
        {
            var rect = new RECT(rc);

            using (var hdc = graphics.GetDisposableHdc())
            {
                bool returnValue;

                if (_exclude != null)
                {
                    var exclude = (Rectangle)_exclude;
                    var regionFlags = ExcludeClipRect(hdc, exclude.Left, exclude.Top, exclude.Right, exclude.Bottom);
                }

                returnValue = DrawEdge(hdc, ref rect, edge, flags);

                return returnValue;
            }
        }

        public static Size Swap(this Size size)
        {
            return new Size(size.Height, size.Width);
        }

        public static Color MidPoint(this Color color1, Color color2, float percentTo)
        {
            var r1 = color1.R;
            var g1 = color1.G;
            var b1 = color1.B;
            var r2 = color2.R;
            var g2 = color2.G;
            var b2 = color2.B;
            var percent1 = 1f - percentTo;
            var percent2 = percentTo;

            return Color.FromArgb((int)((r1 * percent1) + (r2 * percent2)), (int)((g1 * percent1) + (g2 * percent2)), (int)((b1 * percent1) + (b2 * percent2)));
        }

        public static Color MidPoint(this Color color1, Color color2)
        {
            return color1.MidPoint(color2, .5f);
        }

        public static Point FindIntersection(this Line lineA, Line lineB)
        {
            double determinant = lineA.A * lineB.B - lineB.A * lineA.B;

            if (determinant.IsZero()) //lines are parallel
                return default(Point);

            //Cramer's Rule

            double x = (lineB.B * lineA.C - lineA.B * lineB.C) / determinant;
            double y = (lineA.A * lineB.C - lineB.A * lineA.C) / determinant;

            Point intersectionPoint = new Point((int) x, (int) y);

            return intersectionPoint;
        }

        private static bool IsInsideLine(Line line, double x, double y)
        {
            return ((x >= line.x1 && x <= line.x2)
                        || (x >= line.x2 && x <= line.x1))
                   && ((y >= line.y1 && y <= line.y2)
                        || (y >= line.y2 && y <= line.y1));
        }

        public const double NegativeMachineEpsilon = 1.1102230246251565e-16D;
        public const double PositiveMachineEpsilon = 2D * NegativeMachineEpsilon;

        public static bool IsZero(this double value)
        {
            //return Math.Abs(value) <= PositiveMachineEpsilon;

            if (value > PositiveMachineEpsilon ||
                value < -PositiveMachineEpsilon)
                return false;

            return true;
        }

        public static Color Lighten(this Color color, double percent)
        {
            var hslColor = (HSLColor) color;
            Color newColor;

            if (percent < 0)
            {
                var margin = (((float)hslColor.Luminosity) * -percent);

                hslColor.Luminosity -= margin;
            }
            else
            {
                var margin = ((double) HSLColor.SCALE - hslColor.Luminosity) * percent;

                hslColor.Luminosity += margin;
            }

            newColor = hslColor;

            return Color.FromArgb(color.A, newColor);
        }

        public static Color Saturate(this Color color, double percent)
        {
            var hslColor = (HSLColor)color;
            Color newColor;

            if (percent < 0)
            {
                var margin = (((float)hslColor.Saturation) * -percent);

                hslColor.Saturation -= margin;
            }
            else
            {
                var margin = ((double)HSLColor.SCALE - hslColor.Saturation) * percent;

                hslColor.Saturation += margin;
            }

            newColor = hslColor;

            return Color.FromArgb(color.A, newColor);
        }

#if !NOCOLORMINE

        public static Color[] GetDominantColors(this Bitmap sourceBitmap, int sampleSkipSize = 50, int sampleSize = 100, int compareSpacingAmount = 20)
        {
            var bitmap = (Bitmap)sourceBitmap.Clone();
            var colors = bitmap.GetColors(sampleSkipSize).ToList();
            var distinctColors = colors.Distinct().ToList();
            var dominantColors = distinctColors.OrderByDescending(c => colors.Count(c2 => c == c2)).Take(sampleSize);
            var returnColors = new List<Color>();
            Color? lastColor = null;

            foreach (var color in dominantColors)
            {
                if (lastColor.HasValue)
                {
                    var compare = color.Compare(lastColor.Value);

                    if (compare > compareSpacingAmount)
                    {
                        returnColors.Add(color);
                        lastColor = color;
                    }
                }
                else
                {
                    returnColors.Add(color);
                    lastColor = color;
                }
            }

            return returnColors.ToArray();
        }

        public static double Compare(this Color color, Color colorCompare)
        {
            var rgbColor = new Rgb(color.R, color.G, color.B);
            var rgbCompare = new Rgb(colorCompare.R, colorCompare.G, colorCompare.B);

            return rgbColor.Compare(rgbCompare, new CieDe2000Comparison());
        }

        public static double Compare(this Color color, Color colorCompare, ColorCompareOption option)
        {
            var rgbColor = new Rgb(color.R, color.G, color.B);
            var rgbCompare = new Rgb(colorCompare.R, colorCompare.G, colorCompare.B);

            switch (option)
            {
                case ColorCompareOption.CIE2000:
                    return rgbColor.Compare(rgbCompare, new CieDe2000Comparison());
                default:
                    DebugUtils.Break();
                    return -1;
            }
        }
#endif
        public static Color SetLight(this Color color, double luminosity)
        {
            var hslColor = (HSLColor)color;

            e.AssertInRange(luminosity, 0, HSLColor.SCALE);

            hslColor.Luminosity = luminosity;

            return hslColor;
        }
        
        public static Color SetA(this Color color, byte a)
        {
            return Color.FromArgb(a, color.R, color.G, color.B);
        }

        public static Color SetR(this Color color, byte r)
        {
            return Color.FromArgb(color.A, r, color.G, color.B);
        }

        public static Color SetG(this Color color, byte g)
        {
            return Color.FromArgb(color.A, color.R, g, color.B);
        }

        public static Color SetB(this Color color, byte b)
        {
            return Color.FromArgb(color.A, color.R, color.G, b);
        }

        public static void DrawString(this Graphics graphics, string text, Font font, Brush brush, Rectangle layoutRectangle, StringAlignment alignment)
        {
            graphics.DrawString(text, font, brush, layoutRectangle, new StringFormat { LineAlignment = alignment, Alignment = alignment});
        }

        public static void DrawString(this Graphics graphics, string text, Font font, Brush brush, Rectangle layoutRectangle, StringAlignment alignment, StringFormatFlags stringFormatFlags)
        {
            graphics.DrawString(text, font, brush, layoutRectangle, new StringFormat(stringFormatFlags) { LineAlignment = alignment, Alignment = alignment });
        }

        public static void DrawString(this Graphics graphics, string text, Font font, Brush brush, Rectangle layoutRectangle, StringAlignment alignment, StringFormatFlags stringFormatFlags, StringTrimming trimming)
        {
            graphics.DrawString(text, font, brush, layoutRectangle, new StringFormat(stringFormatFlags) { Trimming = trimming, LineAlignment = alignment, Alignment = alignment });
        }

        public static void DrawString(this Graphics graphics, string text, Font font, Brush brush, Rectangle layoutRectangle, StringAlignment alignment, StringTrimming trimming)
        {
            graphics.DrawString(text, font, brush, layoutRectangle, new StringFormat { Trimming = trimming, LineAlignment = alignment, Alignment = alignment });
        }

        public static DisposableHandle GetDisposableHdc(this Graphics graphics)
        {
            var handle = new DisposableHandle(graphics.GetHdc());

            handle.Disposed += (sender, e) => graphics.ReleaseHdc();

            return handle;
        }
    }
}
