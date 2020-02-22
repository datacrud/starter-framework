using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project.Model;
using Project.Repository;
using Project.Service.Bases;
using Project.ViewModel;

namespace Project.Service
{
    public interface IEmailNotificationLogService : IHaveTenantIdCompanyIdBranchIdServiceBase<EmailLog, EmailNotificationLogViewModel>
    {
        
    }

    public class EmailNotificationLogService : HaveTenantIdCompanyIdBranchIdServiceBase<EmailLog, EmailNotificationLogViewModel>, IEmailNotificationLogService
    {
        private readonly IEmailNotificationLogRepository _repository;

        public EmailNotificationLogService(IEmailNotificationLogRepository repository) : base(repository)
        {
            _repository = repository;
        }
    }
}
