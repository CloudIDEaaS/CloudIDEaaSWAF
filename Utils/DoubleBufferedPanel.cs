using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Utils
{
    public partial class DoubleBufferedPanel : Control
    {
        public new event PaintEventHandler Paint;
        private Graphics currentGraphics;
        private Bitmap currentGraphicsImage;
        private bool selectable;

        public DoubleBufferedPanel()
        {
            this.CreateControl();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (selectable && this.Focused)
            {
                var rect = this.ClientRectangle;

                rect.Inflate(-2, -2);

                ControlPaint.DrawFocusRectangle(e.Graphics, rect);
            }
        }

        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
        }

        public void DebugDraw()
        {
            currentGraphics.DrawImage(currentGraphicsImage, 0, 0);
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == (int) ControlExtensions.WindowsMessage.PAINT)
            {
                using (var graphics = this.CreateGraphics())
                {
                    if (Paint == null)
                    {
                        graphics.FillRectangle(SystemBrushes.Control, this.ClientRectangle);
                    }
                    else
                    {
                        var image = new Bitmap(this.Width, this.Height);
                        var clip = Rectangle.Round(graphics.ClipBounds);

                        currentGraphics = graphics;
                        currentGraphicsImage = image;

                        Paint(this, new PaintEventArgs(Graphics.FromImage(image), clip));
                        graphics.DrawImage(image, 0, 0);
                    }
                }
            }

            base.WndProc(ref m);
        }

        public bool Selectable
        {
            set
            {
                selectable = true;
                this.SetStyle(ControlStyles.Selectable, value);
            }

            get
            {
                return selectable;
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (selectable)
            {
                this.Focus();
            }
              
            base.OnMouseDown(e);
        }

        protected override bool IsInputKey(Keys keyData)
        {
            if (selectable)
            {
                if (keyData == Keys.Up || keyData == Keys.Down)
                {
                    return true;
                }

                if (keyData == Keys.Left || keyData == Keys.Right)
                {
                    return true;
                }
            }

            return base.IsInputKey(keyData);
        }

        protected override void OnEnter(EventArgs e)
        {
            this.Invalidate();
            base.OnEnter(e);
        }

        protected override void OnLeave(EventArgs e)
        {
            this.Invalidate();
            base.OnLeave(e);
        }
    }
}
