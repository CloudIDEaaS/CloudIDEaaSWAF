using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Utils.ControlExtensions;

namespace Utils.ActiveControls
{
    public class ToolStrip : System.Windows.Forms.ToolStrip
    {
        private static bool isDown = false;

        protected override void WndProc(ref Message m)
        {
            var windowsMessage = (WindowsMessage)m.Msg;

            if (windowsMessage == WindowsMessage.LBUTTONUP && !isDown)
            {
                m.Msg = (int)WindowsMessage.LBUTTONDOWN;
                base.WndProc(ref m);
                m.Msg = (int)WindowsMessage.LBUTTONUP;
            }

            windowsMessage = (WindowsMessage)m.Msg;

            if (windowsMessage == WindowsMessage.LBUTTONDOWN)
            {
                isDown = true;
            }

            if (windowsMessage == WindowsMessage.LBUTTONUP)
            {
                isDown = false;
            }

            base.WndProc(ref m);
        }
    }
}
