using Project.Model.EntityBases;

namespace Project.Server.Controllers.Bases
{
    public interface IHaveTenantIdCompanyIdBranchIdControllerBase<in T> : IControllerBase<T> where T : HaveTenantIdCompanyIdBranchIdEntityBase
    {
        
    }
}