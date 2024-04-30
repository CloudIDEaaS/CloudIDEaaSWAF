using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Utils
{
#if !UTILS_INTERNAL
    public class ManagedMutexObject : IManagedMutexObject
#else
    internal class ManagedMutexObject : IManagedMutexObject
#endif
    {
        private bool isRunning;
        private ManualResetEvent runningEvent;
        private Mutex internalMutex;
        private Thread aquisitionThread;
        private Thread lockingThread;
        private List<ManagedMutex> locks;
        private bool isLocked;
        private ManagedMutex lockedBy;
        public event EventHandler<EventArgs<Exception>> OnThreadException;
        internal List<ManagedLockDoOnceAction> DoOnceActions { get; private set; }

        internal ManagedMutexObject(string name)
        {
            internalMutex = new Mutex(false, name);

            this.Locks = new List<ManagedMutex>();
            this.DoOnceActions = new List<ManagedLockDoOnceAction>();
        }

        ~ManagedMutexObject()
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

        internal List<ManagedMutex> Locks
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

        internal void RemoveLock(ManagedMutex _lock)
        {
            LockSet(() => locks.Remove(_lock));
        }

        internal void AddLock(ManagedMutex _lock)
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

        internal ManagedMutex LockedBy
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

            internalMutex.WaitOne();
            
            returnVal = func();

            internalMutex.ReleaseMutex();

            return returnVal;
        }

        private void LockSet(Action action)
        {
            internalMutex.WaitOne();
            
            action();

            internalMutex.ReleaseMutex();
        }

        public bool ContainsAction(Delegate action)
        {
            bool contains = false;

            internalMutex.WaitOne();
            
            contains = this.DoOnceActions.Any(a => a.Action == action);

            internalMutex.ReleaseMutex();

            return contains;
        }

        public void DoOnceAquired(Action action)
        {
            DoOnceAquired(action, TimeSpan.Zero);
        }

        public void DoOnceAquired(Action<bool> action, TimeSpan timeout)
        {
            DoOnceAquired((Delegate)action, timeout);
        }

        private void DoOnceAquired(Delegate action, TimeSpan timeout)
        {
            IDisposable ManagedMutex;

            if (this.TryLock(out ManagedMutex))
            {
                var doOnceAction = new ManagedLockDoOnceAction { Action = action, Lock = ManagedMutex, TimeOut = timeout, TimeStarted = DateTime.Now };

                internalMutex.WaitOne();
                
                this.DoOnceActions.Add(doOnceAction);

                internalMutex.ReleaseMutex();

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
                    ManagedMutex.Dispose();
                }
            }
            else
            {
                var doOnceAction = new ManagedLockDoOnceAction { Action = action, TimeOut = timeout, TimeStarted = DateTime.Now };

                internalMutex.WaitOne();
                
                this.DoOnceActions.Add(doOnceAction);

                internalMutex.ReleaseMutex();

                if (aquisitionThread == null)
                {
                    aquisitionThread = new Thread(AquisitionThreadProc);
                    aquisitionThread.Name = string.Format("ManagedMutexAquisitionThread:{0}", Guid.NewGuid().ToString());

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
                internalMutex.WaitOne();
                
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
                        IDisposable managedMutex;

                        if (this.TryLock(out managedMutex))
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
                                managedMutex.Dispose();
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

                internalMutex.ReleaseMutex();

                Thread.Sleep(100);
            }

            runningEvent.Set();
        }

        internal void Stop()
        {
            if (aquisitionThread != null)
            {
                var running = false;

                internalMutex.WaitOne();
                
                running = isRunning;

                internalMutex.ReleaseMutex();

                if (running)
                {
                    internalMutex.WaitOne();
                    
                    isRunning = false;

                    internalMutex.ReleaseMutex();

                    if (!runningEvent.WaitOne(TimeSpan.FromSeconds(5)))
                    {
                        OnThreadException(this, new EventArgs<Exception>(new Exception(string.Format(@"{0}: Unable to stop AquisitionThread", this.GetType().Name))));
                    }
                }
            }
        }

        public bool TryLock(out IDisposable managedMutex)
        {
            var managedTryLock = new ManagedMutex(this);

            managedMutex = null;

            managedTryLock.Disposed += (sender, e) =>
            {
                internalMutex.WaitOne();
                
                this.Locks.Remove(e.Value);

                internalMutex.ReleaseMutex();
            };

            var locked = managedTryLock.TryLock();

            if (locked)
            {
                managedMutex = managedTryLock;

                internalMutex.WaitOne();
                
                this.Locks.Add(managedTryLock);

                internalMutex.ReleaseMutex();
            }

            return locked;
        }

        public bool TryLock(out IDisposable managedMutex, int millisecondsTimeout)
        {
            var managedTryLock = new ManagedMutex(this);

            managedMutex = null;

            managedTryLock.Disposed += (sender, e) =>
            {
                internalMutex.WaitOne();
                
                this.Locks.Remove(e.Value);

                internalMutex.ReleaseMutex();
            };

            var locked = managedTryLock.TryLock(millisecondsTimeout);

            if (locked)
            {
                managedMutex = managedTryLock;

                internalMutex.WaitOne();
                
                this.Locks.Add(managedTryLock);

                internalMutex.ReleaseMutex();
            }

            return locked;
        }

        public IDisposable Lock()
        {
            var managedMutex = new ManagedMutex(this);

            managedMutex.Disposed += (sender, e) =>
            {
                this.RemoveLock(e.Value);
            };

            this.AddLock(managedMutex);

            managedMutex.Lock();

            return managedMutex;
        }

        internal bool Wait(int millisecondsTimeout = 0)
        {
            if (millisecondsTimeout < 0)
            {
                return internalMutex.WaitOne();
            }
            else
            {
                return internalMutex.WaitOne(millisecondsTimeout);
            }
        }

        internal void Release()
        {
            internalMutex.ReleaseMutex();
        }
    }
}
