using System;
using System.Linq;
using Itenso.TimePeriod;

namespace FBOLinx.Core.Utilities.DatesAndTimes
{
    public class DateTimeHelper
    {
        public static System.DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new System.DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        public static string MinutesToTimeString(int inTime)
        {
            string outTime;
            int hrs = Convert.ToInt32(Math.Floor(Convert.ToDouble(inTime) / 60));
            int min = Convert.ToInt32(Convert.ToDouble(inTime) - (Convert.ToDouble(hrs) * 60));
            string outHrs = String.Format("{0:00}", hrs);
            string outMin = String.Format("{0:00}", min);
            outTime = String.Format("{0}:{1}", outHrs, outMin);
            if (outTime == "24:00")
                outTime = "00:00";
            return outTime;
        }

        public static int DateTimeToMinutesOfDay(DateTime dt1)
        {
            int outTime = 0;
            int hrs = Convert.ToInt32(dt1.Hour) * 60;
            int mins = Convert.ToInt32(dt1.Minute);
            outTime = hrs + mins;
            return outTime;
        }

        public static string JulianDaysToDateTimeString(int dayCnt)
        {
            DateTime refDate = DateTime.Parse("12/31/1899");
            DateTime fltDate = refDate.AddDays(dayCnt);
            return fltDate.ToShortDateString();
        }

        public static int DateTimeToJulianDays(DateTime fosDt)
        {
            DateTime refDate = DateTime.Parse("12/31/1899");
            // Difference in days, hours, and minutes.
            TimeSpan ts = fosDt - refDate;
            // Difference in days.
            int differenceInDays = ts.Days;
            return differenceInDays;
        }

        public static DateTime GetUtcAsUnspecifiedKind()
        {
            string comparisonDate = DateTime.UtcNow.ToString("MM-dd-yyyy HH:mm");
            return DateTime.Parse(comparisonDate);
        }

        public static DateRange GetLastQuarter()
        {
            TimeCalendar calendar = new TimeCalendar();
            Quarter quarter = new Quarter(DateTime.Today, calendar);
            Quarter pastQuarter = quarter.GetPreviousQuarter();

            return new DateRange() { StartDate = pastQuarter.Start, EndDate = pastQuarter.End };
        }

        public static DateRange GetLastMonth()
        {
            TimeCalendar calendar = new TimeCalendar();
            Month month = new Month(DateTime.Today);
            var previousMonth = month.GetPreviousMonth();

            return new DateRange() { StartDate = previousMonth.Start, EndDate = previousMonth.End };
        }

        public static bool IsDateNothing(DateTime dateTime)
        {
            if (dateTime == null || dateTime.ToShortDateString() == DateTime.MinValue.ToShortDateString() || dateTime <= DateTime.Parse("1/2/1753") || dateTime <= DateTime.Parse("1/2/1900"))
                return true;
            return false;
        }

        public static bool IsDate(string date)
        {
            DateTime dateTime;
            if (DateTime.TryParse(date, out dateTime))
                return true;
            string expectedFormat = "yyyy-MM-dd";
            bool result = DateTime.TryParseExact(
                date,
                expectedFormat,
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None,
                out dateTime);
            if (result)
                return true;
            return false;
        }

        public static DateTime DatabaseDateTimeMinValue()
        {
            return DateTime.Parse("1/1/1753");
        }

        public static bool IsWithinHourDifference(DateTime? dateTime1, DateTime? dateTime2, int hourDifference)
        {
            return (dateTime1 != null && dateTime2 != null &&
                    Math.Abs((dateTime1.Value - dateTime2.Value).TotalHours) <= hourDifference);
        }

        public static DateTime GetLocalTime(DateTime utcDateTime, double? intlTimeZone, bool respectDaylightSavings)
        {
            TimeZoneInfo tst = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");
            bool isDaylightSavingTime = tst.IsDaylightSavingTime(utcDateTime);
            DateTime localTime = utcDateTime.AddHours(intlTimeZone.GetValueOrDefault());

            if (isDaylightSavingTime && respectDaylightSavings)
            {
                localTime = localTime.AddHours(1);
            }
            return DateTime.SpecifyKind(localTime, DateTimeKind.Unspecified);
        }

        public static DateTime GetUtcTime(DateTime localDateTime, double? intlTimeZone, bool respectDaylightSavings)
        {
            TimeZoneInfo tst = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");
            bool isDaylightSavingTime = tst.IsDaylightSavingTime(localDateTime);
            DateTime utcTime = localDateTime.AddHours(-intlTimeZone.GetValueOrDefault());

            if (isDaylightSavingTime && respectDaylightSavings)
            {
                utcTime = utcTime.AddHours(-1);
            }
            return DateTime.SpecifyKind(utcTime, DateTimeKind.Unspecified);
        }

        public static string GetLocalTimeZone(DateTime utcDateTime, double? intlTimeZone, bool respectDaylightSavings)
        {
            var allTimeZones = TimeZoneInfo.GetSystemTimeZones();
            TimeZoneInfo tst = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");
            bool isDaylightSavingTime = tst.IsDaylightSavingTime(utcDateTime);
            var offSet = (int)intlTimeZone.GetValueOrDefault();

            if (isDaylightSavingTime && respectDaylightSavings)
            {
                offSet = offSet + 1;
            }

            var timeZone = allTimeZones.FirstOrDefault(x => x.BaseUtcOffset == new TimeSpan(offSet, 0, 0) && x.DisplayName.Contains("(US & Canada)"));
            DateTime localTime = utcDateTime.AddHours(intlTimeZone.GetValueOrDefault());

            if (isDaylightSavingTime)
            {
                switch(timeZone.DaylightName)
                {
                    case "Eastern Daylight Time":
                        return "EDT";
                    case "Central Daylight Time":
                            return "CDT";
                    case "Mountain Daylight Time":
                            return "MDT";
                    case "Pacific Daylight Time":
                        return "PDT";
                    default:
                        return "";
                }
            }

            switch (timeZone.StandardName)
            {
                case "Eastern Standard Time":
                    return "EST";
                case "Central Standard Time":
                    return "CST";
                case "Mountain Standard Time":
                    return "MST";
                case "Pacific Standard Time":
                    return "PST";
                default:
                    return "";
            }
        }

        public static DateTime GetLocalTimeNow(double? intlTimeZone, bool respectDaylightSavings)
        {
            return GetLocalTime(DateTime.UtcNow, intlTimeZone, respectDaylightSavings);
        }

        public static DateTime GetUtcTimeNow(double? intlTimeZone, bool respectDaylightSavings)
        {
            return GetLocalTime(DateTime.UtcNow, intlTimeZone, respectDaylightSavings);
        }

        public static string GetLocalTimeZone(double? intlTimeZone, bool respectDaylightSavings)
        {
            return GetLocalTimeZone(DateTime.UtcNow, intlTimeZone, respectDaylightSavings);
        }

        public static DateTime GetNextTuesdayDate(DateTime date)
        {
            DayOfWeek day = DayOfWeek.Tuesday;
            int daysToAdd = ((int)day - (int)date.DayOfWeek + 7) % 7;
            if (daysToAdd == 0)
                daysToAdd = 7;
            return date.AddDays(daysToAdd).AddMinutes(1);
        }

        #region Objects

        public class DateRange
        {
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
        }
        #endregion
    }
}
