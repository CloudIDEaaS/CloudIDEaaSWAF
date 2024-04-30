using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils.PortableExecutable.Enums
{
    public enum DirectoryId
    {
        Export,
        Import,
        Resource,
        Exception,
        Security,
        Relocation,
        Debug,
        Architecture,
        GlobalPointer,
        TLS,
        LoadConfiguration,
        BoundImport,
        ImportAddress,
        DelayImport,
        CLRMetaData,
        Unknown,
    }
}
