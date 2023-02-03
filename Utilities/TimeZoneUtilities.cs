using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    public class TimeZoneUtilities
    {
        public static List<TimeZoneModel> GetTimeZone()
        {
            var timezones = new List<TimeZoneModel>();
            foreach (var item in TimeZoneInfo.GetSystemTimeZones())
            {
                timezones.Add(
                    new TimeZoneModel
                    {
                        name = item.DisplayName,
                        value = item.BaseUtcOffset.Hours
                    });
            }
            return timezones;
        }
        public class TimeZoneModel
        {
            public string name { get; set; }
            public double value { get; set; }
        }
        public static DateTime ConvertTimestampToDatetime(double timestamp)
        {
            timestamp = timestamp / 1000;
            // First make a System.DateTime equivalent to the UNIX Epoch.-
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            // Add the number of seconds in UNIX timestamp to be converted.
            dateTime = dateTime.AddSeconds(timestamp);
            return dateTime;
        }
    }
}
