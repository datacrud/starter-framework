namespace Project.Core.DomainBase
{
    public abstract class HaveTenantCompanyAndBranchEntity<TKey> : HaveTenantAndCompanyEntity<TKey>, IHaveTenant<TKey>, IHaveCompany<TKey>, IHaveBranch<TKey>
    {
        public TKey BranchId { set; get; }
    }
}