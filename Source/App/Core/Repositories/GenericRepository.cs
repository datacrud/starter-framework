using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;
using Project.Core.DomainBase;

namespace Project.Core.Repositories
{
    public class GenericRepository<TDc, T, TKey> : IGenericRepository<T, TKey> where T : class, IEntity<TKey> where TDc : DbContext, new()
    {

        public TDc Context { get; set; }


        public GenericRepository(TDc dbContext)
        {
            Context = new TDc();
        }


        public virtual IQueryable<T> GetAll()
        {

            IQueryable<T> query = Context.Set<T>();
            return query;
        }

        public IQueryable<T> FindBy(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {

            IQueryable<T> query = Context.Set<T>().Where(predicate);
            return query;
        }

        public T Get(object id)
        {
            T entity = Context.Set<T>().Find(id);
            return entity;
        }

        public T GetById(object id)
        {
            return Get(id);
        }

        public T First(Expression<Func<T, bool>> predicate)
        {
            T entity = Context.Set<T>().First(predicate);
            return entity;
        }

        public T FirstOrDefault(Expression<Func<T, bool>> predicate)
        {
            T entity = Context.Set<T>().FirstOrDefault(predicate);
            return entity;
        }

        public T Last(Expression<Func<T, bool>> predicate)
        {
            T entity = Context.Set<T>().Last(predicate);
            return entity;
        }

        public T LastOrDefault(Expression<Func<T, bool>> predicate)
        {
            T entity = Context.Set<T>().LastOrDefault(predicate);
            return entity;
        }

        public virtual T Create(T entity)
        {
            return Context.Set<T>().Add(entity);
        }

        public List<T> Create(List<T> entities)
        {
            Context.Set<T>().AddRange(entities);
            return entities;
        }


        public void Delete(TKey id)
        {
            var entity = Get(id);
            Delete(entity);
        }

        public virtual void Delete(T entity)
        {
            Context.Set<T>().Remove(entity);
        }

        public void Delete(List<T> entities)
        {
            Context.Set<T>().RemoveRange(entities);
        }


        public virtual T Modify(T entity)
        {
            Context.Entry(entity).State = EntityState.Modified;
            return entity;
        }

        public List<T> Modify(List<T> entities)
        {
            foreach (var entity in entities)
            {
                Context.Entry(entity).State = EntityState.Modified;
            }
            return entities;
        }

        public virtual T Update(T entity)
        {
            var type = typeof(T);
            var propertyInfo = type.GetProperty("Id");
            if (propertyInfo != null)
            {
                var id = propertyInfo.GetValue(entity);
                var find = Context.Set<T>().Find(id);
                if (find != null)
                {

                    Context.Entry(find).CurrentValues.SetValues(entity);
                }
            }

            return entity;
        }

        public List<T> Update(List<T> entities)
        {
            foreach (var entity in entities)
            {
                Update(entity);

            }
            return entities;
        }

        public virtual void Save()
        {
            try
            {
                Context.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine(@"Entity of type ""{0}"" in state ""{1}"" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine(@"- Property: ""{0}"", Error: ""{1}""",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
        }

        public void Commit()
        {
            Save();
        }
    }
}