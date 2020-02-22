using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Project.Core.DomainBase;
using Project.Core.Repositories;

namespace Project.Model.Repositories
{
    public interface IRepository<T> : IGenericRepository<T, string> where T : Entity<string>, IHaveIsActive, IMayHaveOrder //BusinessEntityBase
    {
        /// <summary>
        /// Disable an active data filter. Use "DataFilter" const to define the filters
        /// </summary>
        /// <param name="filterName"></param>
        void DisableFilter(string filterName);

        /// <summary>
        /// Enable an inactive data filter. Use "DataFilter" const to define the filters
        /// </summary>
        /// <param name="filterName"></param>
        void EnableFilter(string filterName);

        /// <summary>
        /// Disable all active data filter
        /// </summary>
        void DisableAllFilters();

        /// <summary>
        /// Enable all disabled data filter
        /// </summary>
        void EnableAllFilters();



        //Host Gets
        /// <summary>
        /// Get all without tenant filter
        /// </summary>
        /// <returns></returns>
        new IQueryable<T> GetAll();

        /// <summary>
        /// Get all without tenant filter and tracking
        /// </summary>
        /// <returns></returns>
        IQueryable<T> AsNoTracking();



        //Host and Tenant Get by id
        new T GetById(object id);
        Task<T> GetByIdAsync(object id);


        //Host add, edit
        T CreateAsHost(T model);
        List<T> CreateAsHost(List<T> models);
        EntityState EditAsHost(T model);
        EntityState EditAsHost(List<T> models);
        T UpdateAsHost(T entity);



        //Host and Tenant Delete, Trash
        EntityState Trash(T entity);
        EntityState TrashAll(List<T> entries);
        new T Delete(T entity);
        new List<T> Delete(List<T> entries);


        new bool Commit();
        Task<bool> CommitAsync();
        void Dispose();
    }
}