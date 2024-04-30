using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Utils.Logging
{
    public interface ILineLogger : ILogger
    {
        public string SubFolderName { get; set; }
        public string LogFileName { get; set; }
    }

    public class LineLogger : ILineLogger
    {
        private string categoryName;
        private string path;
        public string SubFolderName { get; set; }
        public string LogFileName { get; set; }
        private IManagedLockObject lockObject; 

        public LineLogger(string path, string categoryName)
        {
            this.path = path;
            this.categoryName = categoryName;

            lockObject = LockManager.CreateObject();
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            throw new NotImplementedException();
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            throw new NotImplementedException();
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            var output = formatter(state, exception);
            var directory = new DirectoryInfo(Path.Combine(path, this.SubFolderName));
            var logFilePath = Path.Combine(directory.FullName, this.LogFileName);
            var logFile = new FileInfo(logFilePath);

            if (!directory.Exists)
            {
                directory.Create();
            }

            if (logFile.Exists)
            {
                logFile.MakeWritable();
            }

            using (lockObject.Lock())
            {
                using (var writer = File.AppendText(logFilePath))
                {
                    writer.WriteLine(output);
                    writer.Flush();
                }
            }
        }
    }
}
