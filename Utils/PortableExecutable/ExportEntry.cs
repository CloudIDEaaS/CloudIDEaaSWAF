using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils.PortableExecutable
{
    public class ExportEntry : DataDirectoryEntry
    {
        public override string DebugInfo
        {
            get { throw new NotImplementedException(); }
        }
    }
}
