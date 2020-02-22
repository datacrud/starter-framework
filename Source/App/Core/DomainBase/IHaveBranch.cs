namespace Project.Core.DomainBase
{
    public interface IHaveBranch<TKey>
    {
        TKey BranchId { set; get; }
    }
}