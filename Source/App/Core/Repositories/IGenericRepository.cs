using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Project.Core.DomainBase;

namespace Project.Core.Repositories
{
    public interface IGenericRepository<T, TKey> where T : class, IEntity<TKey>
    {
        IQueryable<T> GetAll();
        IQueryable<T> FindBy(Expression<Func<T, bool>> predicate);
        T Get(object id);
        [Obsolete]
        T GetById(object id);
        T First(Expression<Func<T, bool>> predicate);
        T FirstOrDefault(Expression<Func<T, bool>> predicate);
        T Last(Expression<Func<T, bool>> predicate);
        T LastOrDefault(Expression<Func<T, bool>> predicate);
        T Create(T entity);
        List<T> Create(List<T> entities);
        T Modify(T entity);
        List<T> Modify(List<T> entity);
        T Update(T entity);
        List<T> Update(List<T> entity);
        void Delete(TKey id);
        void Delete(T entity);
        void Delete(List<T> entities);
        void Save();
        [Obsolete]
        void Commit();
    }
}