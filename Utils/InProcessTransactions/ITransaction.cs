using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.InProcessTransactions
{
    public enum TransactionAction
    {
        None,
        Committed,
        Rolledback
    }

    public enum NotifyTransactionChangedAction
    {
        Commit = 0,
        Abort = 1
    }

    public class NotifyTransactionChangedEventArgs : EventArgs
    {
        public NotifyTransactionChangedAction Action { get; }
        public ITransaction Transaction { get; }

        public NotifyTransactionChangedEventArgs(ITransaction transaction, NotifyTransactionChangedAction action)
        {
            this.Transaction = transaction;
            this.Action = action;
        }
    }

    public interface ITransaction : ILockable
    {
        TransactionAction Action { get; }
        List<IEnlistment> UncommittedEnlistments { get; }
        event EventHandler<NotifyTransactionChangedEventArgs> TransactionChanged;
        long CommitSequenceNumber { get; }
        long TransactionId { get; }
        void Abort();
        void Abort(Exception exception);
        Task CommitAsync();
    }
}
