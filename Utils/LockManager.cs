using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils
{
#if !UTILS_INTERNAL
    public static class LockManager
#else
    internal static class LockManager
#endif
    {
        internal static Dictionary<ManagedLockObject, string> LockObjects { get; private set; }
        public static event EventHandler<EventArgs<Exception>> OnLockThreadException;

        static LockManager()
        {
            LockObjects = new Dictionary<ManagedLockObject, string>();
        }

        public static IManagedLockObject CreateObject()
        {
            var lockObject = new ManagedLockObject();

            lockObject.OnThreadException += (sender, e) =>
            {
                OnLockThreadException(lockObject, e);
            };

            lock (LockObjects)
            {
                LockObjects.Add(lockObject, lockObject.GetStackText(10, 3));
            }

            return lockObject;
        }

        public static IManagedMutexObject CreateMutex(string name)
        {
            var mutextObject = new ManagedMutexObject(name);

            return mutextObject;
        }

        public static IManagedSemaphoreObject CreateSemaphore(int initialCount = 1, int maximumCount = 1)
        {
            var semaphoreLockObject = new ManagedSemaphoreObject(initialCount, maximumCount);

            return semaphoreLockObject;
        }
    }
}
