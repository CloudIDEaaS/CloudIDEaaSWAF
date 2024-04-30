using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils
{
    public class ConsoleTimer : IDisposable 
    {
        public bool Enabled = false;
        private int interval = 1000;
        private int nextTick = 0;
        public string Name { get; set; }
        public event EventHandler Tick;
        private static IManagedLockObject lockObject;
        private static List<ConsoleTimer> timers;

        static ConsoleTimer()
        {
            lockObject = LockManager.CreateObject();

            timers = new List<ConsoleTimer>();
        }

        public ConsoleTimer(string name)
        {
            this.Name = name;

            using (var _lock = lockObject.Lock())
            {
                timers.Add(this);
            }
        }

        public static void ClearTimers()
        {
            timers.Clear();
        }

        public static IEnumerable<ConsoleTimer> Timers
        {
            get
            {
                IEnumerable<ConsoleTimer> timers;

                using (var _lock = lockObject.Lock())
                {
                    timers = ConsoleTimer.timers.ToList();
                }

                return timers;
            }
        }

        public void RaiseTick()
        {
            if (Enabled && Environment.TickCount >= nextTick)
            {
                Tick.Invoke(this, null);
                nextTick = Environment.TickCount + Interval;
            }
        }

        public void Start()
        {
            this.Enabled = true;
            Interval = interval;
        }

        public void Stop()
        {
            this.Enabled = false;
        }

        public int Interval
        {
            get 
            {
                return interval; 
            }
            
            set 
            { 
                interval = value; 
                nextTick = Environment.TickCount + interval; 
            }
        }

        public void Dispose()
        {
            this.Tick = null;
            this.Stop();
        }
    }
}
