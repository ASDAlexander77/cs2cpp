namespace System
{
    using System;
    using System.Globalization;

    internal static class DateTimeParse
    {
        internal static DateTime ParseExact(string s, string format, Globalization.DateTimeFormatInfo dateTimeFormatInfo, Globalization.DateTimeStyles style)
        {
            throw new NotImplementedException();
        }

        internal static DateTime ParseExactMultiple(string s, string[] formats, Globalization.DateTimeFormatInfo dateTimeFormatInfo, Globalization.DateTimeStyles style)
        {
            throw new NotImplementedException();
        }

        internal static DateTime Parse(string s, Globalization.DateTimeFormatInfo dateTimeFormatInfo, Globalization.DateTimeStyles dateTimeStyles)
        {
            throw new NotImplementedException();
        }

        public static bool TryParse(string s, DateTimeFormatInfo currentInfo, DateTimeStyles none, out DateTime result)
        {
            throw new NotImplementedException();
        }

        public static bool TryParseExact(string s, string format, DateTimeFormatInfo getInstance, DateTimeStyles style, out DateTime result)
        {
            throw new NotImplementedException();
        }

        public static bool TryParseExactMultiple(string s, string[] formats, DateTimeFormatInfo getInstance, DateTimeStyles style, out DateTime result)
        {
            throw new NotImplementedException();
        }
    }
}
