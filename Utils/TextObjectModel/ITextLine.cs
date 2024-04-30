using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utils.TextObjectModel;

namespace Utils.TextObjectModel
{
    public interface ITextLine : ITextParagraph
    {
        IList<string> TextItems { get; }
    }
}
