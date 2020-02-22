using Project.Model;
using Project.Model.Repositories;

namespace Project.Repository
{
    public interface IEmployeeRepository : IHaveTenantIdCompanyIdBranchIdRepositoryBase<Employee>
    {
        Employee CreateTenantAdminEmployee(Employee employee);
    }

    public class EmployeeRepository :HaveTenantIdCompanyIdBranchIdRepositoryBase<Employee>, IEmployeeRepository
    {
        private readonly BusinessDbContext _db;

        public EmployeeRepository(BusinessDbContext db) : base(db)
        {
            _db = db;
        }

        public Employee CreateTenantAdminEmployee(Employee employee)
        {
            return _db.Employees.Add(employee);
        }
    }
}
