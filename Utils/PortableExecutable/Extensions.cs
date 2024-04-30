using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils.PortableExecutable.Enums;

namespace Utils.PortableExecutable
{
    public static class Extensions
    {
        public static bool Is64Bit(this Machine machine)
        {
            return (machine & Machine.IA64) == Machine.IA64;
        }
    }
}
