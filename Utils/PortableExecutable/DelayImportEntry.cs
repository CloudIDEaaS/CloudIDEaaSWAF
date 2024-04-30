using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils.PortableExecutable
{
    public class DelayImportEntry : DataDirectoryEntry
    {
        public DelayImportEntry(ulong offset, ulong size) : base(offset, size)
        {
        }

        public override string DebugInfo
        {
            get { throw new NotImplementedException(); }
        }
    }
}
