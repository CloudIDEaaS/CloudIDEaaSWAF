//using PostSharp.Aspects.Advices;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace Utils
{
    public class LoggerRelayEvent
    {
        public LogEvent LogEvent { get; }
        public string LogFileName { get; }

        public LoggerRelayEvent(LogEvent logEvent, string logFileName)
        {
            this.LogEvent = logEvent;
            this.LogFileName = logFileName;
        }
    }
}
