using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace Utils
{
    public class SingleThreadDbConnection : DbConnection
    {
        private DbConnection internalDbConnection;
        internal IManagedLockObject lockObject;

        public SingleThreadDbConnection(DbConnection internalDbConnection)
        {
            this.internalDbConnection = internalDbConnection;
            lockObject = LockManager.CreateObject();
        }

        public override string ConnectionString 
        {
            get
            {
                return internalDbConnection.ConnectionString;
            }

            set
            {
                internalDbConnection.ConnectionString = value;
            }
        }

        public override string Database 
        {
            get
            {
                return internalDbConnection.Database;
            }
        }
        
        public override string DataSource 
        {
            get
            {
                return internalDbConnection.DataSource;
            }
        }

        public override string ServerVersion 
        {
            get
            {
                return internalDbConnection.ServerVersion;
            }
        }
        
        public override ConnectionState State 
        {
            get
            {
                return internalDbConnection.State;
            }
        }

        public override void ChangeDatabase(string databaseName)
        {
            internalDbConnection.ChangeDatabase(databaseName);
        }

        public override void Close()
        {
            using (lockObject.Lock())
            {
                internalDbConnection.Close();
            }
        }

        public override void Open()
        {
            using (lockObject.Lock())
            {
                internalDbConnection.Open();
            }
        }

        protected override DbTransaction BeginDbTransaction(IsolationLevel isolationLevel)
        {
            return new SingleThreadDbTransaction(internalDbConnection.BeginTransaction(isolationLevel), this, lockObject);
        }

        protected override DbCommand CreateDbCommand()
        {
            return new SingleThreadDbCommand(internalDbConnection.CreateCommand(), this, lockObject);
        }
    }
}
