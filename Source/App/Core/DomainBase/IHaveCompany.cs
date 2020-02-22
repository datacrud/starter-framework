namespace Project.Core.DomainBase
{
    public interface IHaveCompany<TKey>
    {
        TKey CompanyId { set; get; }
    }
}