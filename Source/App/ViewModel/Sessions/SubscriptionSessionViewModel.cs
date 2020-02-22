using System;
using Project.Core.Enums;
using Project.Core.Extensions;

namespace Project.ViewModel.Session
{
    public class SubscriptionSessionViewModel
    {
        public string SubscriptionId { get; set; }

        public bool IsInTrialPeriod { get; set; }

        public int NoOfShowroom { get; set; }
        public DateTime? SubscriptionEndTime { get; set; }
        public int SubscriptionRemainingDays => (SubscriptionEndTime.GetValueOrDefault().Date - DateTime.Now.Date).Days;
        public int WaitingDayAfterExpire { get; set; }
        public DateTime? WaitingDayAfterExpireEndTime => SubscriptionEndTime?.AddDays(WaitingDayAfterExpire);

        public bool HasAnySubscription { get; set; }

        public SubscriptionPackage? Package { get; set; }
        public bool IsLifeTimePackage => Package.HasValue && Package == SubscriptionPackage.LifeTime; 
        public string PackageName => Package?.GetDescription();
        public SubscriptionStatus? Status { get; set; }
        public string StatusName => Status?.GetDescription();
        public SubscriptionPaymentStatus? PaymentStatus { get; set; }
        public string PaymentStatusName => PaymentStatus?.GetDescription();

        public bool IsSubscriptionExpired => HasAnySubscription && Package.HasValue && Package!= SubscriptionPackage.LifeTime &&
                                             SubscriptionEndTime.HasValue && SubscriptionEndTime < DateTime.Now.Date;
        public bool IsWaitingDayExpired =>
            WaitingDayAfterExpireEndTime.HasValue && WaitingDayAfterExpireEndTime < DateTime.Now.Date;

        public bool IsAwaitingForPayment { get; set; }
        public bool IsAwaitingForActivation { get; set; }
        public bool IsSubscriptionActivated { get; set; }

        public bool HasHostingAgreement { get; set; }
        public DateTime? HostingExpireDate { get; set; }

        public bool IsHostingExpired =>
            HasHostingAgreement && HostingExpireDate.HasValue && HostingExpireDate < DateTime.Today;
    }
}