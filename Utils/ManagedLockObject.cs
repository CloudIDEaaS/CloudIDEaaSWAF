using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Utils
{
    internal class ManagedLockObject : IManagedLockObject
    {
        private bool isRunning;
        private ManualResetEvent runningEvent;
        private object internalLockObject;
        private Thread aquisitionThread;
        private Thread lockingThread;
        private List<ManagedLock> locks;
        private bool isLocked;
        private ManagedLock lockedBy;
        public event EventHandler<EventArgs<Exception>> OnThreadException;
        internal List<ManagedLockDoOnceAction> DoOnceActions { get; private set; }

        internal ManagedLockObject()
        {
            internalLockObject = new object();

            this.Locks = new List<ManagedLock>();
            this.DoOnceActions = new List<ManagedLockDoOnceAction>();
        }

        ~ManagedLockObject()
        {
            this.Stop();
        }

        public string LastLockStack
        {
            get
            {
                if (this.IsLocked)
                {
                    return LockReturn(() => lockedBy != null ? lockedBy.LockStack : null);
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public Thread LockingThread 
        {
            get
            {
                return LockReturn(() => lockingThread);
            }

            internal set
            {
                LockSet(() => lockingThread = value);
            }
        }

        internal List<ManagedLock> Locks 
        {
            get
            {
                return LockReturn(() => locks.ToList());
            }

            private set
            {
                LockSet(() => locks = value);
            }
        }

        internal void RemoveLock(ManagedLock _lock)
        {
            LockSet(() => locks.Remove(_lock));
        }

        internal void AddLock(ManagedLock _lock)
        {
            LockSet(() => locks.Add(_lock));
        }

        internal bool IsLocked 
        {
            get
            {
                return LockReturn(() => isLocked);
            }

            set
            {
                LockSet(() => isLocked = value);
            }
        }

        internal ManagedLock LockedBy 
        { 
            get
            {
                return LockReturn(() => lockedBy);
            }

            set
            {
                LockSet(() => lockedBy = value);
            }
        }

        private T LockReturn<T>(Func<T> func)
        {
            T returnVal;

            lock (internalLockObject)
            {
                returnVal = func();
            }

            return returnVal;
        }

        private void LockSet(Action action)
        {
            lock (internalLockObject)
            {
                action();
            }
        }

        public bool ContainsAction(Delegate action)
        {
            bool contains = false;

            lock (internalLockObject)
            {
                contains = this.DoOnceActions.Any(a => a.Action == action);
            }

            return contains;
        }

        public void DoOnceAquired(Action action)
        {
            DoOnceAquired(action, TimeSpan.Zero);
        }
        
        public void DoOnceAquired(Action<bool> action, TimeSpan timeout)
        {
            DoOnceAquired((Delegate) action, timeout);
        }

        private void DoOnceAquired(Delegate action, TimeSpan timeout)
        {
            IDisposable managedLock;

            if (this.TryLock(out managedLock))
            {
                var doOnceAction = new ManagedLockDoOnceAction { Action = action, Lock = managedLock, TimeOut = timeout, TimeStarted = DateTime.Now };

                lock (internalLockObject)
                {
                    this.DoOnceActions.Add(doOnceAction);
                }

                try
                {
                    if (timeout != TimeSpan.Zero)
                    {
                        action.DynamicInvoke(false);
                    }
                    else
                    {
                        action.DynamicInvoke();
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    this.DoOnceActions.Remove(doOnceAction);
                    managedLock.Dispose();
                }
            }
            else
            {
                var doOnceAction = new ManagedLockDoOnceAction { Action = action, TimeOut = timeout, TimeStarted = DateTime.Now };

                lock (internalLockObject)
                {
                    this.DoOnceActions.Add(doOnceAction);
                }

                if (aquisitionThread == null)
                {
                    aquisitionThread = new Thread(AquisitionThreadProc);
                    aquisitionThread.Name = string.Format("ManagedLockAquisitionThread:{0}", Guid.NewGuid().ToString());

                    runningEvent = new ManualResetEvent(false);

                    aquisitionThread.Start();
                }
            }
        }

        private void AquisitionThreadProc()
        {
            var running = true;

            isRunning = running;

            while (running)
            {
                lock (internalLockObject)
                {
                    running = isRunning;

                    foreach (var action in this.DoOnceActions.ToList())
                    {
                        if (action.TimeOut != TimeSpan.Zero && DateTime.Now - action.TimeStarted > action.TimeOut)
                        {
                            action.Action.DynamicInvoke(true);

                            this.DoOnceActions.Remove(action);
                        }
                        else 
                        {
                            IDisposable managedLock;

                            if (this.TryLock(out managedLock))
                            {
                                try
                                {
                                    if (action.TimeOut != TimeSpan.Zero)
                                    {
                                        action.Action.DynamicInvoke(false);
                                    }
                                    else
                                    {
                                        action.Action.DynamicInvoke();
                                    }
                                }
                                catch (Exception ex)
                                {
                                    OnThreadException(action, new EventArgs<Exception>(ex));
                                }
                                finally
                                {
                                    managedLock.Dispose();
                                    this.DoOnceActions.Remove(action);
                                }
                            }
                        }
                    }

                    if (this.DoOnceActions.Count == 0)
                    {
                        aquisitionThread = null;
                        break;
                    }
                }

                Thread.Sleep(100);
            }

            runningEvent.Set();
        }

        internal void Stop()
        {
            if (aquisitionThread != null)
            {
                var running = false;

                lock (internalLockObject)
                {
                    running = isRunning;
                }

                if (running)
                {
                    lock (internalLockObject)
                    {
                        isRunning = false;
                    }

                    if (!runningEvent.WaitOne(TimeSpan.FromSeconds(5)))
                    {
                        OnThreadException(this, new EventArgs<Exception>(new Exception(string.Format(@"{0}: Unable to stop AquisitionThread", this.GetType().Name))));
                    }
                }
            }
        }

        public bool TryLock(out IDisposable managedLock)
        {
            var managedTryLock = new ManagedLock(this);

            managedLock = null;

            managedTryLock.Disposed += (sender, e) =>
            {
                lock (internalLockObject)
                {
                    this.Locks.Remove(e.Value);
                }
            };

            var locked = managedTryLock.TryLock();

            if (locked)
            {
                managedLock = managedTryLock;

                lock (internalLockObject)
                {
                    this.Locks.Add(managedTryLock);
                }
            }

            return locked;
        }

        public bool TryLock(out IDisposable managedLock, int millisecondsTimeout)
        {
            var managedTryLock = new ManagedLock(this);

            managedLock = null;

            managedTryLock.Disposed += (sender, e) =>
            {
                lock (internalLockObject)
                {
                    this.Locks.Remove(e.Value);
                }
            };

            var locked = managedTryLock.TryLock(millisecondsTimeout);

            if (locked)
            {
                managedLock = managedTryLock;

                lock (internalLockObject)
                {
                    this.Locks.Add(managedTryLock);
                }
            }

            return locked;
        }

        public IDisposable Lock()
        {
            var managedLock = new ManagedLock(this);

            managedLock.Disposed += (sender, e) =>
            {
                this.RemoveLock(e.Value);
            };

            this.AddLock(managedLock);

            managedLock.Lock();

            return managedLock;
        }
    }
}
