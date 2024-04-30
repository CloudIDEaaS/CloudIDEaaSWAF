using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils.PortableExecutable
{
    [Flags]
    public enum CodeTypeFlags
    {
        Unknown,
        X86Assembly = 1,
        MSIL = 2,
        CSharp = 4,
        StreamOffset = 8,
        Bytes = 16,
        Custom = 32
    }
}
