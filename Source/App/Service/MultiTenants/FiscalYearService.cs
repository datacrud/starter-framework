using System;
using System.Linq;
using Project.Model;
using Project.Repository;
using Project.Service.Bases;
using Project.ViewModel;

namespace Project.Service
{
    public interface IFiscalYearService : IHaveTenantIdCompanyIdBranchIdServiceBase<FiscalYear, FiscalYearViewModel>
    {
        bool IsExistFiscalYear(DateTime startDate, DateTime endDate, string id, string tenantId);
        FiscalYear GetFiscalYear(DateTime startDate, DateTime endDate, string tenantId);
    }


    public class FiscalYearService : HaveTenantIdCompanyIdBranchIdServiceBase<FiscalYear, FiscalYearViewModel>, IFiscalYearService
    {
        private readonly IFiscalYearRepository _repository;

        public FiscalYearService(IFiscalYearRepository repository) : base(repository)
        {
            _repository = repository;
        }

        public bool IsExistFiscalYear(DateTime startDate, DateTime endDate, string id, string tenantId)
        {
            FiscalYear fiscalYear = GetFiscalYear(startDate, endDate, tenantId);
            if (fiscalYear != null && fiscalYear.Id != id) return true;

            return false;
        }

        public FiscalYear GetFiscalYear(DateTime startDate, DateTime endDate, string tenantId)
        {
            var fiscalYears = _repository.AsNoTracking().Where(x=> x.TenantId == tenantId).ToList();
            var fiscalYear = fiscalYears.FirstOrDefault(x =>
                x.StartDate.GetValueOrDefault().Date >= startDate.Date &&
                x.EndDate.GetValueOrDefault().Date <= endDate.Date ||
                x.StartDate.GetValueOrDefault().Date >= startDate.Date &&
                x.EndDate.GetValueOrDefault().Date <= startDate.Date ||
                x.StartDate.GetValueOrDefault().Date <= endDate.Date &&
                x.EndDate.GetValueOrDefault().Date <= endDate.Date);

            return fiscalYear;
        }
    }
}