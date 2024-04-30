using System;

namespace Utils
{
#if !UTILS_INTERNAL
    public interface IManagedLockObject
#else
    internal interface IManagedLockObject
#endif
    {
        string LastLockStack { get; }
        event EventHandler<EventArgs<Exception>> OnThreadException;
        bool ContainsAction(Delegate action);
        void DoOnceAquired(Action action);
        void DoOnceAquired(Action<bool> action, TimeSpan timeout);
        IDisposable Lock();
        bool TryLock(out IDisposable managedLock);
        bool TryLock(out IDisposable managedLock, int millisecondsTimeout);
    }
}
