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
    public partial class TipWindow : Form
    {
        public int AnimationDelay { get; set; } = 1000;
        public bool KeepAlive { get; }

        public TipWindow(bool keepAlive)
        {
            this.KeepAlive = keepAlive;

            InitializeComponent();
        }

        public void CloseOrHide()
        {
            if (this.KeepAlive)
            {
                this.Hide();
            }
            else
            {
                this.Close();
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            this.FadeIn(this.AnimationDelay);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            this.FadeOut(this.AnimationDelay);
        }
    }
}
