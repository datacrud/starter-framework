using System.ComponentModel;

namespace Project.Core.Enums
{
    public enum BranchType
    {        
        All = 0,

        [Description("Head Office")]
        HeadOffice = 1,

        [Description("Purchase Order Point")]
        PurchaseOrderPoint = 2,

        [Description("Purchase Receive Point")]
        PurchaseReceivePoint = 3,

        [Description("Production Point")]
        ProductionPoint = 4,

        [Description("Sale Point")]
        SalePoint = 5,

        [Description("Delivery Point")]
        DeliveryPoint = 6,        
    }
}