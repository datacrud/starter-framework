namespace Project.Core.DomainBase
{
    public interface IHaveTenant<TKey>
    {
        TKey TenantId { set; get; }
    }
}