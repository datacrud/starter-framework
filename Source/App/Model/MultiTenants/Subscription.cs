using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Project.Core.Enums;
using Project.Model.EntityBase;
using Project.Model.EntityBases;

namespace Project.Model
{
    public class Subscription : HaveTenantIdCompanyIdEntityBase
    {

        public string TenantName { get; set; }

        public string EditionId { get; set; }
        [ForeignKey("EditionId")]
        public virtual Edition Edition { get; set; }

        public SubscriptionPackage Package { get; set; }

        public double PackageMonthlyPrice { get; set; }
        public int PackageMonth { get; set; }
        public int NoOfShowroom { get; set; }
        public double Price { get; set; } //Subtotal
        public double PackageDiscountPercentage { get; set; }
        public double PackageDiscountAmount { get; set; }
        public double PackageCharge { get; set; }

        public DateTime? ExpireOn { get; set; }
        public DateTime? RenewedOn { get; set; }

        public SubscriptionStatus Status { get; set; }

        public SubscriptionPaymentStatus PaymentStatus { get; set; }
        public bool IsPaymentCompleted { get; set; }

        public virtual ICollection<SubscriptionPayment> Payments { get; set; }
    }
}