using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Utils
{
    public static class GuidExtensions
    {
        [DllImport("rpcrt4.dll", SetLastError = true)]
        public static extern int UuidCreate(out Guid guid);
        
    }
}
