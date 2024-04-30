using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Utils
{
    public class OverlayScreenBase : Form
    {
        protected Screen screen;

        public OverlayScreenBase(Screen initialScreen = null)
        {
            this.screen = initialScreen;

            this.Capture = true;
            this.MouseMove += OverlayScreenBase_MouseMove;
        }

        private void OverlayScreenBase_MouseMove(object sender, MouseEventArgs e)
        {
            var screenPoint = this.PointToScreen(e.Location);

            if (!this.Capture)
            {
                this.Capture = true;
            }

            foreach (var screen in Screen.AllScreens)
            {
                if (screen.Bounds.Contains(screenPoint))
                {
                    if (!screen.Bounds.Contains(this.Location))
                    {
                        this.WindowState = FormWindowState.Normal;
                        this.CenterOver(screen);
                        this.WindowState = FormWindowState.Maximized;

                        this.DoEvents();
                    }

                    break;
                }
            }
        }
    }
}
