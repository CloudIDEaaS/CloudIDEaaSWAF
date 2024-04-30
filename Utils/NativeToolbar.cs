using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Utils
{
    public class NativeToolbar : System.Windows.Forms.NativeWindow
    {
        private IntPtr hwnd;
        NativeToolbarButtons buttons;

        public NativeToolbar(IntPtr hwnd)
        {
            this.hwnd = hwnd;
            base.AssignHandle(hwnd);

            buttons = new NativeToolbarButtons(this);
        }

        public NativeToolbarButtons Buttons
        {
            get
            {
                return buttons;
            }
        }

        internal void Refresh(NativeToolbarButton button)
        {
            buttons.Refresh(button);
        }
    }
}
