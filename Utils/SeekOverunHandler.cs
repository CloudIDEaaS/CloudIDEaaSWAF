using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Utils
{
    public delegate void SeekOverunHandler(object sender, SeekOverunEventArgs e);

    public class SeekOverunEventArgs
    {
        public long Offset { get; private set; }
        public long Position { get; set; }
        public bool ContinueBaseSeek { get; set; }
        public bool OutOfRegionPosition { get; set; }
        public SeekOrigin BaseSeekOrigin { get; set; }
        public long BaseOffset { get; set; }
        public ProcessSeekOrigin Location { get; private set; }
        public bool MemoryAllocated { get; set; }
    
        public SeekOverunEventArgs(long offset, ProcessSeekOrigin loc)
        {
            this.Offset = offset;
            this.Location = loc;
        }

    }
}
