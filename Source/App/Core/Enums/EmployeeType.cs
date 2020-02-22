using System.ComponentModel;

namespace Project.Core.Enums
{
    public enum EmployeeType
    {
        All = 0,
        [Description("Full time")]
        FullTime= 1,

        [Description("Part time")]
        PartTime = 2,
        Labor = 3,

        [Description("Night gard")]
        NightGurd = 4
    }
}