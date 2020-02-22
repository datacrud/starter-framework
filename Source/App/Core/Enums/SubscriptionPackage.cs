using System.ComponentModel;

namespace Project.Core.Enums
{
    public enum SubscriptionPackage
    {
        Trial = 0,
        Monthly = 1,
        Annual = 2,
        Quarter = 3,

        [Description("Half Yearly")]
        HalfYearly = 4,

        [Description("Life Time")]
        LifeTime = 10
    }

    public enum SubscriptionDiscountType
    {
        FixedAmount = 1,
        Percentage = 2,
    }
}