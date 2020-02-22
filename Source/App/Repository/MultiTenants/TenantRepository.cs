using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project.Core.Extensions.Framework;
using Project.Model;
using Project.Model.Repositories;

namespace Project.Repository
{
    public interface ITenantRepository : IRepository<Tenant>
    {
        //Tenant Gets
        IQueryable<Tenant> GetAllAsTenant(string tenantId = null);


        //Tenant add, edit
        Tenant AddAsTenant(Tenant entity);
        IEnumerable<Tenant> AddAsTenant(List<Tenant> entities);
        EntityState EditAsTenant(Tenant entity);
        EntityState EditAsTenant(List<Tenant> entities);

    }

    public class TenantRepository : Repository<Tenant>, ITenantRepository
    {
        private readonly BusinessDbContext _db;
        public TenantRepository(BusinessDbContext db) : base(db)
        {
            _db = db;
        }


        #region Tenant Gets

        public IQueryable<Tenant> GetAllAsTenant(string tenantId = null)
        {

            return _db.Tenants.AsQueryable();
        }


        #endregion


        #region Tenant Add, Edit

        public Tenant AddAsTenant(Tenant entity)
        {
            return _db.Tenants.Add(entity);
        }

        public IEnumerable<Tenant> AddAsTenant(List<Tenant> entities)
        {
            return _db.Tenants.AddRange(entities);
        }

        public EntityState EditAsTenant(Tenant entity)
        {
            return Context.Entry(entity).State = EntityState.Modified;
        }

        public EntityState EditAsTenant(List<Tenant> entities)
        {
            EntityState entityState = 0;
            foreach (var entity in entities)
            {

                entityState = Context.Entry(entity).State = EntityState.Modified;
            }

            return entityState;
        }

        #endregion

        public override EntityState Trash(Tenant entity)
        {
            entity.IsActive = false;
            entity.Active = false;
            entity.IsDeleted = true;
            return Context.Entry(entity).State = EntityState.Modified;
        }

        public override EntityState TrashAll(List<Tenant> entries)
        {
            foreach (Tenant entity in entries)
            {
                entity.IsActive = false;
                entity.Active = false;
                entity.IsDeleted = true;
                Context.Entry(entity).State = EntityState.Modified;
            }

            return EntityState.Modified;
        }

    }
}
