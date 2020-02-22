using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using Project.Core.Handlers;
using Project.Core.Session;
using Project.Model;
using Project.Model.Repositories;
using Project.Service;

namespace Project.Server.Controllers.Bases
{
    [Authorize]
    public abstract class BackgroundJobControllerBase: ControllerBase
    {
        protected readonly IRepository<Company> CompanyRepository;

        protected BackgroundJobControllerBase(IRepository<Company> companyRepository)
        {
            CompanyRepository = companyRepository;
        }


        public string GetTenantId(string companyId)
        {
            if (string.IsNullOrWhiteSpace(companyId))
                throw new UiFriendlyException(HttpStatusCode.NotFound, "Company not found",
                    "Company not found to register the background job");

            var company = CompanyRepository.GetAll().AsNoTracking().SingleOrDefault(x=> x.Id == companyId);

            if (company == null || string.IsNullOrWhiteSpace(company.TenantId))
                throw new UiFriendlyException(HttpStatusCode.NotFound, "Tenant not found",
                    "Tenant not found to register the background job");

            return company?.TenantId;
        }

        public async Task<string> GetTenantIdAsync(string companyId)
        {
            if (string.IsNullOrWhiteSpace(companyId))
                throw new UiFriendlyException(HttpStatusCode.NotFound, "Company not found",
                    "Company not found to register the background job");

            var company = await CompanyRepository.GetAll().AsNoTracking().SingleOrDefaultAsync(x => x.Id == companyId);

            if (company == null || string.IsNullOrWhiteSpace(company.TenantId))
                throw new UiFriendlyException(HttpStatusCode.NotFound, "Tenant not found",
                    "Tenant not found to register the background job");

            return company?.TenantId;
        }
    }
}