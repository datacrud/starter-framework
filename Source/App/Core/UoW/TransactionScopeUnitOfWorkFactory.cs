using System.Data;

namespace Project.Core.UoW
{
    public class TransactionScopeUnitOfWorkFactory : IUnitOfWorkFactory
    {
        private readonly IsolationLevel _isolationLevel;

        public TransactionScopeUnitOfWorkFactory(IsolationLevel isolationLevel)
        {
            _isolationLevel = isolationLevel;
        }

        public ITransactionUnitOfWork Create()
        {
            return new TransactionScopeUnitOfWork(_isolationLevel);
        }
    }
}