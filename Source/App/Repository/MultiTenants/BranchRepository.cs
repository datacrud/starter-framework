using Project.Model;
using Project.Model.Repositories;

namespace Project.Repository
{
    public interface IBranchRepository : IHaveTenantIdCompanyIdRepositoryBase<Branch>
    {
        Branch CreateTenantHeadOfficeBranch(Branch branch);
    }

    public class BranchRepository : HaveTenantIdCompanyIdRepositoryBase<Branch>, IBranchRepository
    {
        private readonly BusinessDbContext _db;
        public BranchRepository(BusinessDbContext db) : base(db)
        {
            _db = db;
        }

        public Branch CreateTenantHeadOfficeBranch(Branch branch)
        {
            return _db.Branches.Add(branch);
        }
    }
}