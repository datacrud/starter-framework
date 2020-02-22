using System;
using Project.Core.Enums;
using Project.Model;
using Project.ViewModel.Bases;

namespace Project.ViewModel
{
    public class PartnerViewModel : HaveTenantIdCompanyIdBranchIdViewModelBase<Partner>
    {
        public PartnerViewModel(Partner model) : base(model)
        {
           
            Name = model.Name;
            Type = model.Type;
            PartnershipType = model.Type.ToString();
            PartnerFrom = model.PartnerFrom;
            PrimaryContribution = model.PrimaryContribution;
            Importance = model.Importance;
            Note = model.Note;
        }

        public string Name { get; set; }
        public PartnershipType Type { get; set; }
        public string PartnershipType { get; set; }
        public DateTime PartnerFrom { get; set; }
        public double PrimaryContribution { get; set; }
        public string Importance { get; set; }
        public string Note { get; set; }
    }
}