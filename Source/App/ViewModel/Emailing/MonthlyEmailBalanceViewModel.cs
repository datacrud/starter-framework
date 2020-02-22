using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project.Core.Enums;
using Project.Model;
using Project.ViewModel.Bases;

namespace Project.ViewModel
{
    public class MonthlyEmailBalanceViewModel : HaveTenantIdCompanyIdBranchIdViewModelBase<MonthlyEmailBalance>
    {
        public MonthlyEmailBalanceViewModel(MonthlyEmailBalance model) : base(model)
        {
           

            EditionId = model.EditionId;
            MonthStartDate = model.MonthStartDate;
            MonthEndDate = model.MonthEndDate;
            Month = model.Month;
            TotalSubscribeEmail = model.TotalSubscribeEmail;
            TotalSendEmail = model.TotalSendEmail;
            TotalRemainingEmail = model.TotalRemainingEmail;
            CarryForwardedEmailFromLastMonth = model.CarryForwardedEmailFromLastMonth;
            IsAllowSendEmailFromCarryForward = model.IsAllowSendEmailFromCarryForward;
        }

        public string EditionId { get; set; }
        public DateTime MonthStartDate { get; set; }
        public DateTime MonthEndDate { get; set; }
        public Month Month { get; set; }
        public int TotalSubscribeEmail { get; set; }
        public int TotalSendEmail { get; set; }
        public int TotalRemainingEmail { get; set; }
        public int CarryForwardedEmailFromLastMonth { get; set; }
        public bool IsAllowSendEmailFromCarryForward { get; set; }
    }
}
