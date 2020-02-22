using System.Collections.Generic;
using Project.Model.EntityBases;
using Project.ViewModel.Bases;

namespace Project.Service.Bases
{
    public interface IHaveTenantIdServiceBase<T, TVm> : IServiceBase<T, TVm>, IHaveTenantIdPagingService<T, TVm>
        where T : HaveTenantIdEntityBase
        where TVm : HaveTenantIdViewModelBase<T>
    {

        /// <summary>
        /// Get all items filter by tenant
        /// </summary>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        List<TVm> GetAllAsTenant(string tenantId = null);
        

        //Tenant Add,Edit
        /// <summary>
        /// Create a tenant item
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool AddAsTenant(T entity);

        /// <summary>
        /// Create tenant items
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        bool AddAsTenant(List<T> entities);

        /// <summary>
        /// Edit a tenant item
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool EditAsTenant(T entity);

        /// <summary>
        /// Edit tenant items
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool EditAsTenant(List<T> entity);

        /// <summary>
        /// Restore a tenant item
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool Restore(string id);

    }
}