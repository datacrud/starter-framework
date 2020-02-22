using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Project.Core.Extensions;
using Project.Core.Extensions.Framework;
using Project.Core.Session;
using Project.Model.EntityBase;
using Project.Model.EntityBases;

namespace Project.Model.Repositories
{
    public abstract class HaveTenantIdCompanyIdBranchIdRepositoryBase<TEntity> : Repository<TEntity>, IHaveTenantIdCompanyIdBranchIdRepositoryBase<TEntity> where TEntity: HaveTenantIdCompanyIdBranchIdEntityBase
    {


        protected HaveTenantIdCompanyIdBranchIdRepositoryBase(BusinessDbContext db) : base(db)
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

            if (string.IsNullOrWhiteSpace(entity.Id)) entity.Id = Guid.NewGuid().ToString();

            SetMultiTenantIds(entity);

            entity.Active = true;
            entity.Created = DateTime.Now;
            if (!string.IsNullOrWhiteSpace(AppSession.UserId)) entity.CreatedBy = AppSession.UserId;

            return Context.Set<TEntity>().Add(entity);
        }

       

        public virtual List<TEntity> AddAsTenant(List<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                if (string.IsNullOrWhiteSpace(entity.Id)) entity.Id = Guid.NewGuid().ToString();

                SetMultiTenantIds(entity);

                entity.Active = true;
                entity.Created = DateTime.Now;
                if (!string.IsNullOrWhiteSpace(AppSession.UserId)) entity.CreatedBy = AppSession.UserId;
            }

            var range = Context.Set<TEntity>().AddRange(entities);

            return range.ToList();
        }

        public virtual TEntity EditAsTenant(TEntity entity)
        {

            SetMultiTenantIds(entity);

            entity.Modified = DateTime.Now;
            if (!string.IsNullOrWhiteSpace(AppSession.UserId)) entity.ModifiedBy = AppSession.UserId;

            Context.Entry(entity).State = EntityState.Modified;

            return entity;
        }

        public virtual List<TEntity> EditAsTenant(List<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                SetMultiTenantIds(entity);

                entity.Modified = DateTime.Now;
                if (!string.IsNullOrWhiteSpace(AppSession.UserId)) entity.ModifiedBy = AppSession.UserId;

                Context.Entry(entity).State = EntityState.Modified;
            }

            return entities;
        }

        public TEntity UpdateAsTenant(TEntity entity)
        {
            SetMultiTenantIds(entity);

            entity.Modified = DateTime.Now;
            if(!string.IsNullOrWhiteSpace(AppSession.UserId))entity.ModifiedBy = AppSession.UserId;

            var find = Context.Set<TEntity>().Find(entity.Id);
            if (find != null)
            {

                Context.Entry(find).CurrentValues.SetValues(entity);
            }

            return entity;
        }

        private void SetMultiTenantIds(TEntity entity)
        {
            if (!string.IsNullOrWhiteSpace(AppSession.TenantId)) entity.TenantId = AppSession.TenantId;

            if (!string.IsNullOrWhiteSpace(AppSession.CompanyId)) entity.CompanyId = AppSession.CompanyId;

            if (!string.IsNullOrWhiteSpace(AppSession.BranchId)) entity.BranchId = AppSession.BranchId;

        }

        #endregion

    }
}
