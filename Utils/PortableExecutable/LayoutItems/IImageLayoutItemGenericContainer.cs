using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils.PortableExecutable
{
    public interface IImageLayoutItemGenericContainer : IImageLayoutItemContainer
    {
        object Item { get; }
    }
}
