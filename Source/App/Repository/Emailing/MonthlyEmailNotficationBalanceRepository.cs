using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project.Model;
using Project.Model.Repositories;

namespace Project.Repository
{
    public interface IMonthlyEmailNotficationBalanceRepository : IHaveTenantIdCompanyIdBranchIdRepositoryBase<MonthlyEmailBalance>
    {
        
    }
    public class MonthlyEmailNotficationBalanceRepository : HaveTenantIdCompanyIdBranchIdRepositoryBase<MonthlyEmailBalance>, IMonthlyEmailNotficationBalanceRepository
    {
        public MonthlyEmailNotficationBalanceRepository(BusinessDbContext db) : base(db)
        {
        }
    }
}
