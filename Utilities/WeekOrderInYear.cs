using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    public class WeekOrderInYear
    {

        /// <summary>
        /// Lấy số tuần trong năm
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static int GetIso8601WeekOfYear(DateTime time)
        {
            var day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(time);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
            {
                time = time.AddDays(3);
            }

            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }
    }
}
