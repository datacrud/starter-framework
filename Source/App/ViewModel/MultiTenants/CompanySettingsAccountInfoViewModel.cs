using System;
using Project.Model;

namespace Project.ViewModel
{
    public class CompanySettingsAccountInfoViewModel
    {

        public CompanySettingsAccountInfoViewModel(CompanySetting model)
        {
            PoweredBy = model.PoweredBy;
            HasYearlyHostingCharge = model.HasYearlyHostingCharge;
            HostingValidTill = model.HostingValidTill;
        }



        #region Host Settings For Company

        public string PoweredBy { get; set; }

        public bool HasYearlyHostingCharge { get; set; }
        public DateTime? HostingValidTill { get; set; }

        #endregion
    }
}