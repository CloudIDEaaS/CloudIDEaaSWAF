using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ProcessDiagnosticsLibrary
{
    [Serializable, DataContractAttribute]
    public class TestRegion
    {
        public IntPtr BaseAddress { get; set; }
        public IntPtr AllocationBase { get; set; }
        public int AllocationProtect { get; set; }
        public IntPtr RegionSize { get; set; }
        public int State { get; set; }
        public int Protect { get; set; }
        public int Type { get; set; }
        public string RegionName { get; set; }
    }
}
