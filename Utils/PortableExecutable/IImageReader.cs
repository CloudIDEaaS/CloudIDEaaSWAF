using System;
using System.Collections.Generic;
using Utils.PortableExecutable;


namespace Utils.PortableExecutable
{
    public interface IImageReader : IUnknown
    {
        IEnumerable<DataDirectoryEntry> ReadDirectory(DataDirectory directory);
        IPEImage ReadImage(string imageFile, bool index = false);
    }
}
