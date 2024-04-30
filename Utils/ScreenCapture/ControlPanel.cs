using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Linq;

namespace Utils.Controls.ScreenCapture
{
    public partial class ScreenCaptureControlPanel : Form
    {
        private Form m_InstanceRef = null;
        public Bitmap Image { get; private set; }

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

        public ScreenCaptureControlPanel()
        {
            InitializeComponent();
        }

        public ScreenCaptureControlPanel(bool Save)
        {
            InitializeComponent();
            this.FormClosed += new FormClosedEventHandler(Form_Close);
        }

        public void Key_Press(object sender, KeyEventArgs e)
        {
            KeyTest(e);
        }

        private void KeyTest(KeyEventArgs e)
        {
            if (e.KeyCode.ToString() == "S")
            {
                ScreenCapture(true);
            }
        }

        private void Form_Close(object sender, FormClosedEventArgs e)
        {
        }

        private void btnCaptureArea_Click(object sender, EventArgs e)
        {
            var screen = Screen.AllScreens.Single(s => s.DeviceName.EndsWith(upDownScreens.Text));
            var overlayScreen = new OverlayScreen(screen);
            ImageEditor imageEditor;

            this.Hide();

            if (this.InstanceRef != null)
            {
                this.InstanceRef.Hide();
            }

            this.InstanceRef.HideConsole();

            overlayScreen.InstanceRef = this;
            overlayScreen.Show();

            while (overlayScreen.IsHandleCreated)
            {
                this.DoEventsSleep(1000);
            }

            this.Image = overlayScreen.Image;

            imageEditor = new ImageEditor(this.Image);

            imageEditor.Show();

            while (imageEditor.IsHandleCreated)
            {
                this.DoEventsSleep(1000);
            }

            this.InstanceRef.ShowConsole();

            if (this.InstanceRef != null)
            {
                this.InstanceRef.Show();
            }

            this.Image = imageEditor.Image;

            if (imageEditor.DialogResult == DialogResult.OK)
            {
                this.Close();
                this.DialogResult = DialogResult.OK;
            }
        }

        private void btnCaptureScreen_Click(object sender, EventArgs e)
        {
            var image = ScreenCapture(false);
            ImageEditor imageEditor;

            this.Hide();

            if (this.InstanceRef != null)
            {
                this.InstanceRef.Hide();
            }

            this.InstanceRef.HideConsole();

            this.Image = image;
            this.Visible = false;

            imageEditor = new ImageEditor(this.Image);

            imageEditor.Show();

            while (imageEditor.IsHandleCreated)
            {
                this.DoEventsSleep(1000);
            }

            this.InstanceRef.ShowConsole();

            if (this.InstanceRef != null)
            {
                this.InstanceRef.Show();
            }

            this.Visible = true;
            this.Image = imageEditor.Image;

            if (imageEditor.DialogResult == DialogResult.OK)
            {
                this.Close();
                this.DialogResult = DialogResult.OK;
            }
        }

        public Bitmap ScreenCapture(bool showCursor)
        {
            Point curPos = new Point(Cursor.Position.X, Cursor.Position.Y);
            Size curSize = new Size();
            curSize.Height = Cursor.Current.Size.Height;
            curSize.Width = Cursor.Current.Size.Width;
            Bitmap image;

            //Conceal this form while the screen capture takes place
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            this.TopMost = false;

            //Allow 250 milliseconds for the screen to repaint itself (we don't want to include this form in the capture)
            System.Threading.Thread.Sleep(250);

            Rectangle bounds = Screen.GetBounds(Screen.GetBounds(Point.Empty));

            image = ScreenShot.CaptureImage(showCursor, curSize, curPos, Point.Empty, Point.Empty, bounds);

            //The screen has been captured and saved to a file so bring this form back into the foreground
            this.WindowState = System.Windows.Forms.FormWindowState.Normal;
            this.TopMost = true;

            return image;
        }

        private void ControlPanel_Load(object sender, EventArgs e)
        {
            this.KeyUp += new KeyEventHandler(Key_Press);

            System.Text.Encoding Encoder = System.Text.ASCIIEncoding.Default;
            Byte[] buffer = new byte[] { (byte)149 };
            string bullet = System.Text.Encoding.GetEncoding(1252).GetString(buffer);

            txtTips.Visible = true;

            txtTips.Text =
            bullet + "To capture an area:" + Environment.NewLine +
            "  Click and hold down the left mouse button." + Environment.NewLine +
            "  Draw the selection area required." + Environment.NewLine +
            "  Once the selection area is drawn you can" + Environment.NewLine +
            "  drag it to move it." + Environment.NewLine + Environment.NewLine +
            "  The selection area can also be resized at its" + Environment.NewLine +
            "  four sides and four corners." + Environment.NewLine + Environment.NewLine +
            "  Once you are satisfied with the selection area" + Environment.NewLine +
            "  simply double click anywhere within or outside" + Environment.NewLine +
            "  the selection area to save the selection to an" + Environment.NewLine +
            "  image file." + Environment.NewLine + Environment.NewLine +
            bullet + " With this form active press the 'S' key to" + Environment.NewLine +
            "  capture the screen with the cursor included." + Environment.NewLine +
            "  Alternatively press the 'S' key when you" + Environment.NewLine +
            "  have selected an area in order to include the" + Environment.NewLine +
            "  cursor in the area captured.";

            foreach (var screen in Screen.AllScreens)
            {
                upDownScreens.Items.Add(screen.DeviceName.RemoveStartIfMatches(@"\\.\"));
            }

            upDownScreens.SelectedIndex = 0;
        }

        private void saveToClipboard_CheckedChanged(object sender, EventArgs e)
        {
            ScreenShot.saveToClipboard = saveToClipboard.Checked;
        }

        private void saveToClipboard_KeyUp(object sender, KeyEventArgs e)
        {
            KeyTest(e);
        }

        private void bttCaptureArea_KeyUp(object sender, KeyEventArgs e)
        {
            KeyTest(e);
        }

        private void bttTips_KeyUp(object sender, KeyEventArgs e)
        {
            KeyTest(e);
        }

        private void bttCaptureScreen_KeyUp(object sender, KeyEventArgs e)
        {
            KeyTest(e);
        }

        private void txtTips_KeyUp(object sender, KeyEventArgs e)
        {
            KeyTest(e);
        }
    }
}