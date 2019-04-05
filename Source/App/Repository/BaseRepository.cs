using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Project.Model;

namespace Project.Repository
{
    public interface IBaseRepository<T> where T : Entity
    {
        IQueryable<T> GetAll();
        T GetById(string id);
        T Add(T entity);
        IQueryable<T> Add(List<T> entities);
        EntityState Edit(T entity);
        EntityState Trash(T entity);
        T Delete(T entity);

        void Commit();        
    }



    public abstract class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity: Entity
    {
        protected DbContext DbContext;

        protected BaseRepository(DbContext db)
        {
            DbContext = db;
        }

        public virtual IQueryable<TEntity> GetAll()
        {
            return DbContext.Set<TEntity>().Where(x=> x.Active).AsQueryable();
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

        public virtual EntityState Trash(TEntity entity)
        {
            return DbContext.Entry(entity).State = EntityState.Modified;
        }

        public TEntity Delete(TEntity entity)
        {
            return DbContext.Set<TEntity>().Remove(entity);
        }

        public void Commit()
        {
            DbContext.SaveChanges();
        }
        
    }

    
}
