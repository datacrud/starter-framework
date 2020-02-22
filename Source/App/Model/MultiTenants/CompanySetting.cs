using System;
using System.ComponentModel.DataAnnotations.Schema;
using Project.Model.EntityBase;

namespace Project.Model
{
    public class CompanySetting : HaveTenantIdCompanyIdEntityBase
    {
        public string BinNumber { get; set; }
        public string TinNumber { get; set; }
        public string CustomerInvoiceTermsAndConditions { get; set; }
        public double SaleVatPercentage { get; set; }
        public string DefineLowStockQuantity { get; set; }

        public bool IsUseDefaultSettings { get; set; }
        public string EmailSenderDisplayName { get; set; }
        public string NotificationSenderEmail { get; set; }


        #region Email Settings

        public bool IsEnableEmailNotification { get; set; }

        #region Admin Emails

        public bool IsSendEmailToAdminForTransaction { get; set; }

        #endregion

        #region Customer Emails

        public bool IsSendEmailToCustomerForOrders { get; set; }
        public bool IsSendEmailToCustomerForDelivery { get; set; }
        public bool IsSendEmailToCustomerForTransaction { get; set; }

        #endregion

        #endregion


        #region Sms Settings

        public bool IsEnableSmsNotification { get; set; }
        
        #region Admin Emails

        public bool IsSendSmsToAdminForTransaction { get; set; }

        #endregion

        #region Customer Sms

        public bool IsSendSmsToCustomerForOrders { get; set; }
        public bool IsSendSmsToCustomerForDelivery { get; set; }
        public bool IsSendSmsToCustomerForTransaction { get; set; }

        #endregion

        #endregion



        #region Host Settings For Company

        public bool EnableStockLessFeatures { get; set; }


        public string PoweredBy { get; set; }

        public bool HasYearlyHostingCharge { get; set; }
        public double YearlyHostingChargeAmount { get; set; }
        public DateTime? HostingValidTill { get; set; }

        #endregion


        //ignore properties
        public bool IsHostAction { get; set; }
    }
}