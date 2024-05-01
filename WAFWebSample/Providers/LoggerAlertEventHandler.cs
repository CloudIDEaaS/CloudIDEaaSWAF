namespace WAFWebSample.WebApi.Providers
{
    public delegate void LoggerAlertEventHandler(object sender, LoggerAlertEventArgs e);

    public class LoggerAlertEventArgs
    {
        public LogInfo LogInfo { get; }

        public LoggerAlertEventArgs(LogInfo logInfo)
        {
            this.LogInfo = logInfo;
        }
    }
}
