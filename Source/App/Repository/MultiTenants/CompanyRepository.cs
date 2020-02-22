using Project.Model;
using Project.Model.Repositories;

namespace Project.Repository.MultiTenants
{
    public interface ICompanyRepository : IHaveTenantIdRepositoryBase<Company>
    {
        Company CreateTenantCompany(Company company);
    }

    public class CompanyRepository :HaveTenantIdRepositoryBase<Company>, ICompanyRepository
    {
        private readonly BusinessDbContext _db;

        public CompanyRepository(BusinessDbContext db) : base(db)
        {
            _db = db;
        }

        public Company CreateTenantCompany(Company company)
        {
            return _db.Companies.Add(company);
        }

    }
}
