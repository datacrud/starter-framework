using Project.Model;
using Project.Repository;
using Project.Service.Bases;
using Project.ViewModel;

namespace Project.Service
{
    public interface IPartnerService : IHaveTenantIdCompanyIdBranchIdServiceBase<Partner, PartnerViewModel>
    {

    }

    public class PartnerService : HaveTenantIdCompanyIdBranchIdServiceBase<Partner, PartnerViewModel>, IPartnerService
    {
        private readonly IPartnerRepository _repository;

        public PartnerService(IPartnerRepository repository) : base(repository)
        {
            _repository = repository;
        }
    }
}