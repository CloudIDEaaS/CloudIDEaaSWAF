using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Utils
{
    public class CueBannerTextBox : TextBox
    {
        public string CueBanner { get; set; }

        private void UpdateCueBanner()
        {
            if (IsHandleCreated && this.CueBanner != null)
            {
                if (ControlExtensions.SendMessage(this.Handle, ControlExtensions.WindowsMessage.EM_SETCUEBANNER, (IntPtr) 1, this.CueBanner) == IntPtr.Zero)
                {
                    DebugUtils.Break();
                }
            }
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);

            if (!this.InDesignMode())
            {
                UpdateCueBanner();
            }
        }
    }
}