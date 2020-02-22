using System.ComponentModel;

namespace Project.Core.Enums
{
    public enum SubscriptionStatus
    {
        [Description("Awaiting Payment")]
        AwaitingPayment = 0,

        [Description("Trial")]
        Trial = 1,

        [Description("Active")]
        Active = 2,

        [Description("Expired")]
        Expired = 3,

        [Description("Inactive")]
        Inactive = 10,

        [Description("Locked")]
        Locked = 11,
    }
}