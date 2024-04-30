using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils.PortableExecutable
{
    public class RelocationDescriptor : DescriptorEntry
    {
        public override IEnumerable<DataDirectoryEntry> Children
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override string DebugInfo
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        internal static List<RelocationDescriptor> ReadRelocations(global::System.IO.BinaryReader reader, DataDirectory dataDirectory)
        {
            throw new NotImplementedException();
        }
    }
}
