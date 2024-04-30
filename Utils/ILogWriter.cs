using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public interface ILogWriter
    {
        IDisposable ErrorMode();
        void Write(string value);
        void Write(string format, params object[] args);
        void WriteLine(string value);
        void WriteLine();
        void WriteLine(string format, params object[] args);
    }
}
