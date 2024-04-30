using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils.PortableExecutable
{
    public class ExportDescriptor : DescriptorEntry
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

        internal static List<ExportDescriptor> ReadExports(global::System.IO.BinaryReader reader, DataDirectory dataDirectory)
        {
            throw new NotImplementedException();
        }
    }
}
