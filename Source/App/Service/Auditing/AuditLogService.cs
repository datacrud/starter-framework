using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project.Model;
using Project.Repository;
using Project.RequestModel.Bases;
using Project.Service.Bases;
using Project.ViewModel;

namespace Project.Service
{
    public interface IAuditLogService : IHaveTenantIdCompanyIdBranchIdServiceBase<AuditLog, AuditLogViewModel>
    {
        
    }

    public class AuditLogService : HaveTenantIdCompanyIdBranchIdServiceBase<AuditLog, AuditLogViewModel>, IAuditLogService
    {
        private readonly IAuditLogRepository _repository;

        public AuditLogService(IAuditLogRepository repository) : base(repository)
        {
            _repository = repository;
        }

        public override ResponseModel<AuditLogViewModel> GetAllAsTenant(HaveTenantIdCompanyIdBranchIdRequestModelBase<AuditLog> requestModel)
        {
            var queryable = GetPagingQuery(Repository.GetAllAsTenant().Include(x=> x.Creator), requestModel);
            var entities = GetEntries(queryable);
            var response = new ResponseModel<AuditLogViewModel>(entities, Repository.GetAllAsTenant().Count());
            //
            return response;
        }
    }


}
