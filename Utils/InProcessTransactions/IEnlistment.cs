using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.InProcessTransactions
{
    public interface IEnlistment
    {
        ITransaction Transaction { get; set; }
        TransactionAction Action { get; set; }
        void Rollback();
        void FinalCommit();
    }
}
