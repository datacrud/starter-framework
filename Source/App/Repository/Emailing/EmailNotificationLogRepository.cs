using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project.Model;
using Project.Model.Repositories;

namespace Project.Repository
{
    public interface IEmailNotificationLogRepository : IHaveTenantIdCompanyIdBranchIdRepositoryBase<EmailLog>
    {
        
    }
    public class EmailNotificationLogRepository : HaveTenantIdCompanyIdBranchIdRepositoryBase<EmailLog>, IEmailNotificationLogRepository
    {
        public EmailNotificationLogRepository(BusinessDbContext db) : base(db)
        {
        }
    }
}
