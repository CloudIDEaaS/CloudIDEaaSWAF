using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utils.PortableExecutable.Enums;

namespace Utils.PortableExecutable
{
    public class RelocationEntry : DataDirectoryEntry
    {
        public ushort OffsetFromBase { get; set; }
        public uint RVA { get; set; }
        public RelocationType Type { get; set; }

        public RelocationEntry(ulong offset, ulong size) : base(offset, size)
        {
        }

        public override string DebugInfo
        {
            get 
            {
                return string.Format(
        			"OffsetFromBase: 0x{0:x4}, "
        			+ "RVA: 0x{1:x8}, "
        			+ "Type: {2}",
        			this.OffsetFromBase,
        			this.RVA,
        			this.Type);
            }
        }
    }
}
