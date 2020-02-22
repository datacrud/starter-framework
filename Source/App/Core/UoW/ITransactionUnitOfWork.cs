namespace Project.Core.UoW
{
    public interface ITransactionUnitOfWork
    {
        void Commit();
    }
}