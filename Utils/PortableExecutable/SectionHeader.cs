using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel;
using Utils.PortableExecutable.Enums;

namespace Utils.PortableExecutable
{
    [TreeImage(@"..\Images\SectionHeader.png")]
    public class SectionHeader : IImageLayoutItem
    {
        public ulong Offset { get; set; }
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
        public ulong Size { get; set; }
        public Guid UniqueId { get; private set; } 
		[Browsable(false)]
		public ISite Site { get; set; }
		public event EventHandler Disposed;

        public SectionHeader(Section section, ulong offset)
        {
            this.Name = section.Name;
            this.VirtualSize = section.VirtualSize;
            this.VirtualAddress = section.VirtualAddress;
            this.SizeOfRawData = section.SizeOfRawData;
            this.PointerToRawData = section.PointerToRawData;
            this.PointerToRelocations = section.PointerToRelocations;
            this.NumberOfRelocations = section.NumberOfRelocations;
            this.NumberOfLinenumbers = section.NumberOfLinenumbers;
            this.Characteristics = section.Characteristics;
            this.Size = section.Size;
            this.Offset = offset;
            this.UniqueId = Guid.NewGuid();
        }

		public void Dispose()
		{
			this.Raise(Disposed);
		}
    }
}
