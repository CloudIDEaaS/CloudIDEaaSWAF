using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils.PortableExecutable
{
    public abstract class DescriptorEntry : DataDirectoryEntry
    {
        public DescriptorEntry(ulong offset, ulong size) : base(offset, size)
        {
        }

        public override bool HasChildren
        {
            get
            {
                return true;
            }
        }

        public abstract override IEnumerable<DataDirectoryEntry> Children { get; }
        public abstract override string DebugInfo { get; }
    }
}
