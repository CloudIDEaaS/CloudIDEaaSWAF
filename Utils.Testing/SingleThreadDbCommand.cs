using System.Data;
using System.Data.Common;

namespace Utils
{
    public class SingleThreadDbCommand : DbCommand
    {
        private DbCommand internalDbCommand;
        private SingleThreadDbConnection dbConnection;
        private IManagedLockObject lockObject;

        public SingleThreadDbCommand(DbCommand internalDbCommand, SingleThreadDbConnection dbConnection, IManagedLockObject lockObject)
        {
            this.internalDbCommand = internalDbCommand;
            this.dbConnection = dbConnection;
            this.lockObject = lockObject;
        }

        public override string CommandText 
        {
            get
            {
                return internalDbCommand.CommandText;
            }

            set
            {
                internalDbCommand.CommandText = value;
            }
        }

        public override int CommandTimeout
        {
            get
            {
                return internalDbCommand.CommandTimeout;
            }

            set
            {
                internalDbCommand.CommandTimeout = value;
            }
        }

        public override CommandType CommandType
        {
            get
            {
                return internalDbCommand.CommandType;
            }

            set
            {
                internalDbCommand.CommandType = value;
            }
        }

        public override bool DesignTimeVisible
        {
            get;
            set;
        }

        public override UpdateRowSource UpdatedRowSource
        {
            get
            {
                return internalDbCommand.UpdatedRowSource;
            }

            set
            {
                internalDbCommand.UpdatedRowSource = value;
            }
        }

        protected override DbConnection DbConnection
        {
            get
            {
                return dbConnection;
            }

            set
            {
                dbConnection = (SingleThreadDbConnection) value;
            }
        }

        protected override DbParameterCollection DbParameterCollection
        {
            get
            {
                return internalDbCommand.GetPropertyValue<DbParameterCollection>("DbParameterCollection");
            }
        }

        protected override DbTransaction DbTransaction
        {
            get
            {
                return internalDbCommand.Transaction;
            }

            set
            {
                if (value is SingleThreadDbTransaction)
                {
                    internalDbCommand.Transaction = internalDbCommand.Transaction;
                }
                else
                {
                    internalDbCommand.Transaction = value;
                }
            }
        }


        public override void Cancel()
        {
            internalDbCommand.Cancel();
        }

        public override int ExecuteNonQuery()
        {
            using (lockObject.Lock())
            {
                return internalDbCommand.ExecuteNonQuery();
            }
        }

        public override object ExecuteScalar()
        {
            using (lockObject.Lock())
            {
                return internalDbCommand.ExecuteScalar();
            }
        }

        public override void Prepare()
        {
            internalDbCommand.Prepare();
        }

        protected override DbParameter CreateDbParameter()
        {
            return internalDbCommand.CreateParameter();
        }

        protected override DbDataReader ExecuteDbDataReader(CommandBehavior behavior)
        {
            using (lockObject.Lock())
            {
                return internalDbCommand.ExecuteReader(behavior);
            }
        }
    }
}