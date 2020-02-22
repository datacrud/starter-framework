using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using EntityFramework.DynamicFilters;
using Project.Core.DomainBase;
using Project.Core.Extensions;
using Project.Core.Repositories;
using Project.Core.Session;

namespace Project.Model.Repositories
{
    public class Repository<TEntity> : GenericRepository<BusinessDbContext, TEntity, string>, IRepository<TEntity>
        where TEntity : Entity<string>, IHaveIsActive, IMayHaveOrder //BusinessEntityBase
    {
        protected new DbContext Context;
        protected readonly IAppSession AppSession;

        protected Repository(BusinessDbContext db) : base(db)
        {
            Context = db;
            AppSession = new AppSession();
        }

        public Repository() : base(new BusinessDbContext())
        {
            Context = new BusinessDbContext();
            AppSession = new AppSession();
        }

        #region Filters

        public void DisableFilter(string filterName)
        {
            Context.DisableFilter(filterName);
        }

        public void EnableFilter(string filterName)
        {
            Context.EnableFilter(filterName);
        }

        public void DisableAllFilters()
        {
            Context.DisableAllFilters();
        }

        public void EnableAllFilters()
        {
            Context.EnableAllFilters();
        }

        #endregion

        #region Host Gets  


        public new virtual IQueryable<TEntity> GetAll()
        {
            return Context.Set<TEntity>().AsQueryable();
        }


        public virtual IQueryable<TEntity> AsNoTracking()
        {
            return GetAll().AsNoTracking();
        }

        #endregion



        public new virtual TEntity GetById(object id)
        {
            return Context.Set<TEntity>().Find(id);
        }

        public virtual Task<TEntity> GetByIdAsync(object id)
        {
            return Context.Set<TEntity>().FindAsync(id);
        }



        #region Host Add, Edit

        public virtual TEntity CreateAsHost(TEntity entity)
        {
            entity.Created = DateTime.Now;
            entity.CreatedBy = GetCurrentUserIdOrNull();

            return Context.Set<TEntity>().Add(entity);
        }

        public virtual List<TEntity> CreateAsHost(List<TEntity> entities)
        {


            foreach (var entity in entities)
            {
                entity.Created = DateTime.Now;
                entity.CreatedBy = GetCurrentUserIdOrNull();
            }

            var addAsHost = Context.Set<TEntity>().AddRange(entities);

            return entities;
        }

        public virtual EntityState EditAsHost(TEntity entity)
        {
            entity.Modified = DateTime.Now;
            entity.ModifiedBy = GetCurrentUserIdOrNull();

            return Context.Entry(entity).State = EntityState.Modified;
        }

        public virtual EntityState EditAsHost(List<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                entity.Modified = DateTime.Now;
                entity.ModifiedBy = GetCurrentUserIdOrNull();
            }
            EntityState entityState = 0;
            foreach (var entity in entities)
            {
                entityState = Context.Entry(entity).State = EntityState.Modified;
            }

            return entityState;
        }

        public TEntity UpdateAsHost(TEntity entity)
        {
            entity.Modified = DateTime.Now;
            entity.ModifiedBy = GetCurrentUserIdOrNull();

            var find = Context.Set<TEntity>().Find(entity.Id);
            if (find != null)
            {

                Context.Entry(find).CurrentValues.SetValues(entity);
            }

            return entity;
        }

        #endregion



        public virtual EntityState Trash(TEntity entity)
        {

            entity.IsDeleted = true;
            entity.Deleted = DateTime.Now;
            entity.DeletedBy = GetCurrentUserIdOrNull();

            entity.Active = false;
            return Context.Entry(entity).State = EntityState.Modified;
        }

        public virtual EntityState TrashAll(List<TEntity> entries)
        {
            foreach (TEntity entity in entries)
            {
                entity.IsDeleted = true;
                entity.Deleted = DateTime.Now;
                entity.DeletedBy = GetCurrentUserIdOrNull();

                entity.Active = false;
                Context.Entry(entity).State = EntityState.Modified;

            }

            return EntityState.Modified;
        }

        public new virtual TEntity Delete(TEntity entity)
        {
            return Context.Set<TEntity>().Remove(entity);
        }

        public new virtual List<TEntity> Delete(List<TEntity> entries)
        {
            var removeRange = Context.Set<TEntity>().RemoveRange(entries);

            return entries;
        }

        public new bool Commit()
        {
            try
            {
                int saveChanges = Context.SaveChanges();
                return saveChanges > 0;
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

        public async Task<bool> CommitAsync()
        {
            try
            {
                int saveChanges = await Context.SaveChangesAsync();
                return saveChanges > 0;
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


        public void Dispose()
        {
            Context.Dispose();
        }



        private string GetCurrentUserIdOrNull()
        {
            string currentUserId;
            try
            {
                currentUserId = HttpContext.Current.User.Identity.GetCurrentUserId();
                if (string.IsNullOrWhiteSpace(currentUserId)) return null;
            }
            catch (Exception e)
            {
                return null;
            }

            return currentUserId;
        }
    }
}