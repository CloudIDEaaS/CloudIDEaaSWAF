using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Utils;
using System.ComponentModel;
using Utils.PortableExecutable.Enums;

namespace Utils.PortableExecutable
{
    [DebuggerDisplay(" { DebugInfo } "), TreeImage(@"..\Images\Section{ Name }.png")]
    public class COFFSection : IImageLayoutItem
    {
        public string Name { get; set; }
        public ulong VirtualSize { get; set; }
        public ulong VirtualAddress { get; set; }
        public ulong SizeOfRawData { get; set; }
        public ulong PointerToRawData { get; set; }
        public ulong PointerToRelocations { get; set; }
        public ulong PointerToLinenumbers { get; set; }
        public ushort NumberOfRelocations { get; set; }
        public ushort NumberOfLinenumbers { get; set; }
        public DataSectionFlags Characteristics { get; set; }
        public byte[] SectionData { get; set; }
        public ulong Size { get; set; }
        public Guid UniqueId { get; private set; } 
		[Browsable(false)]
		public ISite Site { get; set; }
		public event EventHandler Disposed;

        public COFFSection()
        {
            this.UniqueId = Guid.NewGuid();
        }

        public string DebugInfo
        {
            get
            {
                return string.Format("Name: {0}, VirtualAddress: 0x{1:x8}, VirtualSizeHex: 0x{2:x8}, VirtualSize: {2}, RawSizeHex: 0x{3:x8}, RawSize: {3}", this.Name, this.VirtualAddress, this.VirtualSize, this.SizeOfRawData);
            }
        }

		public void Dispose()
		{
			this.Raise(Disposed);
		}
    }
}
