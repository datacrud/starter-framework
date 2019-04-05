using System;
using System.Collections.Generic;
using System.Linq;
using Project.Model;
using Project.Repository;

namespace Project.Service
{

    public interface IBaseService<T> where T : Entity
    {
        List<T> GetAll();
        T GetById(string id);
        bool Add(T entity);
        bool Add(List<T> entities);
        bool Edit(T entity);
        bool Delete(string id);
        bool Trash(string id);
    }



    public abstract class BaseService<TEntity> : IBaseService<TEntity> where TEntity: Entity
    {
        protected  IBaseRepository<TEntity> Repository;

        protected BaseService(IBaseRepository<TEntity> repository)
        {
            Repository = repository;
        }

        public virtual List<TEntity> GetAll()
        {
            List<TEntity> entities;
            try
            {
                entities = Repository.GetAll().ToList();
            }
            catch (Exception exception)
            {                
                throw new Exception(exception.Message);
            }

            return entities;
        }

        public virtual TEntity GetById(string id)
        {
            TEntity entity;
            try
            {
                entity = Repository.GetById(id);
            }
            catch (Exception exception)
            {                
                throw new Exception(exception.Message);
            }

            return entity;
        }


        public virtual bool Add(TEntity entity)
        {
            try
            {                
                Repository.Add(entity);
                Repository.Commit();                
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }

            return true;
        }

        public bool Add(List<TEntity> entities)
        {
            try
            {
                Repository.Add(entities);
                Repository.Commit();
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
            return true;
        }


        public virtual bool Edit(TEntity entity)
        {
            try
            {                
                Repository.Edit(entity);
                Repository.Commit();
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }

            return true;
        }

        public virtual bool Delete(string id)
        {
            try
            {
                Repository.Delete(Repository.GetById(id));
                Repository.Commit();
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
            return true;            
        }

        public bool Trash(string id)
        {
            try
            {
                TEntity entity = Repository.GetById(id);
                entity.Active = false;
                Repository.Trash(entity);
                Repository.Commit();
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
            return true;
        }
    }



    

}
