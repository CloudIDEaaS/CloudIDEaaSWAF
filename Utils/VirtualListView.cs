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
    public class VirtualListView : ListView
    {
        public VirtualListView()
        {
            this.Scrollable = false;
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            var rect = this.ClientRectangle;

            rect.Inflate(-50, -50);

            base.OnHandleCreated(e);

            this.SendMessage<Rectangle>(ControlExtensions.WindowsMessage.LVM_SETWORKAREAS, 1, rect);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            var rect = this.ClientRectangle;

            rect.Inflate(-50, -50);

            base.OnSizeChanged(e);

            this.SendMessage<Rectangle>(ControlExtensions.WindowsMessage.LVM_SETWORKAREAS, 1, rect);
        }
    }
}
