using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Utils
{
    public class LogWriter : ILogWriter
    {
        public string Path { get; }

        public LogWriter(string path, bool deleteExistingLog = false)
        {
            this.Path = path;

            if (deleteExistingLog && File.Exists(path))
            {
                try
                {
                    File.Delete(path);
                }
                catch
                {
                    Thread.Sleep(100);
                    File.Delete(path);
                }
            }
        }

        public void Write(string value)
        {
            File.AppendAllText(this.Path, value);
        }

        public void Write(string format, params object[] args)
        {
            File.AppendAllText(this.Path, string.Format(format, args));
        }

        public void WriteLine(string value)
        {
            File.AppendAllText(this.Path, value + "\r\n");
        }

        public void WriteLine(string format, params object[] args)
        {
            File.AppendAllText(this.Path, string.Format(format + "\r\n", args));
        }

        public void WriteLine()
        {
            File.AppendAllText(this.Path, "\r\n");
        }

        public IDisposable ErrorMode()
        {
            throw new NotImplementedException();
        }
    }
}
