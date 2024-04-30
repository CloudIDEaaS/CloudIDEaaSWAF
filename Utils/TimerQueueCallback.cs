using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public class TimerQueueCallback<T>
    {
        public Action<T> CallbackAction { get; set; }
        public uint OffsetMilliseconds { get; set; }
        public T CallbackObject { get; set; }
        internal IntPtr Timer { get; set; }

        public TimerQueueCallback(T callbackObject, uint offsetMilliseconds, Action<T> callbackAction)
        {
            this.CallbackObject = callbackObject;
            this.OffsetMilliseconds = offsetMilliseconds;
            this.CallbackAction = callbackAction;
        }

        internal void WaitOrTimer(IntPtr lpParameter, bool timerOrWaitFired)
        {
            var obj = (T) Marshal.GetObjectForIUnknown(lpParameter);

            CallbackAction(obj);
        }
    }
}
