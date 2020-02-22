using System;
using System.Collections.Generic;
using Project.Core.Enums;
using Project.Model.EntityBase;
using Project.Model.EntityBases;

namespace Project.Model
{
    public class Edition : BusinessEntityBase
    {
        public bool IsActive { get; set; }

        public string DisplayName { get; set; }
        public string Name { get; set; }
        public string Descriminator { get; set; }

        public SubscriptionDiscountType DiscountType { get; set; }

        public double AnnualPriceDiscountAmount { get; set; }
        public double AnnualPriceDiscountPercentage { get; set; }

        public double HalfYearlyPriceDiscountAmount { get; set; }
        public double HalfYearlyPriceDiscountPercentage { get; set; }

        public double QuarterPriceDiscountAmount { get; set; }
        public double QuarterPriceDiscountPercentage { get; set; }

        public double MonthlyPriceDiscountAmount { get; set; }
        public double MonthlyPriceDiscountPercentage { get; set; }

        public int MinimumNoOfShowroom { get; set; }
        public int MaximumNoOfShowroom { get; set; }

        public double AnnualPrice { get; set; } //AnnualPricePerShowroom
        public double HalfYearlyPrice { get; set; } //HalfYearPricePerShowroom
        public double QuarterPrice { get; set; } //QuarterPricePerShowroom
        public double MonthlyPrice { get; set; } //MonthlyPricePerShowroom
        public int TrialDayCount { get; set; }
        public int WaitingDayAfterExpire { get; set; }

        public bool EnableFeatureEdit { get; set; }

        
        public virtual ICollection<Feature> Features { get; set; }
        public virtual ICollection<Subscription> Subscriptions { get; set; }
    }
}