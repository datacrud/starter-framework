using System;
using System.Transactions;
using IsolationLevel = System.Data.IsolationLevel;

namespace Project.Core.UoW
{
    public class TransactionScopeUnitOfWork : ITransactionUnitOfWork
    {
        private bool _disposed = false;

        private readonly TransactionScope _transactionScope;

        public TransactionScopeUnitOfWork(IsolationLevel isolationLevel)
        {
            this._transactionScope = new TransactionScope(
                TransactionScopeOption.Required,
                new TransactionOptions
                {
                    IsolationLevel = (System.Transactions.IsolationLevel) isolationLevel,
                    Timeout = TransactionManager.MaximumTimeout
                });
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    this._transactionScope.Dispose();
                }

                _disposed = true;
            }
        }

        public void Commit()
        {
            this._transactionScope.Complete();
        }

    }
}