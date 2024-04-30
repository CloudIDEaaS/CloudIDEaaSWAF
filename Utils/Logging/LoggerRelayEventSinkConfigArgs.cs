using Serilog.Core;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using System.Text.RegularExpressions;
using System.Runtime.Serialization;

namespace Utils
{
    [DataContract]
    public class LoggerRelayEventSinkConfigArgs
    {
        [DataMember(IsRequired=false)]
        public string RestrictedToMinimumLevel { get; set; }
        [DataMember(IsRequired = false)]
        public List<string> ExcludeSources { get; set; }
        [DataMember(IsRequired = false)]
        public List<string> IncludeOnlySources { get; set; }
        [DataMember(IsRequired = true)]
        public string RootPath { get; set; }
        [DataMember(IsRequired = false)]
        public bool Enabled { get; set; }
        [DataMember(IsRequired = false)]
        public List<LoggerRelayCaptureMethodsEntry> CaptureStack { get; set; }
    }
}
