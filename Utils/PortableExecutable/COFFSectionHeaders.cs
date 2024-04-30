using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.ComponentModel;

namespace Utils.PortableExecutable
{
    [DebuggerDisplay(" { DebugInfo } "), TreeImage(@"..\Images\SectionHeaders.png")]
    public class COFFSectionHeaders : List<COFFSectionHeader>, IImageLayoutItem
    {
        public uint SectionCount { get; private set; }
        public Guid UniqueId { get; private set; }
        public event EventHandler Disposed;
        [Browsable(false)]
		public ISite Site { get; set; }

        public COFFSectionHeaders(uint sectionCount)
        {
            this.SectionCount = sectionCount;
            this.UniqueId = Guid.NewGuid();
        }

        public string DebugInfo
        {
            get
            {
                return string.Format("SectionCount: {0}", this.SectionCount);
            }
        }

        public void Dispose()
        {
            this.Raise(Disposed);
        }
    }
}
