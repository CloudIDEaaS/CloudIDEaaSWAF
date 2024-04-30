using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.InProcessTransactions
{
    public class CountdownEnlistment : SimpleEnlistment, ILockable
    {
        private IManagedLockObject lockObject;
        int countdown;

        public CountdownEnlistment(int countdown)
        {
            this.countdown = countdown;
            lockObject = LockManager.CreateObject();
        }

        public int Countdown
        {
            get
            {
                return ((ILockable)this).LockReturn(() => countdown);
            }

            private set
            {
                ((ILockable)this).LockSet(() =>
                {
                    countdown = value;
                });
            }
        }

        public void Reduce()
        {
            Debug.Assert(this.Countdown != 0);

            this.Countdown -= 1;
        }

        IDisposable ILockable.Lock()
        {
            return lockObject.Lock();
        }

        T ILockable.LockReturn<T>(Func<T> func)
        {
            T returnVal;

            using (((ILockable)this).Lock())
            {
                returnVal = func();
            }

            return returnVal;
        }

        void ILockable.LockSet(Action action)
        {
            using (((ILockable)this).Lock())
            {
                action();
            }
        }
    }
}
