using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils.PortableExecutable.Enums
{
    public enum OptionalMagic
    {
		Magic32 = 0x10b,	// The file is an executable image.
		Magic64 = 0x20b,	// The file is an executable image.
        MagicROM = 0x107,	    // The file is a ROM image.    
    }
}
