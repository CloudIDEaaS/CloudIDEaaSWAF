using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils.PortableExecutable.Enums
{
    public enum Machine
    {
		I386 = 0x014c,	// x86
		IA64 = 0x0200,	// Intel Itanium
		AMD64 = 0x8664,	// x64
    }
}
