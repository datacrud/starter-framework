using System;
using System.Data.Entity;
using System.Linq;
using Project.Core.Enums;
using Project.Core.Extensions;
using Project.Model;
using Project.ViewModel.Bases;

namespace Project.ViewModel
{
    public class SubscriptionViewModel : HaveTenantIdCompanyIdViewModelBase<Subscription>
    {
        public SubscriptionViewModel(Subscription model): base(model)
        {
            EditionId = model.EditionId;
            if (model.Edition != null)
            {
                Edition = new EditionViewModel(model.Edition);
            }

            if (model.Tenant != null)
            {
                TenantName = model.Tenant?.Name;
            }

            Package = model.Package;
            PackageMonthlyPrice = model.PackageMonthlyPrice;
            PackageMonth = model.PackageMonth;
            NoOfShowroom = model.NoOfShowroom;
            Price = model.Price;
            PackageDiscountPercentage = model.PackageDiscountPercentage;
            PackageDiscountAmount = model.PackageDiscountAmount;
            PackageCharge = model.PackageCharge;

            ExpireOn = model.ExpireOn;
            RenewedOn = model.RenewedOn;

            Status = DateTime.Today > model.ExpireOn
                ? SubscriptionStatus.Expired
                : model.Status;

            PaymentStatus = model.PaymentStatus;
            IsPaymentCompleted = model.IsPaymentCompleted;
        }


        public string TenantName { get; set; }

        public string EditionId { get; set; }
        public EditionViewModel Edition { get; set; }


        public SubscriptionPackage Package { get; set; }
        public string PackageName => Package.GetDescription();

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
        public string StatusName => Status.GetDescription();

        public SubscriptionPaymentStatus PaymentStatus { get; set; }
        public string PaymentStatusName => PaymentStatus.GetDescription();
        public bool IsPaymentCompleted { get; set; }
    }
}