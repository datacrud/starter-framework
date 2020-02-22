using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using Project.Core.Extensions;
using Project.Core.Extensions.Framework;
using Project.Core.Session;
using Project.Model.EntityBase;

namespace Project.Model.Repositories
{
    public abstract class HaveTenantIdCompanyIdRepositoryBase<TEntity> : Repository<TEntity>, IHaveTenantIdCompanyIdRepositoryBase<TEntity> where TEntity : HaveTenantIdCompanyIdEntityBase
    {

        protected HaveTenantIdCompanyIdRepositoryBase(BusinessDbContext db) : base(db)
        {
            Context = db;
        }


        #region Tenant Gets

        public virtual IQueryable<TEntity> GetAllAsTenant(string tenantId = null)
        {
            return Context.Set<TEntity>().AsQueryable().FilterTenant(tenantId);
        }

        public override IQueryable<TEntity> AsNoTracking()
        {
            return GetAll().AsNoTracking().FilterTenant();
        }

        #endregion





        #region Tenant Add, Edit

        public virtual TEntity AddAsTenant(TEntity entity)
        {
            if (string.IsNullOrWhiteSpace(entity.TenantId)
                || string.IsNullOrWhiteSpace(entity.CompanyId))
            {
                entity.TenantId = AppSession.TenantId;
                entity.CompanyId = AppSession.CompanyId;
            }

            return Context.Set<TEntity>().Add(entity);
        }

        public virtual IEnumerable<TEntity> AddAsTenant(List<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                entity.TenantId = AppSession.TenantId;
                entity.CompanyId = AppSession.CompanyId;
            }
            return Context.Set<TEntity>().AddRange(entities);
        }

        public virtual EntityState EditAsTenant(TEntity entity)
        {

            if (string.IsNullOrWhiteSpace(entity.TenantId)
                || string.IsNullOrWhiteSpace(entity.CompanyId))
            {
                entity.TenantId = AppSession.TenantId;
                entity.CompanyId = AppSession.CompanyId;
            }

            return Context.Entry(entity).State = EntityState.Modified;
        }

        public virtual EntityState EditAsTenant(List<TEntity> entities)
        {
            EntityState entityState = 0;
            foreach (var entity in entities)
            {
                entity.TenantId = AppSession.TenantId;
                entity.CompanyId = AppSession.CompanyId;

                entityState = Context.Entry(entity).State = EntityState.Modified;
            }

            return entityState;
        }

        public void UpdateAsTenant(TEntity entity)
        {
            if (string.IsNullOrWhiteSpace(entity.TenantId)
                || string.IsNullOrWhiteSpace(entity.CompanyId))
            {
                entity.TenantId = AppSession.TenantId;
                entity.CompanyId = AppSession.CompanyId;
            }

            var find = Context.Set<TEntity>().Find(entity.Id);
            if (find != null)
            {

                Context.Entry(find).CurrentValues.SetValues(entity);
            }
        }

        #endregion
    }
}