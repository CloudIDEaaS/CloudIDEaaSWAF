using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.InProcessTransactions.Implementation
{
    public class Transaction : ITransaction
    {
        private long commitSequenceNumber = -1;
        private static long commitSequenceNumberStatic = -1;
        private StateManager stateManager;
        private long transactionId;
        private IManagedLockObject lockObject;
        private IManagedSemaphoreObject actionSemaphoreObject;
        private static long staticTransactionId;
        private TransactionAction action;

        internal Dictionary<Guid, IEnlistment> Enlistments { get; }
        internal int ExpectedEnlistmentCount { get; }
        public IEnumerable<StackFrame> AbortCallStack { get; private set; }
        public Exception Exception { get; private set; }
        public event EventHandler<NotifyTransactionChangedEventArgs> TransactionChanged;

        public Transaction(StateManager stateManager, int expectedEnlistmentCount = -1)
        {
            lockObject = LockManager.CreateObject();
            actionSemaphoreObject = LockManager.CreateSemaphore();

            this.stateManager = stateManager;
            this.transactionId = ++staticTransactionId;
            this.ExpectedEnlistmentCount = expectedEnlistmentCount;

            Enlistments = new Dictionary<Guid, IEnlistment>();
        }

        public TransactionAction Action
        {
            get
            {
                return this.action;
            }
        }

        public long CommitSequenceNumber
        {
            get
            {
                return commitSequenceNumber;
            }
        }

        public long TransactionId
        {
            get
            {
                return transactionId;
            }
        }

        public List<IEnlistment> UncommittedEnlistments
        {
            get
            {
                return this.Enlistments.Values.Where(e => e.Action != TransactionAction.Committed).ToList();
            }
        }

        internal void RaiseCommit()
        {
            TransactionChanged?.Invoke(this, new NotifyTransactionChangedEventArgs(this, NotifyTransactionChangedAction.Commit));
            action = TransactionAction.Committed;
        }

        internal void RaiseAbort()
        {
            TransactionChanged?.Invoke(this, new NotifyTransactionChangedEventArgs(this, NotifyTransactionChangedAction.Abort));
            action = TransactionAction.Rolledback;
        }

        internal void RaiseAbort(Exception exception)
        {
            this.Exception = exception;

            TransactionChanged?.Invoke(this, new NotifyTransactionChangedEventArgs(this, NotifyTransactionChangedAction.Abort));
        }

        internal void Enlist(Guid id, IEnlistment enlistment)
        {
            ((ILockable)this).LockSet(() =>
            {
                using (actionSemaphoreObject.Lock())
                {
                    this.Enlistments.Add(id, enlistment);
                }
            });
        }

        internal long GetSequenceNumber()
        {
            return ++commitSequenceNumberStatic;
        }

        public void Abort(Exception exception)
        {
            this.Exception = exception;
            this.Abort();
        }

        public void Abort()
        {
            this.AbortCallStack = this.GetStack(10);

            stateManager.InitiateAbort(this);
        }

        public async Task CommitAsync()
        {
            await stateManager.InitiateCommit(this, true);
        }

        internal void InternalAbort(TransactionWrapper transactionWrapper)
        {
            using (actionSemaphoreObject.Lock())
            {
                this.AbortCallStack = transactionWrapper.AbortCallStack;
                this.Exception = transactionWrapper.Exception;

                stateManager.InitiateAbort(this);
            }
        }

        internal async Task InternalCommit(TransactionWrapper transactionWrapper)
        {
            using (actionSemaphoreObject.Lock())
            {
                await stateManager.InitiateCommit(this);

                if (transactionWrapper.Enlistment is CountdownEnlistment countdownEnlistment)
                {
                    if (countdownEnlistment.Countdown == 0)
                    {
                        transactionWrapper.Enlistment.Action = TransactionAction.Committed;
                    }
                }
                else
                {
                    transactionWrapper.Enlistment.Action = TransactionAction.Committed;
                }
            }
        }

        IDisposable ILockable.Lock()
        {
            return lockObject.Lock();
        }

        T ILockable.LockReturn<T>(Func<T> func)
        {
            T returnVal;

            using (((ILockable) this).Lock())
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
