using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public interface ILockable
    {
        IDisposable Lock();
        T LockReturn<T>(Func<T> func);
        void LockSet(Action action);
    }
}
