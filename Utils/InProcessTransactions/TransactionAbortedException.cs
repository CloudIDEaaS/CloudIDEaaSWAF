using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils.InProcessTransactions.Implementation;

namespace Utils.InProcessTransactions
{
    public class TransactionAbortedException : OperationCanceledException
    {
        private Dictionary<string, object> data;

        public TransactionAbortedException(ITransaction transaction, Dictionary<string, object> data) : base("Transaction aborted.")
        {
            this.data = data;

            if (transaction is Transaction primaryTransaction)
            {
                data.Add("Enlistments", primaryTransaction.Enlistments);
            }
        }

        public override IDictionary Data
        {
            get
            {
                return data;
            }
        }
    }
}
