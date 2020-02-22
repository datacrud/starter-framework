using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Project.Model.EntityBase;

namespace Project.Model.Repositories
{
    public interface IHaveTenantIdCompanyIdRepositoryBase<T> : IRepository<T> where T : HaveTenantIdCompanyIdEntityBase
    {
        //Tenant Gets
        IQueryable<T> GetAllAsTenant(string tenantId = null);


        //Tenant add, edit
        T AddAsTenant(T entity);
        IEnumerable<T> AddAsTenant(List<T> entities);
        EntityState EditAsTenant(T entity);
        EntityState EditAsTenant(List<T> entities);
        void UpdateAsTenant(T entity);
    }
}