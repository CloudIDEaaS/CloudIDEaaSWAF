using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Utils.Controls.ScreenCapture
{
    public partial class TipWindow : Form
    {
        private OverlayScreen overlayScreen;

        public string Tip
        {
            get
            {
                return label.Text;
            }

            set
            {
                label.Text = value;
            }
        }

        public TipWindow(OverlayScreen overlayScreen) 
        {
            this.overlayScreen = overlayScreen;


            InitializeComponent();
        }

        private void TipWindow_Load(object sender, EventArgs e)
        {
            this.Left = overlayScreen.Left + overlayScreen.Width - this.Width - 25;
            this.Top = overlayScreen.Top + overlayScreen.Height - this.Height - 50;

            this.FadeIn(1000);
        }

        private void label_Click(object sender, EventArgs e)
        {

        }
    }
}
