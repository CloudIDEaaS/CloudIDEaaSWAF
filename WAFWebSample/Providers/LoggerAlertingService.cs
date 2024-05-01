using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Collections.Generic;
using Utils;

namespace WAFWebSample.WebApi.Providers
{
    public class LoggerAlertingService : BaseThreadedService
    {
        private Queue<Action> actionQueue;
        private Queue<LogInfo> logQueue;
        public static event LoggerAlertEventHandler LoggerAlertEvent;

        public LoggerAlertingService() 
        {
            actionQueue = new Queue<Action>();
            logQueue = new Queue<LogInfo>();

            this.Start();
        }

        public override void DoWork(bool stopping)
        {
            var actions = new List<Action>();
            var logInfos = new List<LogInfo>();

            using (this.Lock())
            {
                while (actionQueue.Count > 0)
                {
                    var action = actionQueue.Dequeue();

                    actions.Add(action);
                }
            }

            if (actions.Count > 0)
            {
                foreach (var action in actions)
                {
                    action();
                }
            }

            using (this.Lock())
            {
                while (logQueue.Count > 0)
                {
                    var logInfo = logQueue.Dequeue();

                    logInfos.Add(logInfo);
                }
            }

            if (logInfos.Count > 0)
            {
                foreach (var logInfo in logInfos)
                {
                    var logger = logInfo.Logger;
                    var message = logInfo.Message;
                    var severity = logInfo.Severity;

                    if (LoggerAlertEvent != null)
                    {
                        LoggerAlertEvent(this, new LoggerAlertEventArgs(logInfo));
                    }

                    switch (severity)
                    {
                        case LogSeverity.Critical:
                            logger.LogCritical(message);
                            break;
                        case LogSeverity.Warning:
                            logger.LogWarning(message);
                            break;
                        case LogSeverity.Information:
                        default:
                            logger.LogInformation(message);
                            break;
                    }
                }
            }
        }

        public void QueueAction(Action action) 
        {
            using (this.Lock())
            {
                actionQueue.Enqueue(action);
            };
        }

        public void QueueLog(LogInfo logInfo)
        {
            using (this.Lock())
            { 
                logQueue.Enqueue(logInfo); 
            };
        }
    }
}
