using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils.PortableExecutable
{
    public class ExportData : DataDirectoryEntry
    {
        public uint AddressOfFunction { get; set; }
        public uint? AddressOfName { get; set; }
        public string Name { get; set; }
        public ushort? NameOrdinal { get; set; }
        public uint? Ordinal { get; set; }

        public ExportData(ulong offset, ulong size) : base(offset, size)
        {
        }

        public override string DebugInfo
        {
            get 
            {
                return string.Format(
        			"AddressOfFunction: 0x{0:x8}, "
        			+ "AddressOfName: 0x{1:x8}, "
        			+ "Name: {2}, "
        			+ "NameOrdinal: 0x{3:x4}, "
        			+ "Ordinal: 0x{4:x8}, ",
        			this.AddressOfFunction,
        			this.AddressOfName,
        			this.Name,
        			this.NameOrdinal,
        			this.Ordinal);
            }
        }
    }
}
