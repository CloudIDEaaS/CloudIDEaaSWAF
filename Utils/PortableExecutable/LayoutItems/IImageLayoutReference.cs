using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils.PortableExecutable
{
    public interface IImageLayoutReference
    {
        IImageLayoutItemContainer Primary { get; }
        IImageLayoutItemContainer Referenced { get; }
        string ReferencingProperty { get; }
    }
}
