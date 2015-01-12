////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Apache License 2.0 (Apache)
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////#define ENABLE_CROSS_APPDOMAIN
#define ENABLE_CROSS_APPDOMAIN
namespace System.Globalization
{
    using System;
    using System.Collections;
    public sealed class DateTimeFormatInfo : /*ICloneable,*/ IFormatProvider
    {
        internal String amDesignator = null;
        internal String pmDesignator = null;
        internal String dateSeparator = null;
        internal String longTimePattern = null;
        internal String shortTimePattern = null;
        internal String generalShortTimePattern = null;
        internal String generalLongTimePattern = null;
        internal String timeSeparator = null;
        internal String monthDayPattern = null;
        internal const String rfc1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
        internal const String sortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
        internal const String universalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
        internal String fullDateTimePattern = null;
        internal String longDatePattern = null;
        internal String shortDatePattern = null;
        internal String yearMonthPattern = null;
        internal String[] abbreviatedDayNames = null;
        internal String[] dayNames = null;
        internal String[] abbreviatedMonthNames = null;
        internal String[] monthNames = null;
        CultureInfo m_cultureInfo;
        internal DateTimeFormatInfo(CultureInfo cultureInfo)
        {
            m_cultureInfo = cultureInfo;
        }

        public static DateTimeFormatInfo CurrentInfo
        {
            get
            {
                return CultureInfo.CurrentUICulture.DateTimeFormat;
            }
        }

        public String AMDesignator
        {
            get
            {
                return m_cultureInfo.EnsureStringResource(ref this.amDesignator, System.Globalization.Resources.CultureInfo.StringResources.AMDesignator);
            }
        }

        public String DateSeparator
        {
            get
            {
                return m_cultureInfo.EnsureStringResource(ref this.dateSeparator, System.Globalization.Resources.CultureInfo.StringResources.DateSeparator);
            }
        }

        public String FullDateTimePattern
        {
            get
            {
                if (fullDateTimePattern == null)
                {
                    fullDateTimePattern = LongDatePattern + " " + LongTimePattern;
                }

                return (fullDateTimePattern);
            }
        }

        public String LongDatePattern
        {
            get
            {
                return m_cultureInfo.EnsureStringResource(ref longDatePattern, System.Globalization.Resources.CultureInfo.StringResources.LongDatePattern);
            }
        }

        public String LongTimePattern
        {
            get
            {
                return m_cultureInfo.EnsureStringResource(ref this.longTimePattern, System.Globalization.Resources.CultureInfo.StringResources.LongTimePattern);
            }
        }

        public String MonthDayPattern
        {
            get
            {
                return m_cultureInfo.EnsureStringResource(ref this.monthDayPattern, System.Globalization.Resources.CultureInfo.StringResources.MonthDayPattern);
            }
        }

        public String PMDesignator
        {
            get
            {
                return m_cultureInfo.EnsureStringResource(ref this.pmDesignator, System.Globalization.Resources.CultureInfo.StringResources.PMDesignator);
            }
        }

        public String RFC1123Pattern
        {
            get
            {
                return (rfc1123Pattern);
            }
        }

        public String ShortDatePattern
        {
            get
            {
                return m_cultureInfo.EnsureStringResource(ref this.shortDatePattern, System.Globalization.Resources.CultureInfo.StringResources.ShortDatePattern);
            }
        }

        public String ShortTimePattern
        {
            get
            {
                return m_cultureInfo.EnsureStringResource(ref this.shortTimePattern, System.Globalization.Resources.CultureInfo.StringResources.ShortTimePattern);
            }
        }

        public String SortableDateTimePattern
        {
            get
            {
                return (sortableDateTimePattern);
            }
        }

        internal String GeneralShortTimePattern
        {
            get
            {
                if (generalShortTimePattern == null)
                {
                    generalShortTimePattern = ShortDatePattern + " " + ShortTimePattern;
                }

                return (generalShortTimePattern);
            }
        }

