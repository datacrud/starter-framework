using System;
using Project.Model.EntityBase;
using Project.Model.EntityBases;

namespace Project.Model
{
    public class FiscalYear : HaveTenantIdCompanyIdBranchIdEntityBase
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public bool IsHostAction { get; set; }
    }
}