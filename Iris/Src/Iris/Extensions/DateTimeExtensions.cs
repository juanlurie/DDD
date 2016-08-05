﻿using System.Globalization;

// ReSharper disable CheckNamespace
namespace System
// ReSharper restore CheckNamespace
{
    public static class DateTimeExtensions
    {
        public const string Format = "yyyy-MM-dd HH:mm:ss:ffffff Z";

        /// <summary>
        /// Converts the <see cref="DateTime"/> to a <see cref="string"/> suitable for transport over the wire
        /// </summary>
        /// <returns></returns>
        public static string ToWireFormattedString(this DateTime dateTime)
        {
            return dateTime.ToUniversalTime().ToString(Format, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Converts a wire formatted <see cref="string"/> from <see cref="ToWireFormattedString"/> to a UTC <see cref="DateTime"/>
        /// </summary>
        /// <returns></returns>
        public static DateTime ToUtcDateTime(this string wireFormattedString)
        {
            return DateTime.ParseExact(wireFormattedString, Format, CultureInfo.InvariantCulture).ToUniversalTime();
        }

        public static string ToDisplayString(this DateTime dateTime)
        {
            return dateTime.ToString("dd/mm/yyyy HH:mm");
        }

        [Obsolete("Use FirstDayOfMonth", true)]
        public static DateTime FirstDayOfCurrentMonth(this DateTime current)
        {
            return new DateTime(current.Year, current.Month, 1);
        }

        public static DateTime FirstDayOfMonth(this DateTime current)
        {
            return new DateTime(current.Year, current.Month, 1);
        }

        public static DateTime FirstDayOfNextMonth(this DateTime current)
        {
            return new DateTime(current.AddMonths(1).Year, current.AddMonths(1).Month, 1);
        }

        public static DateTime LastDayOfMonth(this DateTime date)
        {
            DateTime endOfMonth = date.AddMonths(1);
            return new DateTime(endOfMonth.Year, endOfMonth.Month, 1).AddSeconds(-1);
        }
    }
}
