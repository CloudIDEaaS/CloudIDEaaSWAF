﻿using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Drawing;
#if !NOCOLORMINE
#endif
#if !NOEDGEDETECTION
#endif

namespace Utils
{
    public static partial class DrawingExtensions
    {
        public static int MakeCOLORREF(byte r, byte g, byte b)
        {
            return (int)(((uint)r) | (((uint)g) << 8) | (((uint)b) << 16));
        }

        public static Color FromCOLORREF(uint colorref)
        {
            int r = (int)((colorref >> 16) & 0xFF);
            int g = (int)((colorref >> 8) & 0x00FF);
            int b = (int)(colorref & 0x0000FF);

            return Color.FromArgb(r, g, b);
        }

        public static int ToCOLORREF(this Color color)
        {
            var r = color.R;
            var g = color.G;
            var b = color.B;

            return (int)(((uint)r) | (((uint)g) << 8) | (((uint)b) << 16));
        }
    }

    [DebuggerDisplay(" { DebugInfo } "), StructLayout(LayoutKind.Sequential)]
    public class RECT
    {
        public int Left, Top, Right, Bottom;

        public RECT(int left, int top, int right, int bottom)
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }

        public RECT(Rectangle r) : this(r.Left, r.Top, r.Right, r.Bottom) { }

        public RECT() { }

        public int X
        {
            get { return Left; }
            set { Right -= (Left - value); Left = value; }
        }

        public int Y
        {
            get { return Top; }
            set { Bottom -= (Top - value); Top = value; }
        }

        public int Height
        {
            get { return Bottom - Top; }
            set { Bottom = value + Top; }
        }

        public int Width
        {
            get { return Right - Left; }
            set { Right = value + Left; }
        }

        public Point Location
        {
            get { return new Point(Left, Top); }
            set { X = value.X; Y = value.Y; }
        }

        public Size Size
        {
            get { return new Size(Width, Height); }
            set { Width = value.Width; Height = value.Height; }
        }

        public static implicit operator Rectangle(RECT r)
        {
            return new Rectangle(r.Left, r.Top, r.Width, r.Height);
        }

        public static implicit operator RECT(Rectangle r)
        {
            return new RECT(r);
        }

        public static bool operator ==(RECT r1, RECT r2)
        {
            return r1.Equals(r2);
        }

        public static bool operator !=(RECT r1, RECT r2)
        {
            return !r1.Equals(r2);
        }

        public bool Equals(RECT r)
        {
            return r.Left == Left && r.Top == Top && r.Right == Right && r.Bottom == Bottom;
        }

        public override bool Equals(object obj)
        {
            if (obj is RECT)
                return Equals((RECT)obj);
            else if (obj is Rectangle)
                return Equals(new RECT((Rectangle)obj));
            return false;
        }

        public string DebugInfo
        {
            get
            {
                return this.ToString();
            }
        }

        public override int GetHashCode()
        {
            return ((Rectangle)this).GetHashCode();
        }

        public override string ToString()
        {
            return string.Format(System.Globalization.CultureInfo.CurrentCulture, "{{Left={0},Top={1},Right={2},Bottom={3}}}", Left, Top, Right, Bottom);
        }

        public bool Contains(RECT rect)
        {
            var thisRect = (Rectangle)this;
            var otherRect = (Rectangle)rect;

            return thisRect.Contains(otherRect);
        }
    }
}
