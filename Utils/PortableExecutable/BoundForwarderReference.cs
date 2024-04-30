using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Utils;

namespace Utils.PortableExecutable
{
    public class BoundForwarderReference : DataDirectoryEntry
    {
        public uint TimeDateStamp { get; set; }
        public ushort OffsetModuleName { get; set; }
        public string ModuleName { get; set; }
        public ushort Reserved { get; set; }

        public BoundForwarderReference(ulong offset, ulong size) : base(offset, size)
        {
        }

        public override string DebugInfo
        {
            get 
            {
               return string.Format(
        			"TimeDateStamp: 0x{0:x8}, "
        			+ "OffsetModuleName: 0x{1:x8}, "
                    + "ModuleName: {2}, ,"
                    + "Reserved: 0x{3:x8}",
        			this.TimeDateStamp,
        			this.OffsetModuleName,
                    this.ModuleName,
                    this.Reserved);
            }
        }
    }
}
