using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils
{
    public interface IRegistryKey
    {
        string KeyName { get; }
    }

    public interface IRegistryKeyWithSubKeys : IRegistryKey
    {
        IEnumerable<object> SubKeys { get; }
    }
}
