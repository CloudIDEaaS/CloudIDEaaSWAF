using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils.PortableExecutable
{
    public interface IImageLayoutItemContainer : IImageLayoutItem
    {
        IImageLayoutItem Item { get; }
        ulong Offset { get; }
        ulong Size { get; }
        string Name { get; }
    }
}
