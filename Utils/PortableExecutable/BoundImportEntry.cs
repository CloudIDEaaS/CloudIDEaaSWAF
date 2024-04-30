using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils.PortableExecutable
{
    public class BoundImportEntry : DataDirectoryEntry
    {
        public override string DebugInfo
        {
            get { throw new NotImplementedException(); }
        }
    }
}
