using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Utils
{
    internal class ManagedLock : IDisposable
    {
        internal event EventHandler<EventArgs<ManagedLock>> Disposed;
        private ManagedLockObject lockObject;
        internal string LockStack { get; private set; }
        internal DateTime LockTime { get; private set; }

        internal ManagedLock(ManagedLockObject lockObject)
        {
            this.LockStack = this.GetStackText(10, 3);
            this.lockObject = lockObject;
        }

        internal void Lock()
        {
            string lastLockThread = null;
            var lastLockStack = lockObject.LastLockStack;

            if (lockObject.LockingThread != null)
            {
                var lastThread = lockObject.LockingThread;

                if (lastThread != null)
                {
                    lastLockThread = lastThread.Name;
                }
            }

            this.LockTime = DateTime.Now;
            
            if (!Monitor.TryEnter(lockObject))
            {
                Monitor.Enter(lockObject);
            }

            lockObject.LockedBy = this;
            lockObject.LockingThread = Thread.CurrentThread;
            lockObject.IsLocked = true;
        }

        internal bool TryLock(int millisecondsTimeout)
        {
            bool locked;

            this.LockTime = DateTime.Now;

            locked = Monitor.TryEnter(lockObject, millisecondsTimeout);

            if (locked)
            {
                lockObject.LockedBy = this;
                lockObject.LockingThread = Thread.CurrentThread;
                lockObject.IsLocked = true;
            }

            return locked;
        }

        internal bool TryLock()
        {
            bool locked;

            this.LockTime = DateTime.Now;

            locked = Monitor.TryEnter(lockObject);

            if (locked)
            {
                lockObject.LockedBy = this;
                lockObject.LockingThread = Thread.CurrentThread;
                lockObject.IsLocked = true;
            }

            return locked;
        }

        public void Dispose()
        {
            Monitor.Exit(lockObject);

            lockObject.LockedBy = null;
            lockObject.LockingThread = null;
            lockObject.IsLocked = false;

            Disposed(lockObject, new EventArgs<ManagedLock>(this));
        }
    }
}
