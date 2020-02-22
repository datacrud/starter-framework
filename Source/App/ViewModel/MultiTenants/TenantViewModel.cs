using System;
using System.ComponentModel.DataAnnotations;
using Project.Core.Enums;
using Project.Core.Extensions;
using Project.Model;
using Project.ViewModel.Bases;

namespace Project.ViewModel
{
    public class TenantViewModel : ViewModelBase<Tenant>
    {
        public TenantViewModel()
        {
            
        }

        public TenantViewModel(Tenant model): base(model)
        {
            IsActive = model.IsActive;

            ConnectionString = model.ConnectionString;
            Name = model.Name;
            TenancyName = model.TenancyName;
            Url = model.Url;
            LogoId = model.LogoId;
            LogoFilePath = model.LogoFilePath;
            IsInTrialPeriod = model.IsInTrialPeriod;

            NoOfShowroom = model.NoOfShowroom;
            Package = model.Package;
            SubscriptionId = model.SubscriptionId;
            SubscriptionEndTime = model.SubscriptionEndTime;

            EditionId = model.EditionId;

            if (model.Edition != null)
            {
                Edition = new EditionViewModel(model.Edition);
            }

            BonusUserAccessCount = model.BonusUserAccessCount;

            SpecialMonthlyDiscountPercentage = model.SpecialMonthlyDiscountPercentage;
            SpecialQuarterDiscountPercentage = model.SpecialQuarterDiscountPercentage;
            SpecialHalfYearlyDiscountPercentage = model.SpecialHalfYearlyDiscountPercentage;
            SpecialAnnualDiscountPercentage = model.SpecialAnnualDiscountPercentage;

            IsDemo = model.IsDemo;
        }

        public DateTime? CreationTime { get; set; }
        public DateTime? ModificationTime { get; set; }
        public DateTime? DeletionTime { get; set; }
        public bool IsActive { get; set; }

        public string ConnectionString { get; set; }

        [Required]
        public string Name { get; set; }
        public string TenancyName { get; set; }
        public string Url { get; set; }
        public string LogoId { get; set; }
        public string LogoFilePath { get; set; }

        public int BonusUserAccessCount { get; set; }

        public int SpecialMonthlyDiscountPercentage { get; set; }
        public int SpecialQuarterDiscountPercentage { get; set; }
        public int SpecialHalfYearlyDiscountPercentage { get; set; }
        public int SpecialAnnualDiscountPercentage { get; set; }


        #region Subscription Data

        public string SubscriptionId { get; set; }

        [Required]
        public string EditionId { get; set; }
        public EditionViewModel Edition { get; set; }

        [Required]
        public SubscriptionPackage Package { get; set; }
        public string PackageName => Package.GetDescription();

        public double PackageMonthlyPrice { get; set; }
        public int PackageMonth { get; set; }
        public int NoOfShowroom { get; set; }
        public double Price { get; set; } //Subtotal
        public double PackageDiscountPercentage { get; set; }
        public double PackageDiscountAmount { get; set; }
        public double PackageCharge { get; set; }

        public SubscriptionStatus Status { get; set; }
        public DateTime? SubscriptionEndTime { get; set; }
        public bool IsInTrialPeriod { get; set; }
        public bool IsLifeTimeSubscription { get; set; }
        public bool IsDemo { get; set; }

        #endregion


        #region User Create Data

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [Required]
        public string RetypePassword { get; set; }

        #endregion
    }
}