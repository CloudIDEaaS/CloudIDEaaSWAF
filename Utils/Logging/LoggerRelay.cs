using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Utils
{
    public interface ILoggerRelay
    {
        string LogName { get; }
        string Domain { get; }
        public string LogFileName { get; }
        public string RootPath { get; }
        IDisposable RelayLogging(string logFileName, bool backupOldFile = true);
        void Initialize(LoggerRelayEventSinkConfigArgs logRelayEventSinkConfigArgs);
    }

    public class LoggerRelay : ILoggerRelay
    {
        public string Domain { get; }
        public string RootPath { get; private set; }
        private bool clearSetting;

        public LoggerRelay(string domain, bool clearSetting = false)
        {
            this.Domain = domain;
            this.clearSetting = clearSetting;
        }

        public string LogFileName
        {
            get
            {
                var logFileName = InterprocessVariables.GetVariable(this.RootPath, this.Domain, "RelayLogFileName");
                string logFileFullName = null;

                if (!logFileName.IsNullOrEmpty())
                {
                    logFileFullName = Path.Combine(this.RootPath, logFileName);
                }

                return logFileFullName;
            }
        }

        public string LogName
        {
            get
            {
                var logFileName = this.LogFileName;

                if (logFileName != null)
                {
                    return Path.GetFileNameWithoutExtension(logFileName);
                }
                else
                {
                    return null;
                }
            }
        }

        public void Initialize(LoggerRelayEventSinkConfigArgs logRelayEventSinkConfigArgs)
        {
            this.RootPath = Path.GetFullPath(Environment.ExpandEnvironmentVariables(logRelayEventSinkConfigArgs.RootPath));

            if (clearSetting)
            {
                ClearSetting();
            }
        }

        private void ClearSetting()
        {
            InterprocessVariables.SetVariable(this.RootPath, this.Domain, "RelayLogFileName", string.Empty);
        }

        public IDisposable RelayLogging(string logName, bool backupOldFile = true)
        {
            var logFileName = logName + ".log";
            string logFileFullName;

            var disposable = this.CreateDisposable(() =>
            {
                ClearSetting();
            });

            logFileFullName = Path.Combine(this.RootPath, logFileName);

            if (backupOldFile)
            {
                if (File.Exists(logFileFullName))
                {
                    try
                    {
                        var fileInfo = new FileInfo(logFileFullName);
                        var backupFileName = Path.Combine(fileInfo.DirectoryName, string.Format("{0} {1}", DateTime.Now.ToSortableDateTimeText(), fileInfo.Name));

                        File.Copy(logFileFullName, backupFileName);
                        File.Delete(logFileFullName);
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }

            InterprocessVariables.SetVariable(this.RootPath, this.Domain, "RelayLogFileName", logFileName);

            return disposable;
        }
    }
}
