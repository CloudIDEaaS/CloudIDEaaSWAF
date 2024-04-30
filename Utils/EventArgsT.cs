using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Utils
{
#if !UTILS_INTERNAL
    public class EventArgs<T> : EventArgs
#else
    internal class EventArgs<T> : EventArgs
#endif
    {
        public T Value { get; set; }

        [DebuggerStepThrough]
        public EventArgs(T value)
        {
            this.Value = value;
        }
    }
}
