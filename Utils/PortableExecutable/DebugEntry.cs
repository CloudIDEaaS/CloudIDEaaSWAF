using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils.PortableExecutable
{
    public class DebugEntry : DataDirectoryEntry
    {
        public override string DebugInfo
        {
            get { throw new NotImplementedException(); }
        }

        internal static List<DebugEntry> ReadDebugEntries(global::System.IO.BinaryReader reader, DataDirectory dataDirectory)
        {
            throw new NotImplementedException();
        }
    }
}
