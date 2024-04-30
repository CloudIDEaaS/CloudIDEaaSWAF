using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Utils
{
    public class CtrlTabEventArgs : EventArgs
    {
        public Control SelectedControl { get; set; }
        public IntPtr SelectedWindow { get; set; }
        public bool CancelOperation { get; set; }

        public CtrlTabEventArgs(Control selectedControl)
        {
            this.SelectedControl = selectedControl;
        }

        public CtrlTabEventArgs(IntPtr selectedWindow)
        {
            this.SelectedWindow = selectedWindow;
        }
    }
}
