using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.InProcessTransactions
{
    public interface IStateManager : ILockable
    {
        ITransaction CreateTransaction(string name, int expectedEnlistmentCount = -1);
        ITransaction CreateTransaction(Guid id, int expectedEnlistmentCount = -1);
        ITransaction Enlist(string name, IEnlistment enlistment);
        ITransaction Enlist(Guid id, IEnlistment enlistment);
    }
}
