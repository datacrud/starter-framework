using System.ComponentModel;

namespace Project.Core.Enums
{
    public enum PartnershipType
    {
        General = 1,
        Working = 2,
        Sleeping = 3,
        Nominal = 4,
        Estoppel = 5,
        Limited = 6,

        [Description("Limited Liability")]
        LimitedLiability = 7,

        Secret = 8,

        [Description("Holding Out")]
        HoldingOut = 9,

        Sub = 10,
        Profit = 11,        
        Forming = 12,        
        Other =100,
    }
}