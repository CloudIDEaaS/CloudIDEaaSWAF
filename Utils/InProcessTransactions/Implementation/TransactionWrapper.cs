using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.InProcessTransactions.Implementation
{
    public class TransactionWrapper : ITransaction
    {
        private Transaction primaryTransaction;
        internal IEnlistment Enlistment { get; }
        internal Guid Identifier { get; }
        public DateTime EnlistmentTime;
        public IEnumerable<StackFrame> EnlistmentCallStack { get; }
        public IEnumerable<StackFrame> AbortCallStack { get; private set; }
        public Exception Exception { get; private set; }
        private long commitSequenceNumber = -1;

        public TransactionWrapper(Transaction primaryTransaction, IEnlistment enlistment)
        {
            this.primaryTransaction = primaryTransaction;
            this.Enlistment = enlistment;
            this.Identifier = Guid.NewGuid();
            this.Enlistment.Transaction = this;
            this.EnlistmentTime = DateTime.Now;
            this.EnlistmentCallStack = this.GetStack(10);
        }

        public TransactionAction Action
        {
            get
            {
                return primaryTransaction.Action;
            }
        }

        public long CommitSequenceNumber
        {
            get
            {
                return primaryTransaction.CommitSequenceNumber;
            }
        }

        public long TransactionId
        {
            get
            {
                return primaryTransaction.TransactionId;
            }

        }

        public List<IEnlistment> UncommittedEnlistments => throw new NotImplementedException();

        public event EventHandler<NotifyTransactionChangedEventArgs> TransactionChanged
        { 
            add
            {
                primaryTransaction.TransactionChanged += value;
            }

            remove
            {
                primaryTransaction.TransactionChanged -= value;
            }
        }

        public void Abort(Exception exception)
        {
            this.AbortCallStack = this.GetStack(10);
            this.Exception = exception;

            primaryTransaction.InternalAbort(this);
        }

        public void Abort()
        {
            this.AbortCallStack = this.GetStack(10);

            primaryTransaction.InternalAbort(this);

            ((ILockable)this).LockSet(() => this.Enlistment.Action = TransactionAction.Rolledback);
        }

        public async Task CommitAsync()
        {
            ((ILockable)this).LockSet(() =>
            {
                this.Enlistment.Action = TransactionAction.Committed;
                commitSequenceNumber = primaryTransaction.GetSequenceNumber();
            });

            await primaryTransaction.InternalCommit(this);
        }

        IDisposable ILockable.Lock()
        {
            return ((ILockable)primaryTransaction).Lock();
        }

        T ILockable.LockReturn<T>(Func<T> func)
        {
            return ((ILockable)primaryTransaction).LockReturn(func);
        }

        void ILockable.LockSet(Action action)
        {
            ((ILockable)primaryTransaction).LockSet(action);
        }
    }
}
