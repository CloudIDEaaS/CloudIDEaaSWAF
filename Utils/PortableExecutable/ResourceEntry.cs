using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils.PortableExecutable
{
    public abstract class ResourceEntry : DataDirectoryEntry
    {
        public ResourceEntry(ulong offset, ulong size) : base(offset, size)
        {
        }

        public override abstract string DebugInfo { get; }
    }
}
