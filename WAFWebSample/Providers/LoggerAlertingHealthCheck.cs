using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Diagnostics;
using System.Text;
using Utils;

namespace WAFWebSample.WebApi.Providers
{
    public class LoggerAlertingHealthCheck : IHealthCheck
    {
        private static Queue<LogInfo> criticalLogQueue;
        private static FixedList<LogInfo> distinctLogList;
        private static bool eventHandlerAttached;

        public LoggerAlertingHealthCheck()
        {
            if (!eventHandlerAttached)
            {
                criticalLogQueue = new Queue<LogInfo>();
                distinctLogList = new FixedList<LogInfo>(1024);

                LoggerAlertingService.LoggerAlertEvent += LoggerAlertingService_LoggerAlertEvent;
                eventHandlerAttached = true;
            }
        }

        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            var result = GetResult();

            return Task.FromResult(result);
        }

        private static void LoggerAlertingService_LoggerAlertEvent(object sender, LoggerAlertEventArgs e)
        {
            var logInfo = e.LogInfo;

            if (logInfo.Severity == LogSeverity.Critical)
            {
                criticalLogQueue.Enqueue(logInfo);
            }
        }

        public HealthCheckResult GetResult()
        {
            HealthCheckResult healthCheckResult;
            var builder = new StringBuilder();

            while (criticalLogQueue.Count > 0)
            {
                var logInfo = criticalLogQueue.Dequeue();

                if (!distinctLogList.Any(i => i.Message == logInfo.Message))
                {
                    distinctLogList.Add(logInfo);
                }
            }

            foreach (var logInfo in distinctLogList)
            {
                builder.AppendLine(logInfo.Message);
            }

            if (builder.Length > 0)
            {
                healthCheckResult = HealthCheckResult.Unhealthy(builder.ToString());
            }
            else
            {
                healthCheckResult = HealthCheckResult.Healthy();
            }

            return healthCheckResult;
        }
    }
}
