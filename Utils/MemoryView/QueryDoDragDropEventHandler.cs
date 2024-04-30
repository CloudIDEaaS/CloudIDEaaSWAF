using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Utils.MemoryView
{
    public delegate void QueryDoDragDropEventHandler(object sender, QueryDoDragDropEventArgs e);

    public class QueryDoDragDropEventArgs : EventArgs
    {
        public DragAction DragAction { get; set; }
        public DragDropEffects AllowedEffects { get; set; }
        public RangePainterDragDropDataObject Data { get; set; }
        public MouseEventArgs MouseEventArgs { get; private set; }

        public QueryDoDragDropEventArgs(MouseEventArgs args)
        {
            this.MouseEventArgs = args;
        }
    }
}
