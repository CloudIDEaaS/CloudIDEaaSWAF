using System;
using System.Collections.Generic;
using System.IO;

namespace Utils.PortableExecutable
{
    public interface IPEImage : IUnknown
    {
        bool InMemory { get; set; }
        IEnumerable<IDataDirectory> Directories { get; }
        DOSHeader DOSHeader { get; }
        IImageLayoutTree ImageLayoutTree { get; }
        BinaryReader ImageReader { get; }
        PEHeader PEHeader { get; }
        object SymbolData { get; }
    }
}
