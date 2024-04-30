using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Utils
{
    public abstract class BaseThreadedService : ILockable
    {
        private Thread workerThread;
        private bool shutdownFlag;
        private ThreadPriority threadPriority;
        private IManagedLockObject lockObject;
        private TimeSpan iterationSleep;
        private DateTime firstCheck;
        private DateTime lastProcessing;
        private ManualResetEvent runningEvent;
        private TimeSpan iterationTimeOut;
        private TimeSpan startTimeOut;
        public event EventHandler Started;
        public abstract void DoWork(bool stopping);
        private bool isRunning;
        protected bool processingHalted;
        public bool Working { get; private set; }

        public bool IsRunning
        {
            get
            {
                return isRunning;
            }
        }

        public DateTime StartTime { get; private set; }

        public BaseThreadedService(ThreadPriority threadPriority, TimeSpan iterationSleep, TimeSpan iterationTimeOut, TimeSpan startTimeOut)
        {
            this.threadPriority = threadPriority;
            this.iterationSleep = iterationSleep;
            this.iterationTimeOut = iterationTimeOut;
            this.startTimeOut = startTimeOut;
            runningEvent = new ManualResetEvent(false);

            lockObject = LockManager.CreateObject();
        }
        
        public BaseThreadedService(ThreadPriority threadPriority, int iterationSleepMilliseconds, int iterationTimeOutMilliseconds, int startTimeOutMilliseconds) : this(threadPriority, TimeSpan.FromMilliseconds(iterationSleepMilliseconds), TimeSpan.FromMilliseconds(iterationTimeOutMilliseconds), TimeSpan.FromMilliseconds(startTimeOutMilliseconds))
        {
        }

        public BaseThreadedService(ThreadPriority threadPriority) : this(threadPriority, TimeSpan.FromMilliseconds(100), TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(15))
        {
        }
        public BaseThreadedService(ThreadPriority threadPriority, int iterationSleepMilliseconds) : this(threadPriority, TimeSpan.FromMilliseconds(iterationSleepMilliseconds), TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(15))
        {
        }


        public BaseThreadedService() : this(ThreadPriority.Lowest)
        {
        }

        public IDisposable Lock()
        {
            return lockObject.Lock();
        }

        public T LockReturn<T>(Func<T> func)
        {
            T returnVal;

            using (this.Lock())
            {
                returnVal = func();
            }

            return returnVal;
        }

        public void LockSet(Action action)
        {
            using (this.Lock())
            {
                action();
            }
        }

        public bool TryLock(out IDisposable disposable, int millisecondsTimeOut = 0)
        {
            return lockObject.TryLock(out disposable, millisecondsTimeOut);
        }

        public virtual void Start()
        {
            workerThread = new Thread(WorkerThreadProc);

            workerThread.Priority = threadPriority;
            workerThread.Name = this.GetType().FullName + " Thread";

            this.StartTime = DateTime.Now;

            workerThread.Start();
        }

        public virtual void Start(ApartmentState apartmentState)
        {
            workerThread = new Thread(WorkerThreadProc);

            workerThread.SetApartmentState(apartmentState);
            workerThread.Priority = threadPriority;
            this.StartTime = DateTime.Now;

            workerThread.Start();
        }

        public void WorkerThreadProc()
        {
            var running = true;

            isRunning = running;

            if (Started != null)
            {
                Started(this, EventArgs.Empty);
            }

            while (true)
            {
                var stopping = false;

                using (this.Lock())
                {
                    if (shutdownFlag)
                    {
                        stopping = true;
                        shutdownFlag = false;
                    }

                    running = isRunning;
                    lastProcessing = DateTime.Now;
                }

                try
                {
                    DoWork(stopping);
                }
                catch (Exception ex)
                {
                }

                using (this.Lock())
                {
                    Working = true;

                    if (stopping || processingHalted)
                    {
                        runningEvent.Set();
                        return;
                    }
                }

                Thread.Sleep(iterationSleep);
            }
        }

        public bool CheckThread(StringBuilder results)
        {
            bool processing = false;
            IDisposable _lock;
            var message = string.Format(@"{0}: Checking thread state", this.GetType().Name);

            results.AppendLine(message);

            if (this.TryLock(out _lock, (int)TimeSpan.FromSeconds(iterationTimeOut.TotalSeconds).TotalMilliseconds))
            {
                if (lastProcessing == DateTime.MinValue)
                {
                    if (firstCheck == DateTime.MinValue)
                    {
                        results.AppendLine("First time check of thread. Not yet started.");

                        firstCheck = DateTime.Now;
                        processing = true;
                    }
                    else if (DateTime.Now - firstCheck > startTimeOut)
                    {
                        var error = string.Format(@"{0}: Thread appears to have not started within {1} seconds of initiation", this.GetType().Name, startTimeOut.TotalSeconds);

                        results.AppendLine(error);

                        workerThread.Abort();
                    }
                }
                else
                {
                    if (DateTime.Now - lastProcessing > iterationTimeOut)
                    {
                        var error = string.Format(@"{0}: Lost communication with thread.  Has not processed in {1} seconds", this.GetType().Name, iterationTimeOut.TotalSeconds);

                        results.AppendLine(error);
                        workerThread.Abort();
                    }
                    else
                    {
                        results.AppendLine("Thread running");
                        processing = true;
                    }
                }

                _lock.Dispose();
            }
            else
            {
                var error = string.Format(@"{0}: Lost communication with thread.  Thread locked out.", this.GetType().Name);

                results.AppendLine(error);
                workerThread.Abort();
            }

            if (results.ToString().EndsWith("\r\n"))
            {
                results.Remove(results.Length - 2, 2);
            }

            return processing;
        }

        public void Abort()
        {
            workerThread.Abort();

            using (this.Lock())
            {
                isRunning = false;
            }

            runningEvent.Set();
        }

        public virtual void ForceStop()
        {
            try
            {
                this.workerThread.Abort();
            }
            /// <summary>   An enum constant representing the catch option. </summary>
            catch
            {
            }

            runningEvent.Set();
        }

        public virtual void Wait(int millisecondsTimeout = Timeout.Infinite)
        {
            if (!runningEvent.WaitOne(millisecondsTimeout))
            {
                throw new Exception(string.Format(@"{0}: Service wait timeout", this.GetType().Name));
            }
        }

        protected virtual void HaltProcessing()
        {
            if (workerThread == null)
            {
                return;
            }

            using (this.Lock())
            {
                processingHalted = true;
            }
        }

        public virtual void Stop(int waitSeconds)
        {
            var startTick = DateTime.Now.Ticks;
            var running = false;

            if (workerThread == null)
            {
                return;
            }

            using (this.Lock())
            {
                shutdownFlag = true;
                running = isRunning;
            }

            if (running)
            {
                using (this.Lock())
                {
                    isRunning = false;
                }

                if (!runningEvent.WaitOne(TimeSpan.FromSeconds(waitSeconds)))
                {
                    this.ForceStop();

                    throw new Exception(string.Format(@"{0}: Unable to stop service", this.GetType().Name));
                }
            }

            workerThread = null;
        }

        public virtual void Stop()
        {
            var startTick = DateTime.Now.Ticks;
            var running = false;

            if (workerThread == null)
            {
                return;
            }

            using (this.Lock()) 
            {
                shutdownFlag = true;
                running = isRunning;
            }

            if (running)
            {
                using (this.Lock())
                {
                    isRunning = false;
                }

                if (!runningEvent.WaitOne(TimeSpan.FromSeconds(15)))
                {
                    this.ForceStop();

                    throw new ThreadInterruptedException(string.Format(@"{0}: Unable to stop service", this.GetType().Name));
                }
            }

            workerThread = null;
        }
    }
}
