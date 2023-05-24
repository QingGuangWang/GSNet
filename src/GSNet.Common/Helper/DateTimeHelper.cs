using GSNet.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GSNet.Common.Constant.Predefine;

namespace GSNet.Common.Helper
{
    /// <summary>
    /// 日期相关的辅助类，通过常用的方法
    /// </summary>
    public static class DateTimeHelper
    {
        /// <summary>
        /// 表示格林威治标准时间 1970 年 1 月 1 日的 00:00:00.000,格里高利历。
        /// 
        /// UNIX系统认为1970年1月1日0点是时间纪元,所以常说的UNIX时间戳是以1970年1月1日0点为计时起点时间的。
        /// </summary>
        public static DateTime January1St1970Utc = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        /// <summary>
        ///     计算两个日期之间的工作日，有可能会手工指定节假日
        ///     - 周末 (星期六和星期天/Saturdays and Sundays)
        ///     - 特别指定的节假日时间
        /// </summary>
        /// <param name="firstDay">第一个日期</param>
        /// <param name="lastDay">第二个日期， 其值需要大于等于第一个日期(参数<paramref name="firstDay"/>)</param>
        /// <param name="bankHolidays">特别指定的节假日时间</param>
        /// <returns>Number of business days during the 'span'</returns>
        public static int CountWorkingDay(DateTime firstDay, DateTime lastDay, params DateTime[] bankHolidays)
        {
            //校验日期
            firstDay = firstDay.Date;
            lastDay = lastDay.Date;
            if (firstDay > lastDay)
                throw new Exception("Incorrect last day " + lastDay);

            var span = lastDay - firstDay;
            var businessDays = span.Days + 1; //最大工作日数
            var fullWeekCount = businessDays / 7; //完整周的数量

            //找出除了整周以外的时间是否还有周末
            if (businessDays > fullWeekCount * 7)
            {
                // 这里是想知道周末是一天还是两天
                // 减去完整周后剩余的时间间隔
                var firstDayOfWeek = (int)firstDay.DayOfWeek;
                var lastDayOfWeek = (int)lastDay.DayOfWeek;
               
                if (lastDayOfWeek < firstDayOfWeek)
                {
                    lastDayOfWeek += 7;
                }

                if (firstDayOfWeek <= 6)
                {
                    if (lastDayOfWeek >= 7) // 周六和周日都在剩下的时间间隔内
                    {
                        businessDays -= 2;
                    }
                    else if (lastDayOfWeek >= 6) // 剩下的时间间隔只有星期六
                    {
                        businessDays -= 1;
                    }
                }
                else if (firstDayOfWeek <= 7 && lastDayOfWeek >= 7) // 剩下的时间间隔只有星期天
                {
                    businessDays -= 1;
                }
            }

            //减去间隔中整整一周的周末
            businessDays -= fullWeekCount + fullWeekCount;

            //减去该时间间隔内的指定假日数
            foreach (DateTime bankHoliday in bankHolidays)
            {
                var bh = bankHoliday.Date;
                if (firstDay <= bh && bh <= lastDay)
                {
                    --businessDays;
                }
            }

            return businessDays;
        }

        /// <summary>
        /// 根据工作日时效倒推统计的工作日日期
        /// </summary>
        /// <param name="statDate">统计时间</param>
        /// <param name="timeLiness">时效天数</param>
        /// <returns></returns>
        public static DateTime GetRealDay(DateTime statDate, int timeLiness)
        {
            var weeks = timeLiness / 5;
            var days = timeLiness % 5;
            var weekDiff = 7 * weeks;
            var dayDiff = days;

            var fakeDate = statDate.Date.AddDays(-weekDiff);
            for (var i = 0; i < dayDiff; i++)
            {
                fakeDate = fakeDate.AddDays(-1);

                if (fakeDate.DayOfWeek == DayOfWeek.Saturday || fakeDate.DayOfWeek == DayOfWeek.Sunday)
                {
                    dayDiff++;
                }
            }

            var realDay = fakeDate;
            return realDay;
        }

        /// <summary>
        /// 根据给定的日期时间（参数：<paramref name="dateTime"/>），获取一天的最后时间
        /// eg: 2020-10-02 23:59:59:99999
        /// </summary>
        /// <param name="dateTime">日期时间</param>
        /// <returns>表示某一天的最后时间</returns>
        public static DateTime GetDayEnd(DateTime dateTime)
        {
            return GetDayStart(dateTime).AddDays(1).AddTicks(-1);
        }

        /// <summary>
        /// 根据给定的日期时间字符串（参数：<paramref name="dateStr"/>），获取一天的最后时间
        /// eg: 2020-10-02 23:59:59:99999
        /// </summary>
        /// <param name="dateStr">日期时间</param>
        /// <returns>表示某一天的最后时间</returns>
        public static DateTime GetDayEnd(String dateStr)
        {
            return GetDayStart(dateStr).AddDays(1).AddTicks(-1);
        }

        /// <summary>
        /// 根据给定的日期时间（参数：<paramref name="dateTime"/>），获取一天的最初时间, 可以理解为只取其日期
        /// eg: 2020-10-02 00:00:00 
        /// </summary>
        /// <param name="dateTime">日期时间</param>
        /// <returns>表示某一天的最初时间</returns>
        public static DateTime GetDayStart(DateTime dateTime)
        {
            return dateTime.Date;
        }

        /// <summary>
        /// 根据给定的日期时间字符串（参数：<paramref name="dateStr"/>），获取一天的最初时间, 可以理解为只取其日期
        /// eg: 2020-10-02 00:00:00 
        /// </summary>
        /// <param name="dateStr">日期时间</param>
        /// <returns>表示某一天的最初时间</returns>
        public static DateTime GetDayStart(string dateStr)
        {
            DateTime.TryParse(dateStr, out var result);
            return result.Date;
        }

