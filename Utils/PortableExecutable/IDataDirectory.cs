using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utils.PortableExecutable;
using Utils.PortableExecutable.Enums;

namespace Utils.PortableExecutable
{
    public interface IDataDirectory
    {
        DirectoryId Directory { get; }
        ulong Address { get; }
        ulong Size { get; }
        Section Section { get; }
        Machine Machine { get; }
        IEnumerable<DataDirectoryEntry> Entries { get; }
    }
}
