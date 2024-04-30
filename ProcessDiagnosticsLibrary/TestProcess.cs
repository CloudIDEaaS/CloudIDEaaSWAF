using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ProcessDiagnosticsLibrary
{
    [Serializable, DataContractAttribute]
    public class TestProcess
    {
        public uint ProcessId { get; set; }
        public string FileName { get; set; }
        public ulong BaseAddress { get; set; }
        public ulong SizeOfImage { get; set; }
        public TestModule[] Modules { get; set; }
    }
}
