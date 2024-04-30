using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Utils
{
    public class HostedSynchronizationContext : SynchronizationContext
    {
        private const ControlExtensions.WindowsMessage WM_SENDCALLBACK = ControlExtensions.WindowsMessage.USER + 48925;
        private const ControlExtensions.WindowsMessage WM_POSTCALLBACK = ControlExtensions.WindowsMessage.USER + 48926;
        private Dictionary<int, CallbackState> callbacks;
        public NativeWindow NativeWindow { get; }
        private int index;
        private IManagedLockObject lockObject;

        public HostedSynchronizationContext(IntPtr window)
        {
            callbacks = new Dictionary<int, CallbackState>();
            lockObject = LockManager.CreateObject();

            NativeWindow = ControlExtensions.GetMessages(window, (m) =>
            {
                using (lockObject.Lock())
                {
                    if (m.Msg == (int)WM_POSTCALLBACK)
                    {
                        var callbackIndex = m.WParam;
                        var callbackState = callbacks[(int)callbackIndex];

                        callbackState.SendOrPostCallback(callbackState.State);

                        m.Result = (IntPtr)1;
                    }
                    else if (m.Msg == (int)WM_SENDCALLBACK)
                    {
                        var callbackIndex = m.WParam;
                        var callbackState = callbacks[(int)callbackIndex];

                        callbackState.SendOrPostCallback(callbackState.State);

                        m.Result = (IntPtr)1;
                    }

                    return true;
                }
            });
        }

        public override void Send(SendOrPostCallback d, object state)
        {
            using (lockObject.Lock())
            {
                var thisIndex = index;

                index++;
                callbacks.Add(thisIndex, new CallbackState(d, state, thisIndex));

                if (ControlExtensions.SendMessage(NativeWindow.Handle, WM_SENDCALLBACK, thisIndex, 0) == IntPtr.Zero)
                {
                    //DebugUtils.Break();
                }
            }
        }

        public override void Post(SendOrPostCallback d, object state)
        {
            using (lockObject.Lock())
            {
                index++;
                callbacks.Add(index, new CallbackState(d, state, index));

                if (!ControlExtensions.PostMessage(NativeWindow.Handle, WM_POSTCALLBACK, index, 0))
                {
                    DebugUtils.Break();
                }
            }
        }
    }
}
