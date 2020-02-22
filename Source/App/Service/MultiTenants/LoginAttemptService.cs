using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project.Model;
using Project.Repository;
using Project.RequestModel;
using Project.RequestModel.Bases;
using Project.Service.Bases;
using Project.ViewModel;

namespace Project.Service
{
    public interface ILoginAttemptService : IHaveTenantIdCompanyIdBranchIdServiceBase<LoginAttempt, LoginAttemptViewModel>
    {
        
    }

    public class LoginAttemptService : HaveTenantIdCompanyIdBranchIdServiceBase<LoginAttempt, LoginAttemptViewModel>, ILoginAttemptService
    {
        private readonly ILoginAttemptRepository _repository;

        public LoginAttemptService(ILoginAttemptRepository repository) : base(repository)
        {
            _repository = repository;
        }

        public override ResponseModel<LoginAttemptViewModel> GetAllAsTenant(HaveTenantIdCompanyIdBranchIdRequestModelBase<LoginAttempt> requestModel)
        {
            var queryable = GetPagingQuery(Repository.GetAllAsTenant(), requestModel);
            var entities = GetEntries(queryable);
            var response = new ResponseModel<LoginAttemptViewModel>(entities, Repository.GetAllAsTenant().Where(requestModel.GetExpression()).Count());

            return response;
        }
    }
}
