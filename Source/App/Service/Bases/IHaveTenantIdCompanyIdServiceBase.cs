using System.Collections.Generic;
using Project.Model.EntityBase;
using Project.ViewModel.Bases;

namespace Project.Service.Bases
{
    public interface IHaveTenantIdCompanyIdServiceBase<T, TVm> : IServiceBase<T, TVm>, IHaveTenantIdCompanyIdPagingService<T, TVm>
        where T : HaveTenantIdCompanyIdEntityBase
        where TVm : HaveTenantIdCompanyIdViewModelBase<T>
    {

        //Tenant Gets
        List<TVm> GetAllAsTenant(string tenantId = null);


        //Tenant Add,Edit
        bool AddAsTenant(T entity);
        bool AddAsTenant(List<T> entities);
        bool EditAsTenant(T entity);
        bool EditAsTenant(List<T> entity);

        bool Restore(string id);

    }
}