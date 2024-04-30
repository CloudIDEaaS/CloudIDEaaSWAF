using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utils.PortableExecutable.Enums;

namespace Utils.PortableExecutable
{
    public interface IImageLayoutRelationship
    {
        IImageLayoutItemContainer Parent { get; }
        IImageLayoutItemContainer Child { get; }
        LayoutFlags Flags { get; set; }
    }
}
