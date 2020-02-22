using System.ComponentModel.DataAnnotations.Schema;
using Project.Core.DomainBase;
using Security.Models.Models;

namespace Project.Model.EntityBases
{
    /// <summary>
    ///     Have Tenant Id
    ///     Have Company Id
    ///     Have Branch Id
    /// </summary>
    public abstract class HaveTenantIdCompanyIdBranchIdEntityBase :
        HaveTenantCompanyAndBranchEntity<string>,
        IHaveTenant<string>,
        IHaveCompany<string>,
        IHaveBranch<string>,
        IMayHaveUserReference<User>
    {
        //public string TenantId { get; set; }
        [ForeignKey("TenantId")] public virtual Tenant Tenant { get; set; }


        //public string CompanyId { get; set; }
        [ForeignKey("CompanyId")] public virtual Company Company { get; set; }


        //public string BranchId { get; set; }
        [ForeignKey("BranchId")] public virtual Branch Branch { get; set; }


        [ForeignKey("CreatedBy")] public virtual User Creator { get; set; }


        [ForeignKey("ModifiedBy")] public virtual User Modifier { get; set; }

        [ForeignKey("DeletedBy")] public virtual User Deleter { get; set; }
    }
}
