using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.InProcessTransactions
{
    public class SimpleEnlistment : IEnlistment
    {
        public TransactionAction Action { get; set; }
        public Action HandleFinalCommit { get; set; }
        public Action HandleRollback { get; set; }
        public ITransaction Transaction { get; set; }

        public SimpleEnlistment()
        {
        }

        public SimpleEnlistment(Action finalCommit, Action rollback)
        {
            this.HandleFinalCommit = finalCommit;
            this.HandleRollback = rollback;
        }

        public void FinalCommit()
        {
            this.HandleFinalCommit();
        }

        public void Rollback()
        {
            this.HandleRollback();
        }
    }
}
