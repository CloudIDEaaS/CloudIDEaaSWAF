using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utils.MemoryView.TextObjectModel;
using Utils;
using Utils.TextObjectModel;

namespace Utils.MemoryView.TextObjectModel
{
    public interface IMemoryTextDocument : ITextDocument
    {
        event EventHandlerT<ITextSelection> SelectionChanged;
        event EventHandler<GetRectEventArgs> OnGetRect;
        event EventHandler<GetTextEventArgs> OnGetText;
        IList<ITextLine> Lines { get; }
    }
}
