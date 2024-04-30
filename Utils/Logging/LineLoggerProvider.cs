using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Utils.Logging
{
    public interface ILineLoggerProvider : ILoggerProvider
    {
        ILogger CreateLogger(string categoryName);
    }

    public class LineLoggerProvider : ILineLoggerProvider
    {
        private string path;

        public LineLoggerProvider(string path)
        {
            this.path = path;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new LineLogger(path, categoryName);
        }

        public void Dispose()
        {
        }
    }
}