        public static double CountDay(DateTime firstDay, DateTime lastDay)
        {
            firstDay = firstDay.Date;
            lastDay = lastDay.Date;
            if (firstDay > lastDay)
            {
                throw new Exception("Incorrect last day " + lastDay);
            }

            var span = lastDay - firstDay;

            return span.TotalDays;
        }

        public static string PrintShortDate(DateTime date)
        {
            if (date == new DateTime(1900, 1, 1))
            {
                return string.Empty;
            }

            return date.ToShortDateString();
        }

        /// <summary>
        /// 计算2个时间的间隔
        /// </summary>
        /// <param name="from">起始时间（Ticks值, 如DateTime.Now.Ticks）</param>
        /// <param name="to">结束时间（Ticks值, 如DateTime.Now.Ticks）</param>
        /// <returns></returns>
        public static TimeSpan ParseDuration(long from, long to)
        {
            return TimeSpan.FromSeconds((to - from) / (double)Stopwatch.Frequency);
        }

        /// <summary>
        /// 获取下一个时间戳
        /// </summary>
        /// <param name="from">起始时间（Ticks值, 如DateTime.Now.Ticks）</param>
        /// <param name="interval">时间间隔</param>
        /// <returns></returns>
        public static long GetNextTimestamp(long from, TimeSpan interval)
        {
            return from + (long)Math.Round(interval.TotalSeconds * Stopwatch.Frequency);
        }

        public static DateTime GetFirstDayOfWeek(this DateTime date)
        {
            int difference = (int)date.DayOfWeek - (int)CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek;
            if (difference < 0)
            {
                difference = 7 + difference;
            }

            var firstDayOfWeek = date.AddDays(-difference);

            return firstDayOfWeek;
        }

        /// <summary>
        /// 解析日期时间，且转换时间为对应 UTC时间
        /// 如果给定的时间字符串(参数 <paramref name="timeStr"/>) 并不是一个标准的ISO Data格式（如: 2020-12-01T12:22:43+0800, 2020-12-01T12:22:43Z 等），
        /// 无法获取时间偏移量的，则可以通过指定的日期时间字符串所对应的时区(参数 <paramref name="timeStr"/>，默认值是中国东八区时间  <see cref="TimeZoneName.ChinaStandardTime"/> ) 来进行计算。
        /// </summary>
        /// <param name="timeStr">待转换的时间</param>
        /// <param name="defaultTimeZone">非ISO Data格式的时间时，默认时间的时区，默认值中国东八区时间(China Standard Time)</param>
        public static DateTime ParseAndConvertTimeToUtc(string timeStr, string defaultTimeZone = TimeZoneName.ChinaStandardTime)
        {
            //如果符合ISO格式的日期时间格式
            if (CommonRegex.ISODateRegex.IsMatch(timeStr))
            {
                return DateTime.Parse(timeStr).ToUniversalTime();
            }
            else
            {
                //若被赋值为空，默认中国东八区时间
                if (defaultTimeZone.IsNullOrBlank())
                {
                    defaultTimeZone = TimeZoneName.ChinaStandardTime;
                }

                //指定时区  TZConvert.GetTimeZoneInfo(defaultTimeZone);
                var timeZone = TimeZoneInfo.FindSystemTimeZoneById(defaultTimeZone);
                
                return TimeZoneInfo.ConvertTimeToUtc(DateTime.Parse(timeStr), timeZone);
            }
        }

        /// <summary>
        /// 解析日期时间，且转换时间为对应 UTC时间
        /// 如果给定的时间字符串(参数 <paramref name="timeStr"/>) 并不是一个标准的ISO Data格式（如: 2020-12-01T12:22:43+0800, 2020-12-01T12:22:43Z 等），
        /// 无法获取时间偏移量的，则可以通过指定的日期时间字符串所对应的时区(参数 <paramref name="timeStr"/>，默认值是中国东八区时间  <see cref="TimeZoneName.ChinaStandardTime"/> ) 来进行计算。
        /// </summary>
        /// <param name="timeStr">待转换的时间</param>
        /// <param name="timeZone">非ISO Data格式的时间时，默认时间的时区</param>
        public static DateTime ParseAndConvertTimeToUtc(string timeStr, TimeZoneInfo timeZone)
        {
            //如果符合ISO格式的日期时间格式
            if (CommonRegex.ISODateRegex.IsMatch(timeStr))
            {
                return DateTime.Parse(timeStr).ToUniversalTime();
            }
            else
            {
                return TimeZoneInfo.ConvertTimeToUtc(DateTime.Parse(timeStr), timeZone);
            }
        }

        /// <summary>
        /// 分割时间段
        /// </summary> 
        public static IList<Tuple<DateTime, DateTime>> SplitTimeQuantum(DateTime beginTime, DateTime endTime, double hours, bool isCloseIntervalForEnd = true)
        {
            beginTime = beginTime.ToUniversalTime();
            endTime = endTime.ToUniversalTime();

            var result = new List<Tuple<DateTime, DateTime>>();
            var isBreak = false;

            var subBeginTime = beginTime.AddHours(-1 * hours);
            var subEndTime = beginTime;

            while (!isBreak)
            {
                subBeginTime = subBeginTime.AddHours(hours);
                subEndTime = subEndTime.AddHours(hours);

                if (subEndTime >= endTime)
                {
                    subEndTime = endTime;
                    isBreak = true;
                }

                result.Add(new Tuple<DateTime, DateTime>(subBeginTime, (isCloseIntervalForEnd && !isBreak) ? subEndTime.AddTicks(-1) : subEndTime));
            }

            return result;
        }
    }

}
