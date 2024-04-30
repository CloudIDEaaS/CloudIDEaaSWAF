using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    [DebuggerDisplay(" { DebugInfo } ")]
#if !UTILS_INTERNAL
    public class CommandPacket<T>
#else
    internal class CommandPacket<T>
#endif
    {
        public string Type { get; set; }
        public string Command { get; set; }
        public DateTime ReceivedTimestamp { get; set; }
        public DateTime SentTimestamp { get; set; }
        public T Response { get; set; }
        private string debugInfo;

        public CommandPacket()
        {
        }

        public CommandPacket(string command, DateTime receivedTimestamp, T response)
        {
            this.Type = "response";
            this.Command = command;
            this.Response = response;
            this.ReceivedTimestamp = receivedTimestamp;
            this.SentTimestamp = DateTime.UtcNow;

            debugInfo = this.ToJsonText();
        }

        [JsonIgnore]
        public string DebugInfo
        {
            get
            {
                return debugInfo;
            }
        }
    }
}
