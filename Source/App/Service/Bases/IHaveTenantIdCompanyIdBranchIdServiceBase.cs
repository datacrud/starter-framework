using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Threading.Tasks;
using Project.Model.EntityBase;
using Project.Model.EntityBases;
using Project.ViewModel.Bases;

namespace Project.Service.Bases
{
    public interface IHaveTenantIdCompanyIdBranchIdServiceBase<T, TVm> : IServiceBase<T, TVm>, IHaveTenantIdCompanyIdBranchIdPagingService<T, TVm>
        where T : HaveTenantIdCompanyIdBranchIdEntityBase
        where TVm : HaveTenantIdCompanyIdBranchIdViewModelBase<T>
    {

        //Tenant Gets
        /// <summary>
        /// Get all data filter by tenant
        /// </summary>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        List<TVm> GetAllAsTenant(string tenantId = null);


        //Tenant Add,Edit
        bool CreateAsTenant(T entity);
        bool CreateAsTenant(List<T> entities);
        bool EditAsTenant(T entity);
        bool EditAsTenant(List<T> entity);

        bool Restore(string id);

    }
}