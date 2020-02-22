using Project.Model.EntityBase;

namespace Project.Server.Controllers.Bases
{
    public interface IHaveTenantIdCompanyIdControllerBase<in T> : IControllerBase<T> where T : HaveTenantIdCompanyIdEntityBase
    {

    }
}