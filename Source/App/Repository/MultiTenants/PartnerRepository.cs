using Project.Model;
using Project.Model.Repositories;

namespace Project.Repository
{
    public interface IPartnerRepository : IHaveTenantIdCompanyIdBranchIdRepositoryBase<Partner>
    {

    }
    public class PartnerRepository : HaveTenantIdCompanyIdBranchIdRepositoryBase<Partner>, IPartnerRepository
    {
        public PartnerRepository(BusinessDbContext db) : base(db)
        {
        }
    }
}