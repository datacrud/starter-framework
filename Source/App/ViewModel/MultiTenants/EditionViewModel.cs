using System;
using System.Collections.Generic;
using System.Linq;
using Project.Core.Enums;
using Project.Model;
using Project.ViewModel.Bases;

namespace Project.ViewModel
{
    public class EditionViewModel : ViewModelBase<Edition>
    {
        public EditionViewModel(Edition model) : base(model)
        {
            IsActive = model.IsActive;

            DisplayName = model.DisplayName;
            Name = model.Name;
            Descriminator = model.Descriminator;

            DiscountType = model.DiscountType;
            AnnualPriceDiscountAmount = model.AnnualPriceDiscountAmount;
            AnnualPriceDiscountPercentage = model.AnnualPriceDiscountPercentage;
            HalfYearlyPriceDiscountAmount = model.HalfYearlyPriceDiscountAmount;
            HalfYearlyPriceDiscountPercentage = model.HalfYearlyPriceDiscountPercentage;
            QuarterPriceDiscountAmount = model.QuarterPriceDiscountAmount;
            QuarterPriceDiscountPercentage = model.QuarterPriceDiscountPercentage;
            MonthlyPriceDiscountAmount = model.MonthlyPriceDiscountAmount;
            MonthlyPriceDiscountPercentage = model.MonthlyPriceDiscountPercentage;

            MinimumNoOfShowroom = model.MinimumNoOfShowroom;
            MaximumNoOfShowroom = model.MaximumNoOfShowroom;

            AnnualPrice = model.AnnualPrice;
            HalfYearlyPrice = model.HalfYearlyPrice;
            QuarterPrice = model.QuarterPrice;
            MonthlyPrice = model.MonthlyPrice;
            TrialDayCount = model.TrialDayCount;
            WaitingDayAfterExpire = model.WaitingDayAfterExpire;
            Order = model.Order.GetValueOrDefault();
            if (model.Features != null)
            {
                Features = model.Features.Where(x=> x.Active && !x.IsDeleted).OrderBy(x => x.Order).ToList().ConvertAll(x => new FeatureViewModel(x));
            }
        }

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

        public int Order { get; set; }

        public List<FeatureViewModel> Features { get; set; }
    }
}