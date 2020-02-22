using Project.Core.Enums;
using Project.Model.EntityBase;
using Project.Model.EntityBases;

namespace Project.Model
{
    public class Rfq : BusinessEntityBase
    {
        public string CompanyName { get; set; }
        public string YourName { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }

        public int TotalUsers { get; set; }
        public int TotalBranch { get; set; }
        public int TotalMonthlyEmailNotification { get; set; }
        public string WhatINeed { get; set; }

        public string HowYouKnowAboutUs { get; set; }
        public string HowYouKnowAboutUsMessage { get; set; }
        public string Comment { get; set; }

        public string ResponseMessage { get; set; }

        public RfqStatus RfqStatus { get; set; }
    }
}