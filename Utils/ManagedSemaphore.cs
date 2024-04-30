using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Utils
{
    internal class ManagedSemaphore : IDisposable
    {
        internal event EventHandler<EventArgs<ManagedSemaphore>> Disposed;
        private ManagedSemaphoreObject semaphoreObject;
        private SemaphoreSlim internalSemaphore;
        internal string LockStack { get; private set; }
        internal DateTime LockTime { get; private set; }

        internal ManagedSemaphore(ManagedSemaphoreObject semaphoreObject)
        {
            this.LockStack = this.GetStackText(10, 3);
            
            this.semaphoreObject = semaphoreObject;
            internalSemaphore = new SemaphoreSlim(semaphoreObject.InitialCount, semaphoreObject.MaximumCount);
        }

        internal void Lock()
        {
            string lastLockThread = null;
            var lastLockStack = semaphoreObject.LastLockStack;

            if (semaphoreObject.LockingThread != null)
            {
                var lastThread = semaphoreObject.LockingThread;

                if (lastThread != null)
                {
                    lastLockThread = lastThread.Name;
                }
            }

            this.LockTime = DateTime.Now;

            internalSemaphore.Wait();

            semaphoreObject.LockedBy = this;
            semaphoreObject.LockingThread = Thread.CurrentThread;
            semaphoreObject.IsLocked = true;
        }

        internal bool TryLock(int millisecondsTimeout)
        {
            bool locked;

            this.LockTime = DateTime.Now;

            locked = internalSemaphore.Wait(millisecondsTimeout);

            if (locked)
            {
                semaphoreObject.LockedBy = this;
                semaphoreObject.LockingThread = Thread.CurrentThread;
                semaphoreObject.IsLocked = true;
            }

            return locked;
        }

        internal bool TryLock()
        {
            bool locked;

            this.LockTime = DateTime.Now;

            locked = internalSemaphore.Wait(1);

            if (locked)
            {
                semaphoreObject.LockedBy = this;
                semaphoreObject.LockingThread = Thread.CurrentThread;
                semaphoreObject.IsLocked = true;
            }

            return locked;
        }

        public void Dispose()
        {
            internalSemaphore.Release();

            semaphoreObject.LockedBy = null;
            semaphoreObject.LockingThread = null;
            semaphoreObject.IsLocked = false;

            Disposed(semaphoreObject, new EventArgs<ManagedSemaphore>(this));
        }
    }
}
