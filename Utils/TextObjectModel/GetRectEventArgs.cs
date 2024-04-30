using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Utils.TextObjectModel
{
    public class GetRectEventArgs : EventArgs
    {
        public ITextRange Range { get; private set; }
        public Rectangle Rect { get; set; }

        public GetRectEventArgs(ITextRange range)
        {
            this.Range = range;
        }
    }
}
