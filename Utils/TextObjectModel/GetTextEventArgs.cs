using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Utils.TextObjectModel
{
    public class GetTextEventArgs : EventArgs
    {
        public ITextRange Range { get; private set; }
        public string Text { get; set; }

        public GetTextEventArgs(ITextRange range)
        {
            this.Range = range;
        }
    }
}
