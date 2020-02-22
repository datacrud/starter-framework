using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project.Model;
using Project.Model.Repositories;

namespace Project.Repository
{
    public interface ILoginAttemptRepository : IHaveTenantIdCompanyIdBranchIdRepositoryBase<LoginAttempt>
    {

    }
    public class LoginAttemptRepository : HaveTenantIdCompanyIdBranchIdRepositoryBase<LoginAttempt>, ILoginAttemptRepository
    {
        public LoginAttemptRepository(BusinessDbContext db) : base(db)
        {
        }


    }
}