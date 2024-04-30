using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Utils.PortableExecutable
{
    public interface IImageLayoutCodeProviderItem : IImageLayoutItem
    {
        IEnumerable<CodeSource> GetCodePoints();
    }
}
