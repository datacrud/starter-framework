using System.ComponentModel.DataAnnotations.Schema;
using Project.Core.DomainBase;

namespace Security.Models.Models
{
    public class SecurityTenantEntityBase<TKey> : Entity<TKey>, IHaveTenant<string>, IHaveCompany<string>
    {
        public string TenantId { get; set; }
        public string CompanyId { get; set; }

        [ForeignKey("CreatedBy")] public virtual User Creator { get; set; }

        [ForeignKey("ModifiedBy")] public virtual User Modifier { get; set; }

        [ForeignKey("DeletedBy")] public virtual User Deleter { get; set; }

    }
}