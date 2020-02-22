using Project.Model.EntityBases;

namespace Project.Server.Controllers.Bases
{
    public interface ICompanyControllerBase<in T> : IControllerBase<T> where T : HaveTenantIdEntityBase
    {

    }
}