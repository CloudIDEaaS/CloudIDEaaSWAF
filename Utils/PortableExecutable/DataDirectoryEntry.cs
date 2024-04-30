using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.ComponentModel;

namespace Utils.PortableExecutable
{
    [DebuggerDisplay(" { DebugInfo } ")]
    public abstract class DataDirectoryEntry : IImageLayoutItem
    {
        public ulong Offset { get; set; }
        public ulong Size { get; set; }
        public Guid UniqueId { get; private set; } 
		[Browsable(false)]
		public ISite Site { get; set; }
		public event EventHandler Disposed;

        public DataDirectoryEntry(ulong offset, ulong size)
        {
            this.Offset = offset;
            this.Size = size;
            this.UniqueId = Guid.NewGuid();
        }

        public virtual bool HasChildren
        {
            get
            {
                return false;
            }
        }

        public virtual IEnumerable<DataDirectoryEntry> Children
        {
            get
            {
                return null;
            }
        }

        public abstract string DebugInfo { get; }

		public void Dispose()
		{
			this.Raise(Disposed);
		}
    }
}
