using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils.PortableExecutable
{
    public class TLSEntry : DataDirectoryEntry
    {
        public TLSEntry(ulong offset, ulong size) : base(offset, size)
        {
        }

        public override string DebugInfo
        {
            get { throw new NotImplementedException(); }
        }
    }
}
