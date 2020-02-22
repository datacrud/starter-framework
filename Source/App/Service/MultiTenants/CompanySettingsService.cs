using System;
using System.Linq;
using Project.Model;
using Project.Repository;
using Project.Repository.MultiTenants;
using Project.Service.Bases;
using Project.ViewModel;

namespace Project.Service.MultiTenants
{
    public interface ICompanySettingsService : IHaveTenantIdCompanyIdServiceBase<CompanySetting, CompanySettingsViewModel>
    {
        CompanySettingsViewModel GetByCompanyId(string companyId);
    }

    public class CompanySettingsService : HaveTenantIdCompanyIdServiceBase<CompanySetting, CompanySettingsViewModel>, ICompanySettingsService
    {
        private readonly ICompanySettingsRepository _repository;
        private readonly ICompanyRepository _companyRepository;
        private readonly ITenantManager _tenantManager;

        public CompanySettingsService(ICompanySettingsRepository repository,
            ICompanyRepository companyRepository,
            ITenantManager tenantManager) : base(repository)
        {
            _repository = repository;
            _companyRepository = companyRepository;
            _tenantManager = tenantManager;
        }

        public CompanySettingsViewModel GetByCompanyId(string companyId)
        {
            var companySettings = _repository.GetAll().FirstOrDefault(x => x.CompanyId == companyId);
            if (companySettings == null)
            {
                var company = _companyRepository.GetById(companyId);

                var tenantCompanySettingsId = _tenantManager.CreateTenantCompanySettings(company.TenantId, company);

                companySettings = _repository.GetById(tenantCompanySettingsId);
            }

            var viewModel = (CompanySettingsViewModel)Activator.CreateInstance(typeof(CompanySettingsViewModel),
                companySettings);

            return viewModel;
        }


    }
}
