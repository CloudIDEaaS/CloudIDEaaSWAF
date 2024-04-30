using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Utils
{
    public class WindowWrapper : IWin32Window
    {
        private IntPtr hwnd;

        public WindowWrapper(IntPtr handle)
        {
            hwnd = handle;
        }

        public IntPtr Handle
        {
            get { return hwnd; }
        }
    }
}
