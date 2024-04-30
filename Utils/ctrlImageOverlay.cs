using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Utils.Controls.ScreenCapture;
using static Utils.ControlExtensions;

namespace Utils
{
    public partial class ctrlImageOverlay : Control
    {
        public Image Image { get; set; }

        public ctrlImageOverlay()
        {
            InitializeComponent();

            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            BackColor = Color.Transparent;
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();

            this.SendToBack();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var rect = this.ClientRectangle;
            var parent = this.Parent;

            base.OnPaint(e);

            foreach (var control in parent.GetAllControls().Where(c => c != this))
            {
                var paintRect = this.Bounds;
                var controlRect = control.ClientRectangle;
                Rectangle imageOffsetRect;

                paintRect = this.RectangleToScreen(paintRect);
                paintRect = control.RectangleToClient(paintRect);

                if (controlRect.IntersectsWith(paintRect))
                {
                    imageOffsetRect = paintRect;

                    imageOffsetRect.Offset(-this.Left, -this.Top);

                    using (var controlGraphics = control.CreateGraphics())
                    {
                        controlGraphics.DrawImage(this.Image, imageOffsetRect);
                    }
                }
            }

            e.Graphics.DrawImage(this.Image, rect);
        }
    }
}
