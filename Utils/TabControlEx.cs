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
    public class TabControlEx : TabControl
    {
        /// <summary>
        /// Gets or sets a value indicating whether the tab headers should be drawn
        /// </summary>
        [Description("Gets or sets a value indicating whether the tab headers should be drawn"), DefaultValue(true)]
        public bool ShowTabHeaders { get; set; }

        public TabControlEx() : base()
        {
        }

        protected override void WndProc(ref Message m)
        {
            if (!ShowTabHeaders && m.Msg == 0x1328 && !DesignMode)
            {
                m.Result = (IntPtr)1;
            }
            else
            {
                base.WndProc(ref m);
            }
        }
    }
}
