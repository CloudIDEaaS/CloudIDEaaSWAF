using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Utils
{
    public class LoggerRelayCaptureMethodsEntry
    {
        public bool Enabled { get; set; }
        public string Log { get; set; }
        public List<LoggerRelayCaptureMethodAssembly> Assemblies { get; set; }
    }
}
