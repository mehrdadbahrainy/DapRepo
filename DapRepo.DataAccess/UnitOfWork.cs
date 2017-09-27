using System;
using System.Data;
using System.Transactions;
using IsolationLevel = System.Transactions.IsolationLevel;

namespace DapRepo.DataAccess
{
    public abstract class UnitOfWork : IUnitOfWork
    {
        protected IDbConnection DbConnection { get; private set; }
        private TransactionScope _transaction;
        private bool _disposed;

        protected UnitOfWork(
            bool isTransactional = false,
            IsolationLevel isolationLevel = IsolationLevel.ReadCommitted,
            int transactionTimeoutInSecond = 300,
            TransactionScopeOption transactionScopeOption = TransactionScopeOption.Required)
        {
            DbConnection = CreateConnection();

            if (isTransactional)
            {
                BeginTransaction(isolationLevel, transactionTimeoutInSecond, transactionScopeOption);
            }
            else
            {
                DbConnection.Open();
            }

        }

        protected abstract IDbConnection CreateConnection();

        public void BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted, int transactionTimeoutInSecound = 300, TransactionScopeOption transactionScopeOption = TransactionScopeOption.Required)
        {
            if (_transaction != null) return;

            if (DbConnection.State == ConnectionState.Open)
            {
                DbConnection.Close();
            }

            TransactionOptions transactionOptions = new TransactionOptions()
            {
                IsolationLevel = isolationLevel,
                Timeout = new TimeSpan(0, 0, transactionTimeoutInSecound)
            };

            _transaction = new TransactionScope(transactionScopeOption, transactionOptions);
            DbConnection.Open();
        }

        public void RollbackTransaction()
        {
            _transaction?.Dispose();
        }

        public void CommitTransaction()
        {
            _transaction?.Complete();
            _transaction?.Dispose();
            _transaction = null;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _transaction?.Dispose();
                    _transaction = null;

                    DbConnection?.Dispose();
                    DbConnection = null;
                }

                _disposed = true;
            }
        }

        ~UnitOfWork()
        {
            Dispose(false);
        }
    }
}