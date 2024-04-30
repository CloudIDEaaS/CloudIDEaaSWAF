using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Utils.PortableExecutable
{
    [TreeImage(@"..\Images\DataDirectories.png")]
    public class DataDirectories : IImageLayoutItem
    {
        public uint DirectoryCount { get; private set; }
        public Guid UniqueId { get; private set; } 
		[Browsable(false)]
		public ISite Site { get; set; }
		public event EventHandler Disposed;

        public DataDirectories(uint directoryCount)
        {
            this.DirectoryCount = directoryCount;
            this.UniqueId = Guid.NewGuid();
        }

		public void Dispose()
		{
			this.Raise(Disposed);
		}
    }
}
