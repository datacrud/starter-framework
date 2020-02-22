using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Project.Core.Enums;
using Project.Model.EntityBase;
using Project.Model.EntityBases;

namespace Project.Model
{
    public class Tenant : BusinessEntityBase
    {
        public string EditionId { get; set; }
        [ForeignKey("EditionId")]
        public virtual Edition Edition { get; set; }

        public bool IsActive { get; set; }

        public string ConnectionString { get; set; }
        public string TenancyName { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Url { get; set; }

        public string LogoId { get; set; }
        public string LogoFilePath { get; set; }


        public int NoOfShowroom { get; set; }
        public bool IsInTrialPeriod { get; set; }


        public string SubscriptionId { get; set; }
        public SubscriptionPackage Package { get; set; }
        public DateTime? SubscriptionEndTime { get; set; }

        public int BonusUserAccessCount { get; set; }

        public int SpecialMonthlyDiscountPercentage { get; set; }
        public int SpecialQuarterDiscountPercentage { get; set; }
        public int SpecialHalfYearlyDiscountPercentage { get; set; }
        public int SpecialAnnualDiscountPercentage { get; set; }



        public BusinessType? BusinessType { get; set; }

        public bool IsDemo { get; set; }
    }
}
