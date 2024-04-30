using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Diagnostics;
using System.Globalization;

namespace Utils.MemoryView
{
    [DebuggerDisplay(" { DebugInfo } ")]
    public class DrawRect
    {
        private Rectangle rect;
        public int StartRow { get; set; }
        public int StartColumn { get; set; }
        public int EndRow { get; set; }
        public int EndColumn { get; set; }

        public DrawRect()
        {
            this.rect = Rectangle.Empty;
        }

        public DrawRect(Rectangle rect)
        {
            this.rect = rect;
        }

        public Rectangle Rectangle
        {
            get 
            {
                return rect; 
            }
            
            set 
            { 
                rect = value; 
            }
        }

        public int Bottom 
        {
            get
            {
                return rect.Bottom;
            }
        }
        
        public bool IsEmpty 
        {
            get
            {
                return rect.IsEmpty;
            } 
        }
        
        public int Left 
        {
            get
            {
                return rect.Left;
            } 
        }
        
        public Point Location 
        {
            get
            {
                return rect.Location;
            }

            set
            {
                rect.Location = value;
            }
        }

        public int Right 
        {
            get
            {
                return rect.Right;
            } 
        }

        public Size Size 
        {
            get
            {
                return rect.Size;
            }

            set
            {
                rect.Size = value;
            } 
        }
        
        public int Top 
        {
            get
            {
                return rect.Top;
            } 
        }
        
        public int Width 
        {
            get
            {
                return rect.Width;
            }

            set
            {
                rect.Width = value;
            } 
        }

        public int Height
        {
            get
            {
                return rect.Height;
            }

            set
            {
                rect.Height = value;
            }
        }
        
        public int X 
        {
            get
            {
                return rect.X;
            }

            set
            {
                rect.X = value;
            } 
        }
        
        public int Y 
        {
            get
            {
                return rect.Y;
            }

            set
            {
                rect.Y = value;
            } 
        }

        public string DebugInfo
        {
            get
            {
                string[] textArray1 = new string[] { "{X=", this.X.ToString(CultureInfo.CurrentCulture), ",Y=", this.Y.ToString(CultureInfo.CurrentCulture), ",Width=", this.Width.ToString(CultureInfo.CurrentCulture), ",Height=", this.Height.ToString(CultureInfo.CurrentCulture), "}" };
                return string.Concat(textArray1);
            }
        }

        public override string ToString()
        {
            return this.DebugInfo;
        }

        public static implicit operator DrawRect(Rectangle rect)
        {
            return new DrawRect(rect);
        }

        public static implicit operator Rectangle(DrawRect drawRect)
        {
            return drawRect.Rectangle;
        }
    }
}
