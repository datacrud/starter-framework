using System;
using System.Collections.Generic;
using System.Linq;
using Project.Repository;

namespace Project.Server.Service
{

    public interface ISecurityBaseService<T> where T : class
    {
        List<T> GetAll();
        T GetById(string id);
        bool Add(T entity);
        bool Edit(T entity);
        bool Delete(string id);
    }



    public abstract class SecurityBaseService<TEntity> : ISecurityBaseService<TEntity> where TEntity: class
    {
        protected  ISecurityBaseRepository<TEntity> Repository;

        protected SecurityBaseService(ISecurityBaseRepository<TEntity> repository)
        {
            Repository = repository;
        }

        public virtual List<TEntity> GetAll()
        {
            return Repository.GetAll().ToList();
        }

        public virtual TEntity GetById(string id)
        {
            return Repository.GetById(id);
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

    }



    

}
