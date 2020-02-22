using System;
using Project.Core.Enums;
using Project.Model.EntityBase;
using Project.Model.EntityBases;

namespace Project.Model
{
    public class MonthlyEmailBalance : HaveTenantIdCompanyIdBranchIdEntityBase
    {
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