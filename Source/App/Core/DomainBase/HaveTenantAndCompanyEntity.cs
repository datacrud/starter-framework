namespace Project.Core.DomainBase
{
    public abstract class HaveTenantAndCompanyEntity<TKey> : HaveTenantEntity<TKey>, IHaveTenant<TKey>, IHaveCompany<TKey>
    {
        public TKey CompanyId { set; get; }
    }
}