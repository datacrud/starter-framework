using System.ComponentModel.DataAnnotations.Schema;
using Project.Core.DomainBase;
using Security.Models.Models;

namespace Project.Model.EntityBases
{
    /// <summary>
    /// Have Tenant Id
    /// </summary>
    public abstract class HaveTenantIdEntityBase : HaveTenantEntity<string>, IHaveTenant<string>, IMayHaveUserReference<User>
    {
        //public string TenantId { get; set; }
        [ForeignKey("TenantId")] public virtual Tenant Tenant { get; set; }



        [ForeignKey("CreatedBy")] public virtual User Creator { get; set; }


        [ForeignKey("ModifiedBy")] public virtual User Modifier { get; set; }

        [ForeignKey("DeletedBy")] public virtual User Deleter { get; set; }
    }
}