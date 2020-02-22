using Project.Model;
using Project.Model.Repositories;

namespace Project.Repository
{
    public interface IFiscalYearRepository : IHaveTenantIdCompanyIdBranchIdRepositoryBase<FiscalYear>
    {

    }


    public class FiscalYearRepository : HaveTenantIdCompanyIdBranchIdRepositoryBase<FiscalYear>, IFiscalYearRepository
    {
        private readonly BusinessDbContext _db;
        public FiscalYearRepository(BusinessDbContext db) : base(db)
        {
            _db = db;
        }
    }
}