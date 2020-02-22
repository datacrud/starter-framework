using System.Data.Entity;

namespace Project.Core.UoW
{
    public interface IUnitOfWorkFactory
    {
        ITransactionUnitOfWork Create();
    }
}