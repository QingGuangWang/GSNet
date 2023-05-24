using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSNet.Common.Extensions
{
    /// <summary>
    /// 日期时间扩展方法
    /// </summary>
    public static class DateTimeExtensions
    {
        private static readonly string YMDFormat = "yyyy-MM-dd";

        private static readonly string YMDHMFormat = "yyyy-MM-dd HH:mm";

        private static readonly string YMDHMSFormat = "yyyy-MM-dd HH:mm:ss";

        /// <summary>
        /// 清除时间，只保留日期
        /// </summary>
        /// <param name="dateTime">需要处理的日期时间DateTime</param>
        /// <returns></returns>
        public static DateTime ClearTime(this DateTime dateTime)
        {
            return dateTime.Subtract(
                new TimeSpan(
                    0,
                    dateTime.Hour,
                    dateTime.Minute,
                    dateTime.Second,
                    dateTime.Millisecond
                )
            );
        }

        /// <summary>
        /// 转换为UTC时间
        /// 如果DateTimeKind 是 Unspecified， 则直接设置为UTC
        /// </summary>
        public static DateTime ConvertToUtc(this DateTime datetime)
        {
            if (datetime.Kind == DateTimeKind.Unspecified)
            {
                datetime = new DateTime(datetime.Year, datetime.Month, datetime.Day,
                    datetime.Hour, datetime.Minute, datetime.Second, datetime.Millisecond, DateTimeKind.Utc);
            }

            return datetime.ToUniversalTime();
        }

        /// <summary>
        /// 转换为UTC时间
        /// 如果DateTimeKind 是 Unspecified， 则直接设置为UTC
        /// </summary>
        /// <param name="datetime">要转换处理的时间</param>
        /// <returns>转换处理后的时间</returns>
        public static DateTime? ConvertToUtc(this DateTime? datetime)
        {
            return datetime?.ConvertToUtc();
        }

        /// <summary>
        /// 将日期时间格式化为 yyyy-MM-dd 的字符串形式
        /// </summary>
        /// <param name="date">需要格式化输出的日期时间</param>
        /// <returns>格式化后输出的日期字符串</returns>
        public static string ToStringWithYMDFormat(this DateTime date)
        {
            return date.ToString(YMDFormat);
        }

        /// <summary>
        /// 将日期时间格式化为 yyyy-MM-dd HH:mm 的字符串形式
        /// </summary>
        /// <param name="date">需要格式化输出的日期时间</param>
        /// <returns>格式化后输出的日期字符串</returns>
        public static string ToStringWithYMDHMFormat(this DateTime date)
        {
            return date.ToString(YMDHMFormat);
        }

        /// <summary>
        /// 将日期时间格式化为 yyyy-MM-dd HH:mm:ss 的字符串形式
        /// </summary>
        /// <param name="date">需要格式化输出的日期时间</param>
        /// <returns>格式化后输出的日期字符串</returns>
        public static string ToStringWithYMDHMSFormat(this DateTime date)
        {
            return date.ToString(YMDHMSFormat);
        }
    }
}