        /*=================================GeneralLongTimePattern=====================
        **Property: Return the pattern for 'g' general format: shortDate + Long time
        **Note: This is used by DateTimeFormat.cs to get the pattern for 'g'
        **      We put this internal property here so that we can avoid doing the
        **      concatation every time somebody asks for the general format.
        ==============================================================================*/
        internal String GeneralLongTimePattern
        {
            get
            {
                if (generalLongTimePattern == null)
                {
                    generalLongTimePattern = ShortDatePattern + " " + LongTimePattern;
                }

                return (generalLongTimePattern);
            }
        }

        public String TimeSeparator
        {
            get
            {
                return m_cultureInfo.EnsureStringResource(ref this.timeSeparator, System.Globalization.Resources.CultureInfo.StringResources.TimeSeparator);
            }
        }

        public String UniversalSortableDateTimePattern
        {
            get
            {
                return (universalSortableDateTimePattern);
            }
        }

        public String YearMonthPattern
        {
            get
            {
                return m_cultureInfo.EnsureStringResource(ref this.yearMonthPattern, System.Globalization.Resources.CultureInfo.StringResources.YearMonthPattern);
            }
        }

        public String[] AbbreviatedDayNames
        {
            get
            {
                return m_cultureInfo.EnsureStringArrayResource(ref abbreviatedDayNames, System.Globalization.Resources.CultureInfo.StringResources.AbbreviatedDayNames);
            }
        }

        public String[] DayNames
        {
            get
            {
                return m_cultureInfo.EnsureStringArrayResource(ref dayNames, System.Globalization.Resources.CultureInfo.StringResources.DayNames);
            }
        }

        public String[] AbbreviatedMonthNames
        {
            get
            {
                return m_cultureInfo.EnsureStringArrayResource(ref abbreviatedMonthNames, System.Globalization.Resources.CultureInfo.StringResources.AbbreviatedMonthNames);
            }
        }

        public String[] MonthNames
        {
            get
            {
                return m_cultureInfo.EnsureStringArrayResource(ref monthNames, System.Globalization.Resources.CultureInfo.StringResources.MonthNames);
            }
        }

        public static DateTimeFormatInfo GetInstance(IFormatProvider provider)
        {
            // Fast case for a regular CultureInfo
            DateTimeFormatInfo info;
            CultureInfo cultureProvider = provider as CultureInfo;
            if (cultureProvider != null && !cultureProvider.m_isInherited)
            {
                return cultureProvider.DateTimeFormat;
            }
            // Fast case for a DTFI;
            info = provider as DateTimeFormatInfo;
            if (info != null)
            {
                return info;
            }
            // Wasn't cultureInfo or DTFI, do it the slower way
            if (provider != null)
            {
                info = provider.GetFormat(typeof(DateTimeFormatInfo)) as DateTimeFormatInfo;
                if (info != null)
                {
                    return info;
                }
            }
            // Couldn't get anything, just use currentInfo as fallback
            return CurrentInfo;
        }

        public Object GetFormat(Type formatType)
        {
            return (formatType == typeof(DateTimeFormatInfo) ? this : null);
        }

        internal const DateTimeStyles InvalidDateTimeStyles = ~(DateTimeStyles.AllowLeadingWhite | DateTimeStyles.AllowTrailingWhite
                                                               | DateTimeStyles.AllowInnerWhite | DateTimeStyles.NoCurrentDateDefault
                                                               | DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeLocal
                                                               | DateTimeStyles.AssumeUniversal | DateTimeStyles.RoundtripKind);

        internal static void ValidateStyles(DateTimeStyles style, String parameterName) {
            if ((style & InvalidDateTimeStyles) != 0) {
                throw new ArgumentException(Environment.GetResourceString("Argument_InvalidDateTimeStyles"), parameterName);
            }
            if (((style & (DateTimeStyles.AssumeLocal)) != 0) && ((style & (DateTimeStyles.AssumeUniversal)) != 0)) {
                throw new ArgumentException(Environment.GetResourceString("Argument_ConflictingDateTimeStyles"), parameterName);
            }

            if (((style & DateTimeStyles.RoundtripKind) != 0)
                && ((style & (DateTimeStyles.AssumeLocal | DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal)) != 0)) {
                throw new ArgumentException(Environment.GetResourceString("Argument_ConflictingDateTimeRoundtripStyles"), parameterName);
            }
        }
    }
}


