using System;
using System.Globalization;

namespace AtmReportGenerator.Utils
{
    public static class DateTimeExtensions
    {
        public static string ToLongAtmFormat(this DateTime date) => date.ToString("dd.MM.yyyy HH:mm", CultureInfo.InvariantCulture);

        public static string ToShortAtmFormat(this DateTime date) => date.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture);
    }
}