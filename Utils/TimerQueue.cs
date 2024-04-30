using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public delegate void WaitOrTimerDelegate(IntPtr lpParameter, bool timerOrWaitFired);

    public enum ExecuteFlags
    {

        /// <summary>
        /// By default, the callback function is queued to a non-I/O worker thread.
        /// </summary>
        WT_EXECUTEDEFAULT = 0x00000000,
        /// <summary>
        /// The callback function is invoked by the timer thread itself. This flag should be used only for short tasks or it could affect other timer operations.
        /// The callback function is queued as an APC. It should not perform alertable wait operations.
        /// </summary>
        WT_EXECUTEINTIMERTHREAD = 0x00000020,
        /// <summary>
        /// The callback function is queued to an I/O worker thread. This flag should be used if the function should be executed in a thread that waits in an alertable state.
        /// The callback function is queued as an APC. Be sure to address reentrancy issues if the function performs an alertable wait operation.
        /// </summary>
        WT_EXECUTEINIOTHREAD = 0x00000001,

        /// <summary>
        /// The callback function is queued to a thread that never terminates. It does not guarantee that the same thread is used each time. This flag should be used only for short tasks or it could affect other timer operations.
        /// Note that currently no worker thread is truly persistent, although no worker thread will terminate if there are any pending I/O requests.
        /// </summary>
        WT_EXECUTEINPERSISTENTTHREAD = 0x00000080,

        /// <summary>
        /// The callback function can perform a long wait. This flag helps the system to decide if it should create a new thread.
        /// </summary>
        WT_EXECUTELONGFUNCTION = 0x00000010,

        /// <summary>
        /// The timer will be set to the signaled state only once. If this flag is set, the Period parameter must be zero.
        /// </summary>
        WT_EXECUTEONLYONCE = 0x00000008,
        /// <summary>
        /// Callback functions will use the current access token, whether it is a process or impersonation token. If this flag is not specified, callback functions execute only with the process token.
        /// Windows XP/2000:  This flag is not supported until Windows XP with SP2 and Windows Server 2003.
        /// </summary>
        WT_TRANSFER_IMPERSONATION = 0x00000100
    };

    internal class ApiMethods
    {
        [DllImport("kernel32.dll")]
        public static extern IntPtr CreateTimerQueue();
        [DllImport("kernel32.dll")]
        public static extern bool DeleteTimerQueue(IntPtr timerQueue);
        [DllImport("kernel32.dll")]
        public static extern bool CreateTimerQueueTimer(out IntPtr phNewTimer, IntPtr timerQueue, WaitOrTimerDelegate callback, IntPtr parameter, uint dueTime, uint period, ExecuteFlags flags);
    }

    public class TimerQueue<T> : IDisposable
    {
        private IntPtr hTimerQueue;
        private List<TimerQueueCallback<T>> callbacks;

        public TimerQueue()
        {
            callbacks = new List<TimerQueueCallback<T>>();
            hTimerQueue = ApiMethods.CreateTimerQueue();

            if (hTimerQueue == IntPtr.Zero)
            {
                DebugUtils.Break();
            }
        }

        //public void Start()
        //{
        //    foreach (var callback in callbacks)
        //    {
        //        IntPtr newTimer;

        //        if (!ApiMethods.CreateTimerQueueTimer(out newTimer, hTimerQueue, callback.WaitOrTimer, Marshal.GetIUnknownForObject(callback.CallbackObject), callback.OffsetMilliseconds, 0, ExecuteFlags.WT_EXECUTEDEFAULT | ExecuteFlags.WT_EXECUTEONLYONCE))
        //        {
        //            DebugUtils.Break();
        //        }
        //    }
        //}

        public void AddCallback(TimerQueueCallback<T> callback)
        {
            IntPtr newTimer;

            if (!ApiMethods.CreateTimerQueueTimer(out newTimer, hTimerQueue, callback.WaitOrTimer, Marshal.GetIUnknownForObject(callback.CallbackObject), callback.OffsetMilliseconds, 0, ExecuteFlags.WT_EXECUTEINTIMERTHREAD | ExecuteFlags.WT_EXECUTEONLYONCE))
            {
                DebugUtils.Break();
            }

            callbacks.Add(callback);
        }

        public void Dispose()
        {
            ApiMethods.DeleteTimerQueue(hTimerQueue);
        }
    }
}
