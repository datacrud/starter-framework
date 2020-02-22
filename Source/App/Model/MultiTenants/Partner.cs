using System;
using Project.Core.Enums;
using Project.Model.EntityBase;
using Project.Model.EntityBases;

namespace Project.Model
{
    public class Partner : HaveTenantIdCompanyIdBranchIdEntityBase
    {
        public string Name { get; set; }
        public PartnershipType Type { get; set; }
        public DateTime PartnerFrom { get; set; }
        public double PrimaryContribution { get; set; }
        public string Importance { get; set; }
        public string Note { get; set; }
        public double CurrentContribution { get; set; }


        //ignore properties
        public bool IsHostAction { get; set; }
    }
}