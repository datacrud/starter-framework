using System.Collections.Generic;
using System.Linq;
using Project.Model.EntityBase;
using Project.Model.EntityBases;

namespace Project.Model.Repositories
{
    public interface IHaveTenantIdCompanyIdBranchIdRepositoryBase<T> : IRepository<T> where T : HaveTenantIdCompanyIdBranchIdEntityBase
    {

        //Tenant Gets
        IQueryable<T> GetAllAsTenant(string tenantId = null);
        

        //Tenant add, edit
        T AddAsTenant(T entity);
        List<T> AddAsTenant(List<T> entities);
        T EditAsTenant(T entity);
        List<T> EditAsTenant(List<T> entities);
        T UpdateAsTenant(T entity);
    }
}