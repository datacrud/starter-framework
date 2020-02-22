using Project.Model;
using Project.Model.Repositories;


namespace Project.Repository
{
    public interface IWarehouseRepository : IHaveTenantIdCompanyIdRepositoryBase<Warehouse>
    {
        Warehouse CreateTenantWarehouse(Warehouse warehouse);
    }

    public class WarehouseRepository : HaveTenantIdCompanyIdRepositoryBase<Warehouse>, IWarehouseRepository
    {
        private readonly BusinessDbContext _db;
        public WarehouseRepository(BusinessDbContext db) : base(db)
        {
            _db = db;
        }

        public Warehouse CreateTenantWarehouse(Warehouse warehouse)
        {
            return _db.Warehouses.Add(warehouse);

        }
    }
}