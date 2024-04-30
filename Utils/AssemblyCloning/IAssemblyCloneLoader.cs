using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Utils
{
    public interface IAssemblyCloneLoader : IDisposable
    { 
        _Assembly LoadAssemblyClone(string assemblyPath);
    }
}
