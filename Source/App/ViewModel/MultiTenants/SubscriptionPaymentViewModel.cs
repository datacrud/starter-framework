using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations.Model;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project.Core.Enums;
using Project.Core.Extensions;
using Project.Model;
using Project.ViewModel.Bases;

namespace Project.ViewModel
{
    public class SubscriptionPaymentViewModel : HaveTenantIdCompanyIdViewModelBase<SubscriptionPayment>
    {
        public SubscriptionPaymentViewModel(SubscriptionPayment model) : base(model)
        {

            SubscriptionId = model.SubscriptionId;
            if (model.Subscription != null)
            {
                Subscription = new SubscriptionViewModel(model.Subscription);
            }
            Date = model.Date;
            PaymentMethod = model.PaymentMethod;
            PaymentMethodName = model.PaymentMethod.GetDescription();
            SubscriptionCharge = model.SubscriptionCharge;
            ServiceCharge = model.ServiceCharge;
            AmountToBePaid = model.AmountToBePaid;            
            PaidAmount = model.PaidAmount;
            TransectionNumber = model.TransectionNumber;
            VerificationCode = model.VerificationCode;
            PaymentReferance = model.PaymentReferance;
            PaymentStatus = model.PaymentStatus;
            PaymentStatusName = model.PaymentStatus.GetDescription();

            TenantName = model.Tenant?.Name;
        }

        public string TenantName { get; set; }
        public string SubscriptionId { get; set; }
        public SubscriptionViewModel Subscription { get; set; }
        public DateTime? Date { get; set; }
        public SubscriptionPaymentMethod PaymentMethod { get; set; }
        public string PaymentMethodName { get; set; }
        public double SubscriptionCharge { get; set; }
        public double ServiceCharge { get; set; }
        public double AmountToBePaid { get; set; }
        public double PaidAmount { get; set; }
        public string TransectionNumber { get; set; }
        public string VerificationCode { get; set; }
        public string PaymentReferance { get; set; }
        public SubscriptionPaymentStatus PaymentStatus { get; set; }
        public string PaymentStatusName { get; set; }

        public string BkashOrRocketNumber { get; set; }
    }
}
