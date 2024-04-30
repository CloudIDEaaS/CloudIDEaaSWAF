using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils.PortableExecutable.Enums
{
    [Flags]
    public enum DllCharacteristics : ushort
    {
        Reserved1 = 0x0001,			// Reserved.
        Reserved2 = 0x0002,			// Reserved.
        Reserved3 = 0x0004,			// Reserved.
        Reserved4 = 0x0008,			// Reserved.
        DynamicBase = 0x0040,		// The DLL can be relocated at load time.
        ForceIntegrity = 0x0080,	// Code integrity checks are forced. If you set this flag and a section contains only uninitialized data, set the PointerToRawData member of IMAGE_SECTION_HEADER for that section to zero; otherwise, the image will fail to load because the digital signature cannot be verified.
        NXCompat = 0x0100,			// The image is compatible with data execution prevention (DEP).
        NoIsolation = 0x0200,		// The image is isolation aware, but should not be isolated.
        NoSEH = 0x0400,				// The image does not use structured exception handling (SEH). No handlers can be called in this image.
        NoBind = 0x0800,			// Do not bind the image.
        Reserved5 = 0x1000,			// Reserved.
        WDMDriver = 0x2000,			// A WDM driver.
        Reserved6 = 0x4000,			// Reserved.
        TerminalServerAware = 0x8000,	// The image is terminal server aware.
    }
}
