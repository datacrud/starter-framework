using System;
using Project.Core.Extensions;
using Project.Model;
using Project.ViewModel.Bases;

namespace Project.ViewModel
{
    public class CompanySettingsViewModel : HaveTenantIdCompanyIdViewModelBase<CompanySetting>
    {
        public CompanySettingsViewModel(CompanySetting model): base(model)
        {
            CustomerInvoiceTermsAndConditions = model.CustomerInvoiceTermsAndConditions;
            SaleVatPercentage = model.SaleVatPercentage;
            DefineLowStockQuantity = model.DefineLowStockQuantity;

            IsUseDefaultSettings = model.IsUseDefaultSettings;
            EmailSenderDisplayName = model.EmailSenderDisplayName;
            NotificationSenderEmail = model.NotificationSenderEmail;

            IsEnableEmailNotification = model.IsEnableEmailNotification;
            IsSendEmailToAdminForTransaction = model.IsSendEmailToAdminForTransaction;
            IsSendEmailToCustomerForOrders = model.IsSendEmailToCustomerForOrders;
            IsSendEmailToCustomerForDelivery = model.IsSendEmailToCustomerForDelivery;
            IsSendEmailToCustomerForTransaction = model.IsSendEmailToCustomerForTransaction;

            IsEnableSmsNotification = model.IsEnableSmsNotification;
            IsSendSmsToAdminForTransaction = model.IsSendSmsToAdminForTransaction;

            IsSendSmsToCustomerForOrders = model.IsSendSmsToCustomerForOrders;
            IsSendSmsToCustomerForDelivery = model.IsSendSmsToCustomerForDelivery;
            IsSendSmsToCustomerForTransaction = model.IsSendSmsToCustomerForTransaction;

            BinNumber = model.BinNumber;
            TinNumber = model.TinNumber;

            EnableStockLessFeatures = model.EnableStockLessFeatures;
            PoweredBy = model.PoweredBy;
            HasYearlyHostingCharge = model.HasYearlyHostingCharge;
            YearlyHostingChargeAmount = model.YearlyHostingChargeAmount;
            HostingValidTill = model.HostingValidTill;
        }

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


        public string BinNumber { get; set; }
        public string TinNumber { get; set; }


        #region Host Settings For Company

        public bool EnableStockLessFeatures { get; set; }


        public string PoweredBy { get; set; }

        public bool HasYearlyHostingCharge { get; set; }
        public double YearlyHostingChargeAmount { get; set; }
        public DateTime? HostingValidTill { get; set; }

        #endregion


    }
}