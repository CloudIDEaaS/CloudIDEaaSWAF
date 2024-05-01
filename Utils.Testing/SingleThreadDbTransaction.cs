using System.Data;
using System.Data.Common;

namespace Utils
{
    public class SingleThreadDbTransaction : DbTransaction
    {
        internal DbTransaction internalDbTransaction;
        private SingleThreadDbConnection dbConnection;
        private IManagedLockObject lockObject;

        public SingleThreadDbTransaction(DbTransaction dbTransaction, SingleThreadDbConnection internalDbTransaction, IManagedLockObject lockObject)
        {
            this.internalDbTransaction = dbTransaction;
            this.dbConnection = internalDbTransaction;
            this.lockObject = lockObject;
        }

        public override IsolationLevel IsolationLevel
        {
            get
            {
                return internalDbTransaction.IsolationLevel;
            }
        }
        
        protected override DbConnection DbConnection
        {
            get
            {
                return dbConnection;
            }
        }

        public override void Commit()
        {
            using (lockObject.Lock())
            {
                internalDbTransaction.Commit();
            }
        }

        public override void Rollback()
        {
            using (lockObject.Lock())
            {
                internalDbTransaction.Rollback();
            }
        }
    }
}