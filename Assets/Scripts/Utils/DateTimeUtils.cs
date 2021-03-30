using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading;
using UnityEngine;

namespace Utils
{
    public class DateTimeUtils
    {
        /// <summary>
        /// Converts a DateTime to the long representation which is the number of seconds since the unix epoch.
        /// </summary>
        /// <param name="dateTime">A DateTime to convert to epoch time.</param>
        /// <returns>The long number of seconds since the unix epoch.</returns>
        public static long ToEpoch(DateTime dateTime) => (long)(dateTime - new DateTime(1970, 1, 1)).TotalSeconds;

        /// <summary>
        /// Converts a long representation of time since the unix epoch to a DateTime.
        /// </summary>
        /// <param name="epoch">The number of seconds since Jan 1, 1970.</param>
        /// <returns>A DateTime representing the time since the epoch.</returns>
        public static DateTime FromEpoch(long epoch) => new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified).AddSeconds(epoch);

        /// <summary>
        /// Converts a DateTime? to the long? representation which is the number of seconds since the unix epoch.
        /// </summary>
        /// <param name="dateTime">A DateTime? to convert to epoch time.</param>
        /// <returns>The long? number of seconds since the unix epoch.</returns>
        public static long? ToEpoch(DateTime? dateTime) => dateTime.HasValue ? (long?)ToEpoch(dateTime.Value) : null;

        /// <summary>
        /// Converts a long? representation of time since the unix epoch to a DateTime?.
        /// </summary>
        /// <param name="epoch">The number of seconds since Jan 1, 1970.</param>
        /// <returns>A DateTime? representing the time since the epoch.</returns>
        public static DateTime? FromEpoch(long? epoch) => epoch.HasValue ? (DateTime?)FromEpoch(epoch.Value) : null;
        
        
        
        public static DateTime DateFromTimestamp(double timestamp, DateTimeKind kind = DateTimeKind.Local)
        {
            var epochStart = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            return kind != DateTimeKind.Utc
                ? epochStart.AddSeconds(timestamp).ToLocalTime()
                : epochStart.AddSeconds(timestamp);
        }
        
        public static string Clock(int seconds)
        {
            var negative = MakeNonNegative(ref seconds);
            var output = new StringBuilder();

            output.Append(seconds / 60);
            output.Append(":");
            var secondsOnly = seconds % 60;
            if (secondsOnly < 10)
            {
                output.Append("0");
            }
            output.Append(secondsOnly);

            return AddNegativeSign(negative, output).ToString();
        }

        private static bool MakeNonNegative(ref int value)
        {
            if (value >= 0)
            {
                return false;
            }
            value *= -1;
            return true;
        }

        private static StringBuilder AddNegativeSign(bool negative, StringBuilder output)
        {
            if (negative)
            {
                output.Insert(0, '-');
            }

            return output;
        }

        public static string Stopper(int totalSeconds)
        {
            if (Math.Abs(totalSeconds) < 3600)
            {
                return Clock(totalSeconds);
            }
            
            var output = new StringBuilder();
            var negative = MakeNonNegative(ref totalSeconds);
            var hours = totalSeconds / 3600;
            var minutes = (totalSeconds - hours * 3600) / 60;
            var seconds = totalSeconds % 60;
            
            output.AppendFormat("{0:0}:{1:00}:{2:00}", hours, minutes, seconds);
            return AddNegativeSign(negative, output).ToString();
        }

        
        private static readonly StringBuilder builder = new StringBuilder();
        private const int SEC_PER_MINUTE = 60;
        private const int SEC_PER_HOUR = 3600;
        private const int SEC_PER_DAY = 3600 * 24;
        
        public static string TimeWithSymbols(int totalSeconds,
            bool rounded = false,
            bool forceSymbols = false,
            bool longSymbols = false,
            bool withoutMinutesAndSeconds = false)
        {
            if (forceSymbols == false && totalSeconds <= 3600 * 24)
            {
                return Stopper(totalSeconds);
            }

            var negative = MakeNonNegative(ref totalSeconds);
            var days = totalSeconds / SEC_PER_DAY;
            var hours = (totalSeconds - days * SEC_PER_DAY) / SEC_PER_HOUR;
            var minutes = (totalSeconds - days * SEC_PER_DAY - hours * SEC_PER_HOUR) / SEC_PER_MINUTE;
            var seconds = totalSeconds % SEC_PER_MINUTE;
            builder.Remove(0, builder.Length);

            if (days > 0)
            {
                builder.Append(days + (longSymbols ? " day" + (days != 1 && days != -1 ? "s " : " ") : "d "));
            }
            if (hours > 0 || (days > 0 && minutes > 0))
            {
                builder.Append(hours + (longSymbols ? " hour" + (hours != 1 && hours != -1 ? "s " : " ") : "h "));
            }
            if (totalSeconds >= 3600 && withoutMinutesAndSeconds)
            {
                return AddNegativeSign(negative, builder).ToString().TrimEnd(' ');
            }
            if (minutes > 0 || hours > 0)
            {
                if (rounded == false || days <= 0)
                {
                    builder.Append(minutes + (longSymbols ? " minute" + (minutes != 1 && minutes != -1 ? "s " : " ") : "m "));
                }
            }
            if (seconds > 0)
            {
                if (rounded == false || hours <= 0)
                {
                    builder.Append(seconds + (longSymbols ? " second" + (seconds != 1 && seconds != -1 ? "s " : " ") : "s "));
                }
            }

            return AddNegativeSign(negative, builder).ToString().TrimEnd(' ');
        }
        
        
        
        
        
        
    }
}