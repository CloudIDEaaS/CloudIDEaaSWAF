using System;
using System.Net;
using System.Windows;
using System.Timers;
using Timer = System.Timers.Timer;

namespace Utils
{
#if !UTILS_INTERNAL
    public class OneTimeTimer
#else
    internal class OneTimeTimer
#endif
    {
        private Timer internalTimer;
        private bool elapsed;
        private IManagedLockObject lockObject;

        public OneTimeTimer(TimeSpan timeSpan) : this((int) timeSpan.TotalMilliseconds)
        {
        }

        public OneTimeTimer(int milliseconds)
        {
            lockObject = LockManager.CreateObject();

            internalTimer = new Timer();
            internalTimer.Interval = milliseconds;
        }

        public void Continue()
        {
            elapsed = false;
            internalTimer.Start();
        }

        public void Stop()
        {
            internalTimer.Stop();
        }

        public static void Run(Action action, TimeSpan timeSpan)
        {
            var timer = new OneTimeTimer(timeSpan);

            timer.Start(action);
        }

        public static void Run(Action action, int timeSpan)
        {
            var timer = new OneTimeTimer(timeSpan);

            timer.Start(action);
        }

        public void Start(Action action)
        {
            elapsed = false;

            internalTimer.Elapsed += (sender, e) =>
            {
                using (lockObject.Lock())
                {
                    internalTimer.Stop();

                    if (!elapsed)
                    {
                        action();
                        elapsed = true;
                    }
                }
            };

            internalTimer.Start();
        }
    }
}
