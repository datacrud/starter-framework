using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Project.Core.DomainBase;
using Project.ViewModel.Bases;

namespace Project.Service.Bases
{
    public interface IServiceBase<T, TVm> : IPagingService<T, TVm>
        where T : Entity<string>, IHaveIsActive, IMayHaveOrder //BusinessEntityBase 
        where TVm : ViewModelBase<T>
    {
        /// <summary>
        /// Get all without tenant filter
        /// </summary>
        /// <returns></returns>
        List<TVm> GetAll();

        TVm GetById(string id);
        TVm GetByIdAsNoTracking(string id);
        Task<T> GetByIdAsync(string id);
        Task<T> GetByIdAsNoTrackingAsync(string id);

        T GetEntityById(string id);
        T GetEntityByIdAsNoTracking(string id, params Expression<Func<T, object>>[] expression);


        //Host Add, Edit
        bool CreateAsHost(T entity);
        bool CreateAsHost(List<T> entities);
        bool EditAsHost(T entity);
        bool EditAsHost(List<T> entity);


        bool Delete(string id);
        bool Delete(List<T> entries);
        bool Trash(string id);

        void Dispose();

    }
}