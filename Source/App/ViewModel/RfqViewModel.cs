using Project.Core.Enums;
using Project.Model;
using Project.ViewModel.Bases;

namespace Project.ViewModel
{
    public class RfqViewModel : ViewModelBase<Rfq>
    {
        public RfqViewModel(Rfq model): base(model)
        {
            

            CompanyName = model.CompanyName;
            YourName = model.YourName;
            EmailAddress = model.EmailAddress;
            PhoneNumber = model.PhoneNumber;
            TotalUsers = model.TotalUsers;
            TotalBranch = model.TotalBranch;
            TotalMonthlyEmailNotfication = model.TotalMonthlyEmailNotification;
            WhatINeed = model.WhatINeed;

            HowYouKnowAboutUs = model.HowYouKnowAboutUs;
            HowYouKnowAboutUsMessage = model.HowYouKnowAboutUsMessage;
            Comment = model.Comment;
            ResponseMessage = model.ResponseMessage;
            RfqStatus = model.RfqStatus;
        }

        public string CompanyName { get; set; }
        public string YourName { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }

        public int TotalUsers { get; set; }
        public int TotalBranch { get; set; }
        public int TotalMonthlyEmailNotfication { get; set; }
        public string WhatINeed { get; set; }

        public string HowYouKnowAboutUs { get; set; }
        public string HowYouKnowAboutUsMessage { get; set; }
        public string Comment { get; set; }
        public string ResponseMessage { get; set; }

        public RfqStatus RfqStatus { get; set; }
    }
}