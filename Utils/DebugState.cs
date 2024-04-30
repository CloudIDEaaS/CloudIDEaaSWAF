using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public class DebugState
    {
        public string ProcessName { get; set; }
        public string ExceptionMessage { get; set; }
        public StringBuilder StackTrace { get; private set; }
        public string ExceptionText { get; internal set; }

        public DebugState()
        {
            this.StackTrace = new StringBuilder();
        }

        public void WriteLine(string format, params object[] args)
        {
            this.StackTrace.AppendLineFormat(format, args);
        }
    }
}
