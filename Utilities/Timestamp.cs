using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    public class Timestamp
    {
        public static double UtcNow()
        {
            DateTime date = DateTime.UtcNow;
            DateTime startTime = new DateTime(1970, 1, 1, 0, 0, 0);
            double result = Math.Round(date.Subtract(startTime).TotalSeconds, 0);
            return result;
        }
        public static double Date(DateTime date)
        {
            DateTime startTime = new DateTime(1970, 1, 1, 0, 0, 0);
            double result = Math.Round(date.Subtract(startTime).TotalSeconds, 0);
            return result;
        }

        public static string DateTimeFormatISO(DateTime date)
        {
            return date.ToString("yyyyMMddTHHmm");
        }

        public static double ConvertByTimeZone(double time, double timezone)
        {
            return time + (timezone * 3600000);
        }
        public static double ConvertToUTC(double time, double timezone)
        {
            return time - (timezone * 3600000);
        }
        /// <summary>
        /// Lấy thời gian kết thúc
        /// </summary>
        /// <param name="monthNumber"></param>
        /// <returns></returns>
        public static double GetUtcNowEnd(double start, int monthNumber)
        {
            DateTime end = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(start / 1000).AddMonths(monthNumber);
            var startTime = new DateTime(1970, 1, 1, 0, 0, 0);
            var result = Math.Round(end.Subtract(startTime).TotalSeconds, 0);
            return result;
        }
        /// <summary>
        /// Lấy thời gian bởi ngày truyền vào
        /// </summary>
        /// <param name="datetime"></param>
        /// <param name="dayNumber"></param>
        /// <returns></returns>
        public static double GetUtcNowByDays(double datetime, int dayNumber)
        {
            DateTime end = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(datetime / 1000).AddMonths(dayNumber);
            var startTime = new DateTime(1970, 1, 1, 0, 0, 0);
            var result = Math.Round(end.Subtract(startTime).TotalSeconds, 0);
            return result;
        }


        public static DateTime UnixTimestampToDateTime(double unixTime)
        {
            DateTime unixStart = new DateTime(1970, 1, 1, 0, 0, 0);
            long unixTimeStampInTicks = (long)(unixTime * TimeSpan.TicksPerSecond);
            return new DateTime(unixStart.Ticks + unixTimeStampInTicks).ToLocalTime();
        }

        public class DateTimes
        {
            public double Start { get; set; }
            public double End { get; set; }
        }
    }
}
