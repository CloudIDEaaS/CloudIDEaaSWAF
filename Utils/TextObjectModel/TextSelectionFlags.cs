using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils.TextObjectModel
{
    [Flags]
    public enum TextSelectionFlags
    {
        StartActive = 1,
        AtEOL = 2,
        Overtype = 4,
        Active = 8,
        Replace = 16
    }
}
