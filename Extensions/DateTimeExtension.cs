using System;

namespace Tmc.Util
{
    public static class DateTimeExtension
    {
        /// <summary>
        /// 返回yyyy-MM-dd格式的日期，如果为null或default,则返回string.Empty
        /// </summary>
        public static string ToDateString(this DateTime? dt)
        {
            if (dt == null || dt.Value == default(DateTime)) return string.Empty;            
            return dt.Value.ToString("yyyy-MM-dd");
        }

        /// <summary>
        /// 返回yyyy-MM-dd格式的日期，如果为default,则返回string.Empty
        /// </summary>
        public static string ToDateString(this DateTime dt)
        {
            return ((DateTime?)dt).ToDateString();            
        }
        
        /// <summary>
        /// 返回yyyy-MM-dd HH:mm格式的日期和时间，如果为null或default,则返回string.Empty
        /// </summary>
        public static string ToDateTimeString(this DateTime? dt)
        {
            if (dt == null || dt.Value == default(DateTime)) return string.Empty;
            return dt.Value.ToString("yyyy-MM-dd HH:mm");
        }

        /// <summary>
        /// 返回yyyy-MM-dd HH:mm格式的日期和时间，如果为default,则返回string.Empty
        /// </summary>
        public static string ToDateTimeString(this DateTime dt)
        {
            return ((DateTime?)dt).ToDateTimeString();
        }

        /// <summary>
        /// 返回HH:mm格式的时间，如果为null或default,则返回string.Empty
        /// </summary>
        public static string ToTimeString(this DateTime? dt)
        {
            if (dt == null || dt.Value == default(DateTime)) return string.Empty;
            return dt.Value.ToString("HH:mm");
        }

        /// <summary>
        /// 返回HH:mm格式的时间，如果为default,则返回string.Empty
        /// </summary>
        public static string ToTimeString(this DateTime dt)
        {
            return ((DateTime?)dt).ToTimeString();
        }

        public static string ToDayOfWeekCn(this DateTime dt)
        {
            switch (dt.DayOfWeek)
            {
                case DayOfWeek.Monday: return "周一";
                case DayOfWeek.Tuesday: return "周二";
                case DayOfWeek.Wednesday: return "周三";
                case DayOfWeek.Thursday: return "周四";
                case DayOfWeek.Friday: return "周五";
                case DayOfWeek.Saturday: return "周六";
                case DayOfWeek.Sunday: return "周日";
                default:
                    return string.Empty;
            }
        }

        //北京时间早8个小时，减去8小时才是标准时间
        private static readonly DateTime TimestampStartTime = new DateTime(1970, 1, 1, 8, 0, 0);

        public static long ToQunarTimestamp(this DateTime dt)
        {            
            return Convert.ToInt64((dt - TimestampStartTime).TotalMilliseconds);
        }

        public static DateTime FromQunarTimestamp(long timestamp)
        {
            return TimestampStartTime.AddMilliseconds(timestamp);
        }
    }
}
