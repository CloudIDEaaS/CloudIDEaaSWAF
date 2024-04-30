using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils.PortableExecutable.Enums
{
    public enum DebugType
    {
        Unknown = 0, // Unknown value, ignored by all tools.
        COFF = 1,	// COFF debugging information (line numbers, symbol table, and string table). This type of debugging information is also pointed to by fields in the file headers.
        CodeView = 2,	// CodeView debugging information. The format of the data block is described by the CodeView 4.0 specification.
        FPO = 3,	// Frame pointer omission (FPO) information. This information tells the debugger how to interpret nonstandard stack frames, which use the EBP register for a purpose other than as a frame pointer.
        Misc = 4,	// Miscellaneous information.
        Exception = 5,	// Exception information.
        Fixup = 6,	// Fixup information.
        Borland = 9,	// Borland debugging information.
    }
}
