using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.InProcessTransactions.Implementation
{
    public class StateManager : IStateManager
    {
        private IManagedLockObject lockObject;
        private Dictionary<string, ITransaction> transactions;
        private bool primaryCommitted;

        public StateManager()
        {
            lockObject = LockManager.CreateObject();
            transactions = new Dictionary<string, ITransaction>();
        }

        public ITransaction CreateTransaction(Guid id, int expectedEnlistmentCount = -1)
        {
            var transaction = new Transaction(this, expectedEnlistmentCount);

            ((ILockable)this).LockSet(() =>
            {
                transactions.Add(id.ToString(), transaction);
            });

            return transaction;
        }

        public ITransaction CreateTransaction(string name, int expectedEnlistmentCount = -1)
        {
            var transaction = new Transaction(this, expectedEnlistmentCount);

            ((ILockable)this).LockSet(() =>
            {
                transactions.Add(name, transaction);
            });

            return transaction;
        }

        public ITransaction Enlist(Guid id, IEnlistment enlistment)
        {
            TransactionWrapper transactionWrapper;

            var transaction = (Transaction)((ILockable)this).LockReturn(() =>
            {
                return this.transactions[id.ToString()];
            });

            transactionWrapper = new TransactionWrapper(transaction, enlistment);
            transaction.Enlist(transactionWrapper.Identifier, enlistment);

            return transactionWrapper;
        }

        public ITransaction Enlist(string name, IEnlistment enlistment)
        {
            TransactionWrapper transactionWrapper;

            var transaction = (Transaction) ((ILockable)this).LockReturn(() =>
            {
                return this.transactions[name];
            });

            transactionWrapper = new TransactionWrapper(transaction, enlistment);
            transaction.Enlist(transactionWrapper.Identifier, enlistment);

            return transactionWrapper;
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

        internal void InitiateAbort(Transaction transaction)
        {
            InitiateAbort(transaction, null);
        }

        internal void InitiateAbort(Transaction transaction, Exception exception)
        {
            try
            {
                transaction.RaiseAbort();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        internal async Task InitiateCommit(Transaction transaction, bool isPrimaryCommit = false)
        {
            var task = Task.Run(() =>
            {
                var expectedEnlistmentCount = transaction.ExpectedEnlistmentCount;
                var count = 0;
                IEnlistment enlistment;
                ITransaction transactionWrapper;

                using (((ILockable)transaction).Lock())
                {
                    count = transaction.Enlistments.Count;

                    if (isPrimaryCommit)
                    {
                        primaryCommitted = true;
                    }

                    if (expectedEnlistmentCount != -1 && expectedEnlistmentCount < count)
                    {
                        return;
                    }
                }

                for (var x = 0; x < count; x++)
                {
                    using (((ILockable)transaction).Lock())
                    {
                        var key = transaction.Enlistments.Keys.ElementAt(x);

                        enlistment = transaction.Enlistments[key];

                        if (enlistment.Action != TransactionAction.Committed)
                        {
                            return;
                        }
                    }
                }

                if (primaryCommitted)
                {
                    try
                    {
                        for (var x = 0; x < count; x++)
                        {
                            using (((ILockable)transaction).Lock())
                            {
                                var key = transaction.Enlistments.Keys.ElementAt(x);

                                enlistment = transaction.Enlistments[key];
                                transactionWrapper = enlistment.Transaction;

                                enlistment.FinalCommit();
                            }
                        }

                        transaction.RaiseCommit();
                    }
                    catch (Exception ex)
                    {
                        InitiateAbort(transaction, ex);
                    }
                }
            });

            await task;
        }
    }
}
