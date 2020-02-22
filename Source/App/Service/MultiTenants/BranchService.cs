using System.Linq;
using Project.Core.Enums;
using Project.Core.Enums.Framework;
using Project.Model;
using Project.Repository;
using Project.Service.Bases;
using Project.ViewModel;

namespace Project.Service
{
    public interface IBranchService : IHaveTenantIdCompanyIdServiceBase<Branch, BranchViewModel>
    {
        bool IsHeadOfficeExist(BranchType branchType, string id, string tenantId);
        bool IsBranchExist(string name, string id, string tenantId);
        bool IsReachedMaximumBranchCount(string tenantId, int featureBranchCount);
    }


    public class BranchService : HaveTenantIdCompanyIdServiceBase<Branch, BranchViewModel>, IBranchService
    {
        private readonly IBranchRepository _repository;
       

        public BranchService(IBranchRepository repository) : base(repository)
        {
            _repository = repository;

        }

        public bool IsHeadOfficeExist(BranchType branchType, string id, string tenantId)
        {
            var branch = _repository.AsNoTracking().Where(x => x.TenantId == tenantId)
                .FirstOrDefault(x => x.Type == BranchType.HeadOffice);
            if (branch != null && branch.Id != id) return true;

            return false;
        }

        public bool IsBranchExist(string name, string id, string tenantId)
        {
            var branch = _repository.AsNoTracking().Where(x => x.TenantId == tenantId)
                .FirstOrDefault(x => x.Name == name);
            if (branch != null && branch.Id != id) return true;

            return false;
        }

        public bool IsReachedMaximumBranchCount(string tenantId, int featureBranchCount)
        {
            var count = _repository.AsNoTracking().Count(x => x.TenantId == tenantId);
            return count >= featureBranchCount;
        }
    }
}