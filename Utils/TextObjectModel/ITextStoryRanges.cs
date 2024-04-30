using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Utils.TextObjectModel
{
    public interface ITextStoryRanges : IList<ITextRange>
    {
        ITextRange this[string index] { get; }
        int Count { get; }
    }
}
