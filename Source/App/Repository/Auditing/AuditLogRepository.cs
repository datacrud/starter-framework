using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project.Model;
using Project.Model.Repositories;

namespace Project.Repository
{
    public interface IAuditLogRepository : IHaveTenantIdCompanyIdBranchIdRepositoryBase<AuditLog>
    {
        
    }
    public class AuditLogRepository : HaveTenantIdCompanyIdBranchIdRepositoryBase<AuditLog>, IAuditLogRepository
    {
        public AuditLogRepository(BusinessDbContext db) : base(db)
        {
        }
    }
}
