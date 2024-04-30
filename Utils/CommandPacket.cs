using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    [Serializable]
    [DebuggerDisplay(" { DebugInfo } ")]
#if !UTILS_INTERNAL
    public class CommandPacket
#else
    internal class CommandPacket
#endif
    {
        public string Type { get; set; }
        public string Command { get; set; }
        public KeyValuePair<string, object>[] Arguments { get; set; }
        public DateTime ReceivedTimestamp { get; set; }
        public DateTime SentTimestamp { get; set; }
        public bool IsChainedStream { get; set; }
        public object Response { get; set; }
        private string debugInfo;

        public CommandPacket()
        {
            debugInfo = "CommandPacket";
        }

        public CommandPacket(string command, Dictionary<string, object> arguments)
        {
            this.Type = "request";
            this.Command = command;
            this.Arguments = arguments.ToArray();
            this.SentTimestamp = DateTime.UtcNow;

            debugInfo = this.ToJsonText();
        }

        public CommandPacket(string command, params KeyValuePair<string, object>[] arguments)
        {
            this.Type = "request";
            this.Command = command;
            this.Arguments = arguments;
            this.SentTimestamp = DateTime.UtcNow;

            debugInfo = this.ToJsonText();
        }

        public CommandPacket(string command, DateTime receivedTimestamp, object response)
        {
            this.Type = "response";
            this.Command = command;
            this.Response = response;
            this.ReceivedTimestamp = receivedTimestamp;
            this.SentTimestamp = DateTime.UtcNow;

            debugInfo = this.ToJsonText();
        }

        public CommandPacket(string command, DateTime receivedTimestamp)
        {
            this.Type = "response";
            this.Command = command;
            this.ReceivedTimestamp = receivedTimestamp;
            this.SentTimestamp = DateTime.UtcNow;

            debugInfo = this.ToJsonText();
        }

        public CommandPacket(string command, DateTime receivedTimestamp, object response, bool isChainedStream)
        {
            this.Type = "response";
            this.Command = command;
            this.Response = response;
            this.ReceivedTimestamp = receivedTimestamp;
            this.SentTimestamp = DateTime.UtcNow;
            this.IsChainedStream = isChainedStream;

            debugInfo = this.ToJsonText();
        }

        public CommandPacket(string command, DateTime receivedTimestamp, object response, params KeyValuePair<string, object>[] arguments)
        {
            this.Type = "response";
            this.Command = command;
            this.Response = response;
            this.Arguments = arguments;
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
