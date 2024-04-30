using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils.PortableExecutable.Enums
{
    public enum ResourceId : uint
    {
        Unknown = 0,
		Cursor = 1,
		Bitmap = 2,
		Icon = 3,
		Menu = 4,
		DialogBox = 5,
		StringTableEntry = 6,
		FontDirectory = 7,
		Font = 8,
		AcceleratorTable = 9,
		RawData = 10,
		MessageTableEntry = 11,
		GroupCursor = 12,
		GroupIcon = 14,
		VersionInformation = 16,
		DlgInclude = 17,
		PlugAndPlayResource = 19,
		VXD = 20,
		AnimatedCursor = 21,
		AnimatedIcon = 22,
		HTML = 23,
		ConfigurationFile = 24
    }
}
