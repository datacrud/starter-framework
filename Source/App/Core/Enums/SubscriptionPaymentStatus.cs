using System.ComponentModel;

namespace Project.Core.Enums
{
    public enum SubscriptionPaymentStatus
    {
        Unpaid = 0,

        Pending = 1,

        [Description("Awaiting Confirmation")]
        AwaitingConfirmation = 2,

        Paid = 3,

        Rejected = 10,
    }
}