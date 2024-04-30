using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Utils
{
    public class RotatePushButton : CheckBox
    {
        private string text;
        private bool painting = false;

        public enum RotationType { None, Right, Flip, Left }

        [DefaultValue(RotationType.None), Category("Appearance"), Description("Rotates Button Text")]
        public RotationType Rotation { get; set; }

        public RotatePushButton()
        {
            base.Appearance = Appearance.Button;
        }

        [ReadOnly(true)]
        public new Appearance Appearance
        {
            get
            {
                return base.Appearance;
            }
        }

        public override string Text
        {
            get
            {
                if (!painting)
                    return text;
                else
                    return "";
            }
            set
            {
                text = value;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            painting = true;

            base.OnPaint(e);

            StringFormat format = new StringFormat();
            Int32 lNum = (Int32)Math.Log((Double)this.TextAlign, 2);
            format.LineAlignment = (StringAlignment)(lNum / 4);
            format.Alignment = (StringAlignment)(lNum % 4);

            int padding = 2;

            SizeF txt = e.Graphics.MeasureString(Text, this.Font);
            SizeF sz = e.Graphics.VisibleClipBounds.Size;
            switch (Rotation)
            {
                case RotationType.Right:  //90 degrees
                    {
                        e.Graphics.TranslateTransform(sz.Width, 0);
                        e.Graphics.RotateTransform(90);
                        break;
                    }
                case RotationType.Flip: //180 degrees
                    {
                        e.Graphics.TranslateTransform(sz.Width, sz.Height);
                        e.Graphics.RotateTransform(180);
                        break;
                    }
                case RotationType.Left: //270 degrees
                    {
                        e.Graphics.TranslateTransform(0, sz.Height);
                        e.Graphics.RotateTransform(270);
                        break;
                    }
                default: //0 = 360 degrees
                    {
                        e.Graphics.TranslateTransform(0, 0);
                        e.Graphics.RotateTransform(0);
                        break;
                    }
            }

            e.Graphics.DrawString(text, this.Font, Brushes.Black, new RectangleF(padding, padding, sz.Height - padding, sz.Width - padding), format);
            e.Graphics.ResetTransform();

            painting = false;
        }
    }
}
