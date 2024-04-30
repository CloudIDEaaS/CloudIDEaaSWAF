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
    public partial class DoubleBufferedTreeView : TreeView
    {
        private Graphics currentGraphics;
        private Bitmap currentGraphicsImage;

        public DoubleBufferedTreeView()
        {
            this.CreateControl();
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
                    var image = new Bitmap(this.Width, this.Height);
                    var clip = Rectangle.Round(graphics.ClipBounds);

                    currentGraphics = graphics;
                    currentGraphicsImage = image;

                    OnPaint(new PaintEventArgs(Graphics.FromImage(image), clip));
                    graphics.DrawImage(image, 0, 0);
                }

                return;
            }

            base.WndProc(ref m);
        }
    }
}
