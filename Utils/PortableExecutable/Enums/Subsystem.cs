using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils.PortableExecutable.Enums
{
    public enum Subsystem
    {
		Unknown = 0,	            // Unknown subsystem.  
		Native = 1,	                // No subsystem required (device drivers and native system processes).
		WindowsGui = 2,	            // Windows graphical user interface (GUI) subsystem.
		WindowsConsoleUI = 3,	            // Windows character-mode user interface (CUI) subsystem.
		Os2ConsoleUI = 5,	                // OS/2 CUI subsystem.
		PosixConsoleUI = 7,	            // POSIX CUI subsystem.
		WindowsCEGui = 9,	        // Windows CE system.
		EfiApplication = 10,	    // Extensible Firmware Interface (EFI) application.
		EfiBootServiceDriver = 11,	// EFI driver with boot services.
		EfiRuntimeDriver = 12,	    // EFI driver with run-time services.
		EfiROM = 13,	            // EFI ROM image.
		XBox = 14,	                // Xbox system.
		WindowsBootApp = 16,	// Boot application.
    }
}
