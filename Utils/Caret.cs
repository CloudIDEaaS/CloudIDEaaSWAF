using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Utils
{
    public class Caret : IDisposable
    {
        private Control control;

        public int Width { get; }
        public int Height { get; }

        private Point position;

        public Caret(Control control, int width, int height)
        {
            this.control = control;
            this.Width = width;
            this.Height = height;

            ControlExtensions.CreateCaret(control.Handle, IntPtr.Zero, width, height);
        }

        public Rectangle Rect
        {
            get
            {
                return new Rectangle(position, new Size(this.Width, this.Height));
            }
        }

        public System.Drawing.Point Position
        {
            set
            {
                position = value;

                ControlExtensions.SetCaretPos((int)position.X, (int)position.Y);
            }

            get
            {
                return position;
            }
        }

        public void Show()
        {
            ControlExtensions.ShowCaret(control.Handle);
        }

        public void Dispose()
        {
            ControlExtensions.DestroyCaret();
        }

        internal void Hide()
        {
            ControlExtensions.HideCaret(control.Handle);
        }
    }
}
