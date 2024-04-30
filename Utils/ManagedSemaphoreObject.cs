using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Utils
{
#if !UTILS_INTERNAL
    public class ManagedSemaphoreObject : IManagedSemaphoreObject
#else
    internal class ManagedSemaphoreObject : IManagedSemaphoreObject
#endif
    {
        private bool isRunning;
        private ManualResetEvent runningEvent;
        private SemaphoreSlim internalSemaphore;
        internal int InitialCount { get; }
        internal int MaximumCount { get; }
        private Thread aquisitionThread;
        private Thread lockingThread;
        private List<ManagedSemaphore> locks;
        private bool isLocked;
        private ManagedSemaphore lockedBy;
        public event EventHandler<EventArgs<Exception>> OnThreadException;
        internal List<ManagedLockDoOnceAction> DoOnceActions { get; private set; }

        internal ManagedSemaphoreObject(int initialCount, int maximumCount)
        {
            internalSemaphore = new SemaphoreSlim(initialCount, maximumCount);

            this.InitialCount = initialCount;
            this.MaximumCount = maximumCount;

            this.Locks = new List<ManagedSemaphore>();
            this.DoOnceActions = new List<ManagedLockDoOnceAction>();
        }

        ~ManagedSemaphoreObject()
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

        internal List<ManagedSemaphore> Locks
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

        internal void RemoveLock(ManagedSemaphore _lock)
        {
            LockSet(() => locks.Remove(_lock));
        }

        internal void AddLock(ManagedSemaphore _lock)
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

        internal ManagedSemaphore LockedBy
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

            internalSemaphore.Wait();
            
            returnVal = func();
            
            internalSemaphore.Release();

            return returnVal;
        }

        private void LockSet(Action action)
        {
            internalSemaphore.Wait();
            
            action();

            internalSemaphore.Release();
        }

        public bool ContainsAction(Delegate action)
        {
            bool contains = false;

            internalSemaphore.Wait();
            
            contains = this.DoOnceActions.Any(a => a.Action == action);

            internalSemaphore.Release();

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
            IDisposable ManagedSemaphore;

            if (this.TryLock(out ManagedSemaphore))
            {
                var doOnceAction = new ManagedLockDoOnceAction { Action = action, Lock = ManagedSemaphore, TimeOut = timeout, TimeStarted = DateTime.Now };

                internalSemaphore.Wait();
                
                this.DoOnceActions.Add(doOnceAction);

                internalSemaphore.Release();

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
                    ManagedSemaphore.Dispose();
                }
            }
            else
            {
                var doOnceAction = new ManagedLockDoOnceAction { Action = action, TimeOut = timeout, TimeStarted = DateTime.Now };

                internalSemaphore.Wait();
                
                this.DoOnceActions.Add(doOnceAction);

                internalSemaphore.Release();

                if (aquisitionThread == null)
                {
                    aquisitionThread = new Thread(AquisitionThreadProc);
                    aquisitionThread.Name = string.Format("ManagedSemaphoreAquisitionThread:{0}", Guid.NewGuid().ToString());

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
                internalSemaphore.Wait();
                
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
                        IDisposable managedSemaphore;

                        if (this.TryLock(out managedSemaphore))
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
                                managedSemaphore.Dispose();
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

                internalSemaphore.Release();

                Thread.Sleep(100);
            }

            runningEvent.Set();
        }

        internal void Stop()
        {
            if (aquisitionThread != null)
            {
                var running = false;

                internalSemaphore.Wait();
                
                running = isRunning;

                internalSemaphore.Release();

                if (running)
                {
                    internalSemaphore.Wait();
                    
                    isRunning = false;

                    internalSemaphore.Release();

                    if (!runningEvent.WaitOne(TimeSpan.FromSeconds(5)))
                    {
                        OnThreadException(this, new EventArgs<Exception>(new Exception(string.Format(@"{0}: Unable to stop AquisitionThread", this.GetType().Name))));
                    }
                }
            }
        }

        public bool TryLock(out IDisposable managedSemaphore)
        {
            var managedTryLock = new ManagedSemaphore(this);

            managedSemaphore = null;

            managedTryLock.Disposed += (sender, e) =>
            {
                internalSemaphore.Wait();
                
                this.Locks.Remove(e.Value);

                internalSemaphore.Release();
            };

            var locked = managedTryLock.TryLock();

            if (locked)
            {
                managedSemaphore = managedTryLock;

                internalSemaphore.Wait();
                
                this.Locks.Add(managedTryLock);

                internalSemaphore.Release();
            }

            return locked;
        }

        public bool TryLock(out IDisposable managedSemaphore, int millisecondsTimeout)
        {
            var managedTryLock = new ManagedSemaphore(this);

            managedSemaphore = null;

            managedTryLock.Disposed += (sender, e) =>
            {
                internalSemaphore.Wait();
                
                this.Locks.Remove(e.Value);

                internalSemaphore.Release();
            };

            var locked = managedTryLock.TryLock(millisecondsTimeout);

            if (locked)
            {
                managedSemaphore = managedTryLock;

                internalSemaphore.Wait();
                
                this.Locks.Add(managedTryLock);

                internalSemaphore.Release();
            }

            return locked;
        }

        public IDisposable Lock()
        {
            var managedSemaphore = new ManagedSemaphore(this);

            managedSemaphore.Disposed += (sender, e) =>
            {
                this.RemoveLock(e.Value);
            };

            this.AddLock(managedSemaphore);

            managedSemaphore.Lock();

            return managedSemaphore;
        }
    }
}
