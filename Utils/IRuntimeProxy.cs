using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public interface IRuntimeProxy
    {
        T CallMethod<T>(string identifier, string methodName, params object[] args);
        T PropertyGet<T>(string identifier, string propertyName);
        void PropertySet<T>(string identifier, string propertyName, T value);
    }
}
