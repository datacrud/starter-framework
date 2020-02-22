using System;
using System.ComponentModel.DataAnnotations.Schema;
using Project.Core.Enums;
using Project.Model.EntityBase;
using Project.Model.EntityBases;

namespace Project.Model
{
    public class SubscriptionPayment : HaveTenantIdCompanyIdEntityBase
    {
        public string SubscriptionId { get; set; }
        [ForeignKey("SubscriptionId")]
        public virtual Subscription Subscription { get; set; }

        public DateTime? Date { get; set; }
        public SubscriptionPaymentMethod PaymentMethod { get; set; }
        public double SubscriptionCharge { get; set; }
        public double ServiceCharge { get; set; }
        public double AmountToBePaid { get; set; }
        public double PaidAmount { get; set; }
        public string TransectionNumber { get; set; }
        public string VerificationCode { get; set; }
        public string PaymentReferance { get; set; }
        public SubscriptionPaymentStatus PaymentStatus { get; set; }

        //public string BkashOrRocketNumber { get; set; }
    }
}
