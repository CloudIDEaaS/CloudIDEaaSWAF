using Serilog.Core;
using Serilog.Events;
using SuperSocket.ClientEngine;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using SuperSocket.ProtoBase;
using System.Text.RegularExpressions;
using Serilog.Formatting.Display;

namespace Utils.Logging
{
    public class ReceiveFilter : TerminatorReceiveFilter<StringPackageInfo>
    {
        public ReceiveFilter() : base(Encoding.ASCII.GetBytes("||")) // two vertical bars as package terminator
        {
        }

        public override StringPackageInfo ResolvePackage(IBufferStream bufferStream)
        {
            return null;
        }
    }

    public class TraceLogEventSink : BaseThreadedService, ILogEventSink
    {
        public string Address { get; private set; }
        public int Port { get; }
        public CancellationToken CancellationToken { get; }
        private TraceLogEventSinkConfigArgs traceLogEventSinkConfigArgs;
        private FixedDictionary<LogEvent, LogEvent> queuedEvents;
        private EasyClient client;
        private string ID { get; }
        private int lastEventId;

        public TraceLogEventSink(string ipAddress, int port, CancellationToken cancellationToken, TraceLogEventSinkConfigArgs traceLogEventSinkConfigArgs = null) : base()
        {
            queuedEvents = new FixedDictionary<LogEvent, LogEvent>(NumberExtensions.MB);

            this.Address = ipAddress;
            this.Port = port;
            this.CancellationToken = cancellationToken;
            this.traceLogEventSinkConfigArgs = traceLogEventSinkConfigArgs;
            this.ID = Guid.NewGuid().ToString();

            this.Start();

            cancellationToken.Register(() =>
            {
                this.Stop();
            });
        }

        public override void DoWork(bool stopping)
        {
            if (client == null)
            {
                var client = new EasyClient();
                bool connected;
                IPAddress address;

                client.NoDelay = true;

                client.Initialize(new ReceiveFilter(), (request) =>
                {
                });

                if (!IPAddress.TryParse(this.Address, out address))
                {
                    address = Dns.GetHostAddresses(this.Address).Where(a => a.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).FirstOrDefault();

                    if (address == null)
                    {
                        return;
                    }

                    this.Address = address.ToString();
                }

                connected = client.ConnectAsync(new IPEndPoint(IPAddress.Parse(this.Address), this.Port)).Result;

                if (connected)
                {
                    this.client = client;

                    SendQueuedItems();
                }
                else
                {
                    Thread.Sleep(100);
                }
            }
            else
            {
                SendQueuedItems();
            }
        }

        private void SendQueuedItems()
        {
            Queue<LogEvent> queue;
            var batchBuilder = new StringBuilder();

            using (this.Lock())
            {
                queue = new Queue<LogEvent>(queuedEvents.Values.ToList());
                queuedEvents.Clear();
            }

            while (queue.Count > 0)
            {
                var logEvent = queue.Dequeue();
                var logMessage = logEvent.MessageTemplate.Text;
                string source = null;
                string eventId = null;
                string eventMessage;

                logMessage = logEvent.RenderMessage();

                foreach (var property in logEvent.Properties)
                {
                    if (property.Key == "SourceContext")
                    {
                        source = property.Value.ToString().FormatCommandLineArg();
                    }
                    else if (property.Key == "EventId")
                    {
                        eventId = property.Value.ToString().FormatCommandLineArg();
                    }
                    else
                    {
                        //var pattern = $"\\{{\\s*?{ Regex.Escape(property.Key) }\\s*\\}}";

                        //logMessage = logMessage.RegexReplace(pattern, property.Value.ToString().RemoveQuotes());
                    }
                }
                
                if (eventId == null)
                {
                    eventId = $"{{ Id: { ++lastEventId }, Name: { source } }}".FormatCommandLineArg();
                }

                logMessage = logMessage.FormatCommandLineArg();
                eventMessage = string.Format("EventId:{0} Timestamp:{1} Source:{2} Level:{3} LogMessage:{4}\t", eventId, DateTime.UtcNow.ToDateTimeText().SurroundWithQuotes(), source, logEvent.Level, logMessage);

                batchBuilder.Append(eventMessage);
            }

            if (batchBuilder.Length > 0)
            {
                try
                {
                    var eventMessage = string.Format("LOGBATCH {0}\r\n", batchBuilder.ToString());

                    client.Send(Encoding.ASCII.GetBytes(eventMessage));
                }
                catch
                {
                    Thread.Sleep(100);
                }
            }
        }

        public void Emit(LogEvent logEvent)
        {
            var property = logEvent.Properties.SingleOrDefault(p => p.Key == "SourceContext");
            var source = property.Value.ToString().RemoveQuotes();
            string commandText;

            if (logEvent.MessageTemplate.Text.Contains("An error occurred using a transaction."))
            {
            }
            else if (logEvent.MessageTemplate.Text.StartsWith("Executed DbCommand"))
            {
                commandText = logEvent.Properties.Single(p => p.Key == "commandText").Value.ToString();
            }

            if (traceLogEventSinkConfigArgs != null)
            {
                if (this.traceLogEventSinkConfigArgs.IncludeOnlySources != null && !this.traceLogEventSinkConfigArgs.IncludeOnlySources.Contains(source))
                {
                    return;
                }
                else if (this.traceLogEventSinkConfigArgs.ExcludeSources != null && this.traceLogEventSinkConfigArgs.ExcludeSources.Contains(source))
                {
                    return;
                }
            }

            using (this.Lock())
            {
                queuedEvents.Add(logEvent, logEvent);
            }
        }

        ~TraceLogEventSink()
        {
            this.Stop();
        }
    }
}
