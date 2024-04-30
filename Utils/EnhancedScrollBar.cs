using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Utils
{
    public class EnhancedVScrollBar : VScrollBar
    {
        public event EventHandler ScrollInfoChanged;

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == (uint)Utils.ControlExtensions.WindowsMessage.SETSCROLLINFO)
            {
                if (ScrollInfoChanged != null)
                {
                    ScrollInfoChanged(this, EventArgs.Empty);
                }
            }

            base.WndProc(ref m);
        }
    }

    public class EnhancedHScrollBar : HScrollBar
    {
        public event EventHandler ScrollInfoChanged;

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (m.Msg == (uint)Utils.ControlExtensions.WindowsMessage.SETSCROLLINFO)
            {
                if (ScrollInfoChanged != null)
                {
                    ScrollInfoChanged(this, EventArgs.Empty);
                }
            }
        }
    }
}
