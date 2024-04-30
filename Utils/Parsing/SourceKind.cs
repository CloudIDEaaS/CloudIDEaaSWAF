using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils.Parsing
{
    enum SourceKind
    {
        Unknown = 0,
        JS = 1,
        JSX = 2,
        TS = 3,
        TSX = 4,
        CLRText = 5
    }
}
