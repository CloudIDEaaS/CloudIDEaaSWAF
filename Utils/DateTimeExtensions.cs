using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace Utils
{
    public static class DateTimeExtensions
    {
        [DllImport("Kernel32.dll", SetLastError = true)]
        private static unsafe extern bool QueryPerformanceCounter(long* lpPerformanceCount);

        public static long HighResolutionPerformanceCount
        {
            get
            {
                long performanceCount = 0;

                unsafe
                {
                    QueryPerformanceCounter(&performanceCount);
                }

                return performanceCount;
            }
        }

        public static TimeSpan ToTimeSpan(this DateTime dateTime)
        {
            return TimeSpan.FromTicks(dateTime.Ticks);
        }

        public static TimeSpan ToTimeSpan(this DateTimeOffset dateTimeOffset)
        {
            return TimeSpan.FromTicks(dateTimeOffset.Ticks);
        }

        public static TimeSpan ToDateTimeSpan(string dateTimeString)
        {
            var regex = new Regex(@"(?<M>\d{2})/(?<d>\d{2})/(?<y>\d{4}) (?<h>\d{2}):(?<m>\d{2}):(?<s>\d{2})");

            if (!regex.IsMatch(dateTimeString))
            {
                throw new ArgumentException("Invalid dateTimeString", nameof(dateTimeString));
            }
            else
            {
                var dateTimeStart = DateTime.MinValue;
                var match = regex.Match(dateTimeString);
                var months = int.Parse(match.GetGroupValue("M"));
                var days = int.Parse(match.GetGroupValue("d"));
                var years = int.Parse(match.GetGroupValue("y"));
                var hours = int.Parse(match.GetGroupValue("h"));
                var minutes = int.Parse(match.GetGroupValue("m"));
                var seconds = int.Parse(match.GetGroupValue("s"));

                if (months > 0)
                {
                    dateTimeStart = dateTimeStart.AddMonths(months);
                }

                if (days > 0)
                {
                    dateTimeStart = dateTimeStart.AddDays(days);
                }

                if (years > 0)
                {
                    dateTimeStart = dateTimeStart.AddYears(years);
                }

                if (hours > 0)
                {
                    dateTimeStart = dateTimeStart.AddHours(hours);
                }

                if (minutes > 0)
                {
                    dateTimeStart = dateTimeStart.AddMinutes(minutes);
                }

                if (seconds > 0)
                {
                    dateTimeStart = dateTimeStart.AddSeconds(seconds);
                }

                return dateTimeStart.ToTimeSpan();
            }
        }

        public static DateTime? GetValue(this DateTime? dateTime)
        {
            if (dateTime.HasValue)
            {
                return dateTime;
            }
            else
            {
                return null;
            }
        }

        public static string GetValue(this DateTime? dateTime, Func<DateTime, string> formatter)
        {
            if (dateTime.HasValue)
            {
                return formatter(dateTime.Value);
            }
            else
            {
                return string.Empty;
            }
        }

        public static int NextClosetDivisible(this int n, int shift, int multiplier = 10)
        {
            shift.Countdown((n2) =>
            {
                n *= multiplier;
            });

            return n;
        }

        public static float GetDecimalTimeComponent(this TimeSpan timeSpan, Func<TimeSpan, int> getTimeComponent, int decimals)
        {
            var component = getTimeComponent(timeSpan);
            var divisor = (float) 10.NextClosetDivisible(decimals);
            var fComponent = component.As<float>() / divisor;
            var time = Math.Round(fComponent, decimals);

            return (float)time;
        }

        public static double ToUnixTimestamp(this DateTime date)
        {
            var origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            var diff = date.ToUniversalTime() - origin;

            return Math.Floor(diff.TotalSeconds);
        }

        public static string ToUniversalIso8601Time(this DateTime dateTime)
        {
            return dateTime.ToUniversalTime().ToIso8601Time();
        }

        public static string ToIso8601Time(this DateTime dateTime)
        {
            return dateTime.ToString("u").Replace(" ", "T");
        }

        public static string ToXmlDateTimeString(this DateTime dateTime, XmlDateTimeSerializationMode mode = XmlDateTimeSerializationMode.Utc)
        {
            return XmlConvert.ToString(dateTime, mode);
        }

        public static DateTime FromXmlDateTimeString(this string dateTime)
        {
            return XmlConvert.ToDateTime(dateTime);
        }

        public static uint ToEpocTime(this DateTime date)
        {
            DateTime origin = new DateTime(1970, 1, 1, 16, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan diff = date.ToUniversalTime() - origin;

            return (uint) diff.TotalSeconds;
        }

        public static string ToSortableDateTimeText(this DateTime time)
        {
            return time.ToString("yyyyMMdd_HHmmss_fffffff");
        }

        public static string ToSortableDateText(this DateTime time)
        {
            return time.ToString("yyyyMMdd");
        }

        public static string ToSortableShortDateTimeText(this DateTime time)
        {
            return time.ToString("yyyyMMdd_HHmmss");
        }

        public static string ToShortDateTimeString(this DateTime dateTime)
        {
            return dateTime.ToShortDateString() + " " + dateTime.ToShortTimeString();
        }

        public static string ToLongDateTimeString(this DateTime dateTime)
        {
            return dateTime.ToLongDateString() + " " + dateTime.ToLongTimeString();
        }

        public static string ToLongTimeDateString(this DateTime dateTime)
        {
            return dateTime.ToLongTimeString() + ", " + dateTime.ToLongDateString();
        }

        public static bool IsSortableDateTimeText(this FileSystemInfo fileSystemInfo, out DateTime dateTime)
        {
            return IsSortableDateTimeText(fileSystemInfo.Name, out dateTime);
        }

        public static bool IsSortableDateTimeText(this FileSystemInfo fileSystemInfo)
        {
            return IsSortableDateTimeText(fileSystemInfo.Name);
        }

        public static bool IsSortableShortDateTimeText(this FileSystemInfo fileSystemInfo)
        {
            return IsSortableShortDateTimeText(fileSystemInfo.Name);
        }

        public static DateTime GetSortableDateTime(this FileSystemInfo fileSystemInfo)
        {
            return GetSortableDateTime(fileSystemInfo.Name);
        }

        public static DateTime GetSortableShortDateTime(this FileSystemInfo fileSystemInfo)
        {
            return GetSortableShortDateTime(fileSystemInfo.Name);
        }

        public static string RemoveSortableDateTimeText(this string text)
        {
            var regex = new Regex(@"(?<year>\d{4})(?<month>\d{2})(?<day>\d{2})_(?<hour>\d{2})(?<minute>\d{2})(?<second>\d{2})_(?<millisecond>\d*)");

            return regex.Replace(text, string.Empty);
        }

        public static bool IsSortableDateTimeText(string text, out DateTime dateTime)
        {
            var regex = new Regex(@"(?<year>\d{4})(?<month>\d{2})(?<day>\d{2})_(?<hour>\d{2})(?<minute>\d{2})(?<second>\d{2})_(?<millisecond>\d*)");

            if (regex.IsMatch(text))
            {
                var match = regex.Match(text);
                var year = int.Parse(match.GetGroupValue("year"));
                var month = int.Parse(match.GetGroupValue("month"));
                var day = int.Parse(match.GetGroupValue("day"));
                var hour = int.Parse(match.GetGroupValue("hour"));
                var minute = int.Parse(match.GetGroupValue("minute"));
                var second = int.Parse(match.GetGroupValue("second"));
                var millisecond = int.Parse(match.GetGroupValue("millisecond"));

                dateTime = DateTime.ParseExact(match.Value, "yyyyMMdd_HHmmss_fffffff", null);

                return true;
            }
            else
            {
                dateTime = DateTime.MinValue;

                return false;
            }
        }

        public static bool IsSortableShortDateTimeText(string text, out DateTime dateTime)
        {
            var regex = new Regex(@"(?<year>\d{4})(?<month>\d{2})(?<day>\d{2})_(?<hour>\d{2})(?<minute>\d{2})(?<second>\d{2})_(?<millisecond>\d*)");

            if (regex.IsMatch(text))
            {
                var match = regex.Match(text);
                var year = int.Parse(match.GetGroupValue("year"));
                var month = int.Parse(match.GetGroupValue("month"));
                var day = int.Parse(match.GetGroupValue("day"));
                var hour = int.Parse(match.GetGroupValue("hour"));
                var minute = int.Parse(match.GetGroupValue("minute"));
                var second = int.Parse(match.GetGroupValue("second"));
                var millisecond = int.Parse(match.GetGroupValue("millisecond"));

                dateTime = DateTime.ParseExact(match.Value, "yyyyMMdd_HHmmss_fffffff", null);

                return true;
            }
            else
            {
                dateTime = DateTime.MinValue;

                return false;
            }
        }

        public static DateTime GetSortableShortDateTime(string text)
        {
            var regex = new Regex(@"(?<year>\d{4})(?<month>\d{2})(?<day>\d{2})_(?<hour>\d{2})(?<minute>\d{2})(?<second>\d{2})");
            DateTime dateTime;

            if (regex.IsMatch(text))
            {
                var match = regex.Match(text);
                var year = int.Parse(match.GetGroupValue("year"));
                var month = int.Parse(match.GetGroupValue("month"));
                var day = int.Parse(match.GetGroupValue("day"));
                var hour = int.Parse(match.GetGroupValue("hour"));
                var minute = int.Parse(match.GetGroupValue("minute"));
                var second = int.Parse(match.GetGroupValue("second"));

                dateTime = DateTime.ParseExact(match.Value, "yyyyMMdd_HHmmss", null);

                return dateTime;
            }
            else
            {
                dateTime = DateTime.MinValue;

                return dateTime;
            }
        }

        public static DateTime GetSortableDateTime(string text)
        {
            var regex = new Regex(@"(?<year>\d{4})(?<month>\d{2})(?<day>\d{2})_(?<hour>\d{2})(?<minute>\d{2})(?<second>\d{2})_(?<millisecond>\d*)");
            DateTime dateTime;

            if (regex.IsMatch(text))
            {
                var match = regex.Match(text);
                var year = int.Parse(match.GetGroupValue("year"));
                var month = int.Parse(match.GetGroupValue("month"));
                var day = int.Parse(match.GetGroupValue("day"));
                var hour = int.Parse(match.GetGroupValue("hour"));
                var minute = int.Parse(match.GetGroupValue("minute"));
                var second = int.Parse(match.GetGroupValue("second"));
                var millisecond = int.Parse(match.GetGroupValue("millisecond"));

                dateTime = DateTime.ParseExact(match.Value, "yyyyMMdd_HHmmss_fffffff", null);

                return dateTime;
            }
            else
            {
                dateTime = DateTime.MinValue;

                return dateTime;
            }
        }

        public static bool IsSortableDateTimeText(string text)
        {
            var regex = new Regex(@"(?<year>\d{4})(?<month>\d{2})(?<day>\d{2})_(?<hour>\d{2})(?<minute>\d{2})(?<second>\d{2})_(?<millisecond>\d*)");
            DateTime dateTime;

            if (regex.IsMatch(text))
            {
                var match = regex.Match(text);
                var year = int.Parse(match.GetGroupValue("year"));
                var month = int.Parse(match.GetGroupValue("month"));
                var day = int.Parse(match.GetGroupValue("day"));
                var hour = int.Parse(match.GetGroupValue("hour"));
                var minute = int.Parse(match.GetGroupValue("minute"));
                var second = int.Parse(match.GetGroupValue("second"));
                var millisecond = int.Parse(match.GetGroupValue("millisecond"));

                dateTime = DateTime.ParseExact(match.Value, "yyyyMMdd_HHmmss_fffffff", null);

                return true;
            }
            else
            {
                dateTime = DateTime.MinValue;

                return false;
            }
        }

        public static bool IsSortableShortDateTimeText(string text)
        {
            var regex = new Regex(@"(?<year>\d{4})(?<month>\d{2})(?<day>\d{2})_(?<hour>\d{2})(?<minute>\d{2})(?<second>\d{2})");
            DateTime dateTime;

            if (regex.IsMatch(text))
            {
                var match = regex.Match(text);
                var year = int.Parse(match.GetGroupValue("year"));
                var month = int.Parse(match.GetGroupValue("month"));
                var day = int.Parse(match.GetGroupValue("day"));
                var hour = int.Parse(match.GetGroupValue("hour"));
                var minute = int.Parse(match.GetGroupValue("minute"));
                var second = int.Parse(match.GetGroupValue("second"));

                dateTime = DateTime.ParseExact(match.Value, "yyyyMMdd_HHmmss", null);

                return true;
            }
            else
            {
                dateTime = DateTime.MinValue;

                return false;
            }
        }

        public static string ToDateTimeText(this DateTime time)
        {
            return time.ToShortDateString() + " " + time.ToLongTimeString();
        }

        public static DateTime ToTimeValueOnly(this DateTime dateTime)
        {
            return dateTime.AddYears(-dateTime.Year + 1).AddMonths(-dateTime.Month + 1).AddDays(-dateTime.Day + 1);
        }
    }
}
