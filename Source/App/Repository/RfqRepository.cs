using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Project.Model;
using Project.Model.Repositories;

namespace Project.Repository
{
    public interface IRfqRepository : IRepository<Rfq>
    {

    }
    public class RfqRepository : Repository<Rfq>, IRfqRepository
    {
        private readonly BusinessDbContext _db;


        public RfqRepository(BusinessDbContext db) : base(db)
        {
            _db = db;
        }


        #region Rfq Gets

        public IQueryable<Rfq> GetAllAsTenant(string tenantId = null)
        {

            return _db.Rfqs.AsQueryable();
        }

        public IQueryable<Rfq> GetAllInactiveAsTenant(string tenantId = null)
        {

            return _db.Rfqs.Where(x => !x.Active).AsQueryable();
        }

        public IQueryable<Rfq> GetAllAsNoTrackingAsTenant(string tenantId = null)
        {

            return _db.Rfqs.AsNoTracking().AsQueryable();
        }

        public IQueryable<Rfq> GetAllActiveAsNoTrackingAsTenant(string tenantId = null)
        {

            return _db.Rfqs.AsNoTracking().Where(x => x.Active).AsQueryable();
        }

        public IQueryable<Rfq> GetAllInactiveAsNoTrackingAsTenant(string tenantId = null)
        {

            return _db.Rfqs.AsNoTracking().Where(x => !x.Active).AsQueryable();
        }

        #endregion


        #region Rfq Add, Edit

        public Rfq AddAsTenant(Rfq entity)
        {
            return _db.Rfqs.Add(entity);
        }

        public IEnumerable<Rfq> AddAsTenant(List<Rfq> entities)
        {
            return _db.Rfqs.AddRange(entities);
        }

        public EntityState EditAsTenant(Rfq entity)
        {
            return Context.Entry(entity).State = EntityState.Modified;
        }

        public EntityState EditAsTenant(List<Rfq> entities)
        {
            EntityState entityState = 0;
            foreach (var entity in entities)
            {

                entityState = Context.Entry(entity).State = EntityState.Modified;
            }

            return entityState;
        }

        #endregion
    }
}
