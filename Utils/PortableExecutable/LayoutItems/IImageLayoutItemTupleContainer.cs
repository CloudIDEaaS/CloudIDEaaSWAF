using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils.PortableExecutable
{
    public interface IImageLayoutItemTupleContainer : IImageLayoutItemGenericContainer
    {
        IEnumerable<KeyValuePair<string, string>> TupleValues { get; }
    }
}
