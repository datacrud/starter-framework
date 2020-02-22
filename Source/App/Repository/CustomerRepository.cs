using System.Linq;
using Project.Model;
using Project.Model.Repositories;

namespace Project.Repository
{
    public interface ICustomerRepository : IHaveTenantIdCompanyIdBranchIdRepositoryBase<Customer>
    {

    }

    public class CustomerRepository :HaveTenantIdCompanyIdBranchIdRepositoryBase<Customer>,ICustomerRepository
    {
        private BusinessDbContext _db;
        public CustomerRepository(BusinessDbContext db) : base(db)
        {
            _db = db;
        }
        
    }
}
