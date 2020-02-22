namespace Project.Core.DomainBase
{
    public abstract class HaveTenantEntity<TKey>: Entity<TKey>, IHaveTenant<TKey>, IHaveIsActive, IMayHaveOrder
    {
        public TKey TenantId { set; get; }

        public bool Active { get; set; }
        public int? Order { get; set; }
    }
}