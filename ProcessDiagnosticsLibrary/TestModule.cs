using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using System.IO;

namespace ProcessDiagnosticsLibrary
{
    [Serializable, DataContractAttribute]
    public class TestModule
    {
        public string FileName { get; set; }
        public IntPtr BaseAddress { get; set; }
        public int ModuleMemorySize { get; set; }

        [XmlIgnore]
        public string ModuleName 
        {
            get
            {
                return Path.GetFileName(this.FileName);
            }
        }
    }
}
