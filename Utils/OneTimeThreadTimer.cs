using System;
using System.Net;
using System.Windows;
using System.Timers;
using System.Threading;

namespace Utils 
{
    public class OneTimeThreadTimer
    {
        private Thread internalThread;
        private bool elapsed;
        private double interval;
        private DateTime startTime;
        public event EventHandler Elapsed;

        public OneTimeThreadTimer(TimeSpan timeSpan, string threadName = null)
        {
            internalThread = new Thread(ThreadProc);
            interval = timeSpan.TotalMilliseconds;
            internalThread.Name = threadName;
        }

        public OneTimeThreadTimer(int milliseconds, string threadName = null)
        {
            internalThread = new Thread(ThreadProc);
            interval = milliseconds;
            internalThread.Name = threadName;
        }

        public void Continue()
        {
            elapsed = false;
            internalThread.Start();
        }

        public void Stop()
        {
            internalThread.Abort();
        }

        public void ThreadProc()
        {
            while (DateTime.Now - startTime < TimeSpan.FromMilliseconds(interval))
            {
                Thread.Sleep(1);
            }

            Elapsed(this, EventArgs.Empty);
        }

        public void Start(Action action)
        {
            elapsed = false;
            startTime = DateTime.Now;

            Elapsed += (sender, e) =>
            {
                if (!elapsed)
                {
                    action();
                    elapsed = true;
                }
            };

            internalThread.Start();
        }
    }
}
