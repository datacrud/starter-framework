using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project.Core.Enums;

namespace Project.BindingModel.BackgroundJobs
{
    public class BackgroundJobInput
    {
        public SchedulingPeriod? Period { get; set; }
        public DateTime? Date { get; set; }
        public string TenantId { get; set; }
        public string CompanyId { get; set; }


        public string BranchId { get; set; }
        public string SupplierId { get; set; }
        public string CustomerId { get; set; }


        public void SetDate()
        {
            if (!Period.HasValue) return;

            switch (Period)
            {
                case SchedulingPeriod.Today:
                    Date = DateTime.Today;
                    break;
                case SchedulingPeriod.Week:
                    Date = DateTime.Today.AddDays(-7);
                    break;
                case SchedulingPeriod.TwoWeeks:
                    Date = DateTime.Today.AddDays(-14);
                    break;
                case SchedulingPeriod.Month:
                    Date = DateTime.Today.AddMonths(-1);
                    break;
                case SchedulingPeriod.TwoMonths:
                    Date = DateTime.Today.AddMonths(-2);
                    break;
                case SchedulingPeriod.Quarter:
                    Date = DateTime.Today.AddMonths(-3);
                    break;
                case SchedulingPeriod.FourMonths:
                    Date = DateTime.Today.AddMonths(-4);
                    break;
                case SchedulingPeriod.FiveMonths:
                    Date = DateTime.Today.AddMonths(-5);
                    break;
                case SchedulingPeriod.HalfYear:
                    Date = DateTime.Today.AddMonths(-6);
                    break;
                case SchedulingPeriod.SevenMonths:
                    Date = DateTime.Today.AddMonths(-7);
                    break;
                case SchedulingPeriod.EightMonths:
                    Date = DateTime.Today.AddMonths(-8);
                    break;
                case SchedulingPeriod.ThreeQuarter:
                    Date = DateTime.Today.AddMonths(-9);
                    break;
                case SchedulingPeriod.TenMonths:
                    Date = DateTime.Today.AddMonths(-10);
                    break;
                case SchedulingPeriod.ElevenMonths:
                    Date = DateTime.Today.AddMonths(-11);
                    break;
                case SchedulingPeriod.Year:
                    Date = DateTime.Today.AddYears(-1);
                    break;
                case SchedulingPeriod.LifeTime:
                    Date = null;
                    break;
                case null:
                    Date = null;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
