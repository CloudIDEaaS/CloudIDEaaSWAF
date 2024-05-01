namespace WAFWebSample.WebApi.Providers
{
    public class LogInfo
    {
        public string Message { get; }
        public LogSeverity Severity { get; }
        public ILogger Logger { get; }

        public LogInfo(string message, LogSeverity severity, ILogger logger)
        {
            this.Message = message;
            this.Severity = severity;
            this.Logger = logger;
        }
    }
}
