using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Utils
{
    public class CallbackState
    {
        public SendOrPostCallback SendOrPostCallback { get; }
        public object State { get; }
        public int Index { get; }

        public CallbackState(SendOrPostCallback sendOrPostCallback, object state, int index)
        {
            SendOrPostCallback = sendOrPostCallback;
            State = state;
            Index = index;

            Debug.WriteLine("*********************** Created callback state, index: {0}, thread: {1}, stack: {2}", index, Thread.CurrentThread.ManagedThreadId, this.GetStackText(10, 4));
        }
    }
}
