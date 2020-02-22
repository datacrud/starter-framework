using System.Collections.Generic;
using Project.Model;
using Project.Model.Repositories;

namespace Project.Repository
{
    public interface ISupplierRepository : IHaveTenantIdCompanyIdBranchIdRepositoryBase<Supplier>
    {
        bool CreateTenantSupplier(List<Supplier> suppliers);
    }

    public class SupplierRepository :HaveTenantIdCompanyIdBranchIdRepositoryBase<Supplier>, ISupplierRepository
    {
        private readonly BusinessDbContext _db;
        public SupplierRepository(BusinessDbContext db) : base(db)
        {
            _db = db;
        }

        public bool CreateTenantSupplier(List<Supplier> suppliers)
        {
            _db.Suppliers.AddRange(suppliers);
            return true;
        }
    }
}
