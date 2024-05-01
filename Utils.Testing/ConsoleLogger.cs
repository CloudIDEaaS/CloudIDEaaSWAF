using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using Utils;

namespace Utils
{
    public class ConsoleLogger<T> : ILogger<T>
    {
        public IDisposable BeginScope<TState>(TState state)
        {
            return DebugUtils.BreakReturnNull();
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            var entry = formatter(state, exception);

            Console.WriteLine(entry);
        }
    }
}
