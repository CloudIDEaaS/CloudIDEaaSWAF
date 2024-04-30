using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils.PortableExecutable.Enums
{
    [Flags]
    public enum ImageCharacteristics
    {
		RelocsStripped = 0x0001,	// Relocation information was stripped from the file. The file must be loaded at its preferred base address. If the base address is not available, the loader reports an error.
		ExecutableImage = 0x0002,	// The file is executable (there are no unresolved external references).
		LineNumsStripped = 0x0004,	// COFF line numbers were stripped from the file.
		LocalSymsStripped = 0x0008,	// COFF symbol table entries were stripped from file.
		AggresiveWSTrim = 0x0010,	// Aggressively trim the working set. This value is obsolete.
		LargeAddressAware = 0x0020,	// The application can handle addresses larger than 2 GB.
		BytesReversedLo = 0x0080,	// The bytes of the word are reversed. This flag is obsolete.
        Machine32Bit = 0x0100,	// The computer supports 32-bit words.
		DebugStripped = 0x0200,	// Debugging information was removed and stored separately in another file.
		RemovableRunFromSwap = 0x0400,	// If the image is on removable media, copy it to and run it from the swap file.
		NetRunFromSwap = 0x0800,	// If the image is on the network, copy it to and run it from the swap file.
		System = 0x1000,	// The image is a system file.
		DLL = 0x2000,	// The image is a DLL file. While it is an executable file, it cannot be run directly.
		UpSystemOnly = 0x4000,	// The file should be run only on a uniprocessor computer.
		BytesReversedHi = 0x8000,	// The bytes of the word are reverse
    }
}
