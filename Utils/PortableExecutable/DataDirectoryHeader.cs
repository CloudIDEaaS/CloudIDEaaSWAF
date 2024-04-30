using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.ComponentModel;
using Utils.PortableExecutable.Enums;

namespace Utils.PortableExecutable
{
    [DebuggerDisplay(" { DebugInfo } "), TreeImage(@"..\Images\DataDirectoryHeader.png")]
    public class DataDirectoryHeader : IImageLayoutItem
    {
        public DirectoryId Directory { get; set; }
        public ulong Address { get; set; }
        public ulong Size { get; set; }
        public Guid UniqueId { get; private set; } 
		[Browsable(false)]
		public ISite Site { get; set; }
		public event EventHandler Disposed;

        public DataDirectoryHeader(DataDirectory directory)
        {
            this.Directory = directory.Directory;
            this.Address = directory.Address;
            this.Size = directory.Size;
            this.UniqueId = Guid.NewGuid();
        }

        public string Name
        {
            get
            {
                return string.Format("{0} DirectoryHeader", this.Directory);
            }
        }

        public string DebugInfo
        {
            get
            {
                return string.Format(
        			"{0}, "
        			+ "Address: 0x{1:x8}, "
        			+ "Size: 0x{2:x8}",
        			this.Directory,
        			this.Address,
        			this.Size);
            }
        }

		public void Dispose()
		{
			this.Raise(Disposed);
		}
    }
}
