using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Collections.Generic;

namespace Utils.Controls.ColorPicker
{
    public partial class OverlayScreen : OverlayScreenBase
    {
        Graphics graphics;
        private Form m_InstanceRef = null;
        public Color? Color { get; private set; }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                e = null;
            }

            base.OnMouseClick(e);
        }

        public Form InstanceRef
        {
            get
            {
                return m_InstanceRef;
            }
            set
            {
                m_InstanceRef = value;
            }
        }

        public OverlayScreen()
        {
            InitializeComponent();

            this.MouseUp += new MouseEventHandler(mouse_Up);
            this.KeyUp += new KeyEventHandler(OverlayScreen_KeyUp);

            graphics = this.CreateGraphics();

        }

        public void CaptureColor(bool showCursor, Size size, Point curPos)
        {
            using (Bitmap bitmap = new Bitmap(size.Width, size.Height))
            {
                using (Graphics graphics = Graphics.FromImage(bitmap))
                {
                    graphics.CopyFromScreen(curPos, Point.Empty, size);
                }

                this.Color = bitmap.GetPixel(0, 0);
            }
        }

        public void SaveColor(bool showCursor)
        {
            var curPos = new Point(Cursor.Position.X, Cursor.Position.Y);
            var curSize = new Size(1, 1);

            this.Opacity = 0;
            this.DoEvents();

            CaptureColor(showCursor, curSize, curPos);

            this.InstanceRef.Show();
            this.Close();
        }

        public void OverlayScreen_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode.ToString() == "Escape")
            {
                this.InstanceRef.Show();
                this.Close();
            }
        }

        private void mouse_Up(object sender, MouseEventArgs e)
        {
            SaveColor(false);
        }
   }
}