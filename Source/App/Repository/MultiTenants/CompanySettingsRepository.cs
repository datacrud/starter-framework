using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project.Core.Session;
using Project.Model;
using Project.Model.Repositories;

namespace Project.Repository
{
    public interface ICompanySettingsRepository : IHaveTenantIdCompanyIdRepositoryBase<CompanySetting>
    {

    }
    public class CompanySettingsRepository : HaveTenantIdCompanyIdRepositoryBase<CompanySetting>, ICompanySettingsRepository
    {
        public CompanySettingsRepository(BusinessDbContext db) : base(db)
        {

        }

    }
}