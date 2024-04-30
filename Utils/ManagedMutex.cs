using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Utils
{
    internal class ManagedMutex : IDisposable
    {
        internal event EventHandler<EventArgs<ManagedMutex>> Disposed;
        private ManagedMutexObject mutexObject;
        internal string LockStack { get; private set; }
        internal DateTime LockTime { get; private set; }

        internal ManagedMutex(ManagedMutexObject mutexObject)
        {
            this.LockStack = this.GetStackText(10, 3);
            
            this.mutexObject = mutexObject;
        }

        internal void Lock()
        {
            string lastLockThread = null;
            var lastLockStack = mutexObject.LastLockStack;

            if (mutexObject.LockingThread != null)
            {
                var lastThread = mutexObject.LockingThread;

                if (lastThread != null)
                {
                    lastLockThread = lastThread.Name;
                }
            }

            this.LockTime = DateTime.Now;

            mutexObject.Wait();

            mutexObject.LockedBy = this;
            mutexObject.LockingThread = Thread.CurrentThread;
            mutexObject.IsLocked = true;
        }

        internal bool TryLock(int millisecondsTimeout)
        {
            bool locked;

            this.LockTime = DateTime.Now;

            locked = mutexObject.Wait(millisecondsTimeout);

            if (locked)
            {
                mutexObject.LockedBy = this;
                mutexObject.LockingThread = Thread.CurrentThread;
                mutexObject.IsLocked = true;
            }

            return locked;
        }

        internal bool TryLock()
        {
            bool locked;

            this.LockTime = DateTime.Now;

            locked = mutexObject.Wait(1);

            if (locked)
            {
                mutexObject.LockedBy = this;
                mutexObject.LockingThread = Thread.CurrentThread;
                mutexObject.IsLocked = true;
            }

            return locked;
        }

        public void Dispose()
        {
            mutexObject.Release();

            mutexObject.LockedBy = null;
            mutexObject.LockingThread = null;
            mutexObject.IsLocked = false;

            Disposed(mutexObject, new EventArgs<ManagedMutex>(this));
        }
    }
}
