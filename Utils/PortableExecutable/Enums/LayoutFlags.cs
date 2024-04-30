using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils.PortableExecutable.Enums
{
    [Flags]
    public enum LayoutFlags
    {
        AddressSizeValid = 0,
        NoAddress = 1,
        NoSize = 2,
        NoAddressOrSize = 3
    }
}
