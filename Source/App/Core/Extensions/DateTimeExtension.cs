using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Core.Extensions
{
    public static class DateTimeExtension
    {
        public static DateTime EquivalentWeekDay(this DateTime dtOld)
        {
            int num = (int)dtOld.DayOfWeek;
            int num2 = (int)DateTime.Today.DayOfWeek;
            return DateTime.Today.AddDays(num - num2);
        }


        public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = (7 + (dt.DayOfWeek - startOfWeek)) % 7;
            return dt.AddDays(-1 * diff).Date;
        }

        public static DateTime StartOfMonth(this DateTime dt)
        {
            var startDate = new DateTime(dt.Year, dt.Month, 1);
            return startDate;
        }

        public static DateTime StartOfQuarter(this DateTime date)
        {
            DateTime startDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);

            if (date.Month >= 1 && date.Month <= 3) startDate = new DateTime(date.Year, 1, 1);
            else if (date.Month >= 4 && date.Month <= 6) startDate = new DateTime(date.Year, 4, 1);
            else if (date.Month >= 7 && date.Month <= 9) startDate = new DateTime(date.Year, 7, 1);
            else if (date.Month >= 10 && date.Month <= 12) startDate = new DateTime(date.Year, 10, 1);

            return startDate;
        }

        public static DateTime StartOfHalfYear(this DateTime date)
        {
            DateTime startDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);

            if (date.Month >= 1 && date.Month <= 6) startDate = new DateTime(date.Year, 1, 1);
            else if (date.Month >= 7 && date.Month <= 12) startDate = new DateTime(date.Year, 7, 1);

            return startDate;
        }

        public static DateTime StartOfYear(this DateTime date)
        {
            DateTime startDate = new DateTime(DateTime.Today.Year, 1, 1);

            return startDate;
        }
    }
}
