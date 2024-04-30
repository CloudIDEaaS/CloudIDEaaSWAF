using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Utils
{
    [DataContract]
    public class LoggerRelayCaptureMethodType
    {
        public bool Enabled { get; set; }
        public string Type { get; set; }
        public List<string> Methods { get; set; }
    }
}
