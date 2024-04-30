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
using Serilog.Formatting.Display;
using System.IO;
using Serilog.Formatting;

namespace Utils
{
    public class LoggerRelayEventSink : BaseThreadedService, ILogEventSink
    {
        public string Domain { get; }
        public string RootPath { get; }
        public CancellationToken CancellationToken { get; }
        private string ID { get; }
        public bool Stopped { get; private set; }
        private LoggerRelayEventSinkConfigArgs loggerRelayEventSinkConfigArgs;
        private ILoggerRelay loggerRelay;
        private ITextFormatter formatter;
        private FixedDictionary<LoggerRelayEvent, LogEvent> queuedEvents;
        private Mutex mutex;
        private Dictionary<string, Dictionary<string, LoggerRelayCaptureMethodAssembly>> captureStack;

        public LoggerRelayEventSink(ILoggerRelay loggerRelay, CancellationToken cancellationToken, LoggerRelayEventSinkConfigArgs loggerRelayEventSinkConfigArgs = null) : base(ThreadPriority.Highest, 100)
        {
            bool mutexCreated;
            var outputTemplate = "{Timestamp:yyyy-MM-dd hh:mm:ss.fff} [{Level}] {SourceContext} {Message}";

            DateTime.Now.ToDebugDateTimeText();

            this.Domain = loggerRelay.Domain;
            this.RootPath = Environment.ExpandEnvironmentVariables(loggerRelayEventSinkConfigArgs.RootPath);
            this.CancellationToken = cancellationToken;
            this.ID = Guid.NewGuid().ToString();

            try
            {
                mutex = Mutex.OpenExisting(string.Format(@"Global\{0}+Mutex", this.Domain));
            }
            catch
            {
                mutex = new Mutex(false, string.Format(@"Global\{0}+Mutex", this.Domain), out mutexCreated);
            }

            this.loggerRelayEventSinkConfigArgs = loggerRelayEventSinkConfigArgs;
            this.loggerRelay = loggerRelay;
            this.formatter = new MessageTemplateTextFormatter(outputTemplate);
            this.queuedEvents = new FixedDictionary<LoggerRelayEvent, LogEvent>(NumberExtensions.MB);

            if (loggerRelayEventSinkConfigArgs != null)
            {
                if (!loggerRelayEventSinkConfigArgs.Enabled)
                {
                    return;
                }

                captureStack = loggerRelayEventSinkConfigArgs.CaptureStack.Where(c => c.Enabled).ToDictionary(c => c.Log, c => c.Assemblies.Where(a => a.Enabled).ToDictionary(a => a.Assembly, a => a));
            }   

            this.Start();

            cancellationToken.Register(() =>
            {
                this.Stopped = true;
            });
        }

        public override void DoWork(bool stopping)
        {
            var batchBuilder = new StringBuilder();
            string logMessage = null;
            string logFileName = null;
            var success = false;
            var x = 0;

            using (this.Lock())
            {
                if (queuedEvents.Count > 0)
                {
                    var logRelayEvent = queuedEvents.Peek();
                    var buffer = new StringWriter(new StringBuilder(256));

                    formatter.Format(logRelayEvent.LogEvent, buffer);

                    logMessage = buffer.ToString();
                    logFileName = logRelayEvent.LogFileName;
                }
            }

            if (logMessage != null)
            {
                while (!success)
                {
                    try
                    {
                        var fileInfo = new FileInfo(logFileName);

                        if (!fileInfo.Directory.Exists)
                        {
                            fileInfo.Directory.Create();
                        }

                        WriteToLog(logFileName, logMessage + "\r\n");
                        success = true;
                    }
                    catch
                    {
                        if (x == 3)
                        {
                            break;
                        }

                        Thread.Sleep(1);
                        x++;
                    }
                }

                if (success)
                {
                    using (this.Lock())
                    {
                        queuedEvents.Dequeue();
                    }
                }
            }
        }

        public void Emit(LogEvent logEvent)
        {
            var property = logEvent.Properties.SingleOrDefault(p => p.Key == "SourceContext");
            var source = property.Value.ToString().RemoveQuotes();
            var buffer = new StringWriter(new StringBuilder(256));
            var logFileName = loggerRelay.LogFileName;
            string commandText;
            string logMessage;

            formatter.Format(logEvent, buffer);
            logMessage = buffer.ToString();

            if (logEvent.MessageTemplate.Text.Contains("An error occurred using a transaction."))
            {
            }
            else if (logEvent.MessageTemplate.Text.StartsWith("Executed DbCommand"))
            {
                commandText = logEvent.Properties.Single(p => p.Key == "commandText").Value.ToString();
            }

            if (loggerRelayEventSinkConfigArgs != null)
            {
                if (!loggerRelayEventSinkConfigArgs.Enabled)
                {
                    return;
                }
                else if (this.loggerRelayEventSinkConfigArgs.IncludeOnlySources != null && !this.loggerRelayEventSinkConfigArgs.IncludeOnlySources.Contains(source))
                {
                    return;
                }
                else if (this.loggerRelayEventSinkConfigArgs.ExcludeSources != null && this.loggerRelayEventSinkConfigArgs.ExcludeSources.Contains(source))
                {
                    return;
                }
            }

            if (logFileName != null)
            {
                var success = false;
                var x = 0;
                var captureAssemblies = captureStack[Path.GetFileNameWithoutExtension(logFileName)];

                if (captureAssemblies.InCaptureStack())
                {
                    while (!success)
                    {
                        try
                        {
                            var fileInfo = new FileInfo(logFileName);

                            if (!fileInfo.Directory.Exists)
                            {
                                fileInfo.Directory.Create();
                            }

                            WriteToLog(logMessage, logFileName);
                            success = true;
                        }
                        catch
                        {
                            if (x == 3)
                            {
                                using (this.Lock())
                                {
                                    queuedEvents.Add(new LoggerRelayEvent(logEvent, logFileName), logEvent);
                                }

                                break;
                            }

                            Thread.Sleep(1);
                            x++;
                        }
                    }
                }
            }
        }

        private void WriteToLog(string logMessage, string logFileName)
        {
            mutex.WaitOne();

            File.AppendAllText(logFileName, logMessage + "\r\n");

            mutex.ReleaseMutex();
        }

        public object GetFormat(Type formatType)
        {
            if (formatType == typeof(ICustomFormatter))
            {
                return this;
            }
            else
            {
                return null;
            }
        }

        public string Format(string format, object arg, IFormatProvider formatProvider)
        {
            return arg.ToString();
        }

        ~LoggerRelayEventSink()
        {
            this.Stop();
        }
    }
}
