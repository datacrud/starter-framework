using System.ComponentModel;

namespace Project.Core.Enums
{
    public enum EmailType
    {
        [Description("Purchase Order")]
        Po=1,
        [Description("Purchase Receive")]
        Pr =2,
        [Description("Purchase Return")]
        Prn =3,
        [Description("Sales Order")]
        So = 4,
        [Description("Sale Delivery")]
        Sd = 5,
        [Description("Sale Return")]
        Sr = 6,
        [Description("Credit")]
        Cr = 7,
        [Description("Debit")]
        Dr = 8
    }
}