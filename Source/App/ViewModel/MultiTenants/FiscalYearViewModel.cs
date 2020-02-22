using System;
using Project.Model;
using Project.ViewModel.Bases;

namespace Project.ViewModel
{
    public class FiscalYearViewModel : HaveTenantIdCompanyIdBranchIdViewModelBase<FiscalYear>
    {
        public FiscalYearViewModel(FiscalYear model) : base(model)
        {

            StartDate = model.StartDate;
            EndDate = model.EndDate;
        }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}