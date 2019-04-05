using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Project.Repository
{
    public interface ISecurityBaseRepository<T> where T : class
    {
        IQueryable<T> GetAll();
        T GetById(string id);
        T Add(T entity);
        EntityState Edit(T entity);
        T Delete(T entity);

        void Commit();
        IQueryable<T> Add(List<T> entities);
    }



    public abstract class SecurityBaseRepository<TEntity> : ISecurityBaseRepository<TEntity> where TEntity: class
    {
        protected DbContext DbContext;

        protected SecurityBaseRepository(DbContext db)
        {
            DbContext = db;
        }

        public virtual IQueryable<TEntity> GetAll()
        {
            return DbContext.Set<TEntity>().AsQueryable();
        }

        public virtual TEntity GetById(string id)
        {
            return DbContext.Set<TEntity>().Find(id);
        }

        public virtual TEntity Add(TEntity entity)
        {
            return DbContext.Set<TEntity>().Add(entity);
        }

        public virtual IQueryable<TEntity> Add(List<TEntity> entities)
        {
            return DbContext.Set<TEntity>().AddRange(entities).AsQueryable();
        }

        public virtual EntityState Edit(TEntity entity)
        {
            return DbContext.Entry(entity).State = EntityState.Modified;
        }

        public virtual TEntity Delete(TEntity entity)
        {
            return DbContext.Set<TEntity>().Remove(entity);
        }        

        public void Commit()
        {
            DbContext.SaveChanges();
        }
        
    }

    
}
