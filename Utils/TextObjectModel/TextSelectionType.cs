using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils.TextObjectModel
{
    public enum TextSelectionType
    {
        NoSelection = 0,
        IP = 1,
        Normal = 2,
        Frame = 3,
        Column = 4,
        Row = 5,
        Block = 6,
        InlineShape = 7,
        Shape = 8
    }
}
