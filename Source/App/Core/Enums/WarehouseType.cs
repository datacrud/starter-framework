using System.ComponentModel;

namespace Project.Core.Enums
{
    public enum WarehouseType
    {
        All = 0,
        Production = 1,
        Delivery = 2,
        Stock = 3,
        Showroom = 4,

        [Description("Head Office")]
        HeadOffice = 5
    }
}