////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Apache License 2.0 (Apache)
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////namespace System
namespace System
{

    using System;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Globalization;

    // Summary:
    //     Specifies whether a System.DateTime object represents a local time, a Coordinated
    //     Universal Time (UTC), or is not specified as either local time or UTC.
    [Serializable]
    public enum DateTimeKind
    {
        // Summary:
        //     The time represented is not specified as either local time or Coordinated
        //     Universal Time (UTC).
        //     MF does not support Unspecified type. Constructor for DateTime always creates local time.
        //     Use SpecifyKind to set Kind property to UTC or ToUniversalTime to convert local to UTC
        //Unspecified = 0,
        //
        // Summary:
        //     The time represented is UTC.
        Utc = 1,
        //
        // Summary:
        //     The time represented is local time.
        Local = 2,
    }

    /**
     * This value type represents a date and time.  Every DateTime
     * object has a private field (Ticks) of type Int64 that stores the
     * date and time as the number of 100 nanosecond intervals since
     * 12:00 AM January 1, year 1601 A.D. in the proleptic Gregorian Calendar.
     *
     * <p>For a description of various calendar issues, look at
     * <a href="http://serendipity.nofadz.com/hermetic/cal_stud.htm">
     * Calendar Studies web site</a>, at
     * http://serendipity.nofadz.com/hermetic/cal_stud.htm.
     *
     * <p>
     * <h2>Warning about 2 digit years</h2>
     * <p>As a temporary hack until we get new DateTime &lt;-&gt; String code,
     * some systems won't be able to round trip dates less than 1930.  This
     * is because we're using OleAut's string parsing routines, which look
     * at your computer's default short date string format, which uses 2 digit
     * years on most computers.  To fix this, go to Control Panel -&gt; Regional
     * Settings -&gt; Date and change the short date style to something like
     * "M/d/yyyy" (specifying four digits for the year).
     *
     */
    [Serializable()]
    public struct DateTime : IFormattable, IConvertible
    {
        [MethodImplAttribute(MethodImplOptions.Unmanaged)]
        public static extern unsafe int gettimeofday(int* time, int* timezome);

        // Number of 100ns ticks per time unit
        private const long TicksPerMillisecond = 10000;
        private const long TicksPerSecond = TicksPerMillisecond * 1000;
        private const long TicksPerMinute = TicksPerSecond * 60;
        private const long TicksPerHour = TicksPerMinute * 60;
        private const long TicksPerDay = TicksPerHour * 24;

        // Number of milliseconds per time unit
        private const int MillisPerSecond = 1000;
        private const int MillisPerMinute = MillisPerSecond * 60;
        private const int MillisPerHour = MillisPerMinute * 60;
        private const int MillisPerDay = MillisPerHour * 24;

        // Number of days in a non-leap year
        private const int DaysPerYear = 365;
        // Number of days in 4 years
        private const int DaysPer4Years = DaysPerYear * 4 + 1;
        // Number of days in 100 years
        private const int DaysPer100Years = DaysPer4Years * 25 - 1;
        // Number of days in 400 years
        private const int DaysPer400Years = DaysPer100Years * 4 + 1;

        // Number of days from 1/1/0001 to 12/31/1600
        private const int DaysTo1601 = DaysPer400Years * 4;
        // Number of days from 1/1/0001 to 12/30/1899
        private const int DaysTo1899 = DaysPer400Years * 4 + DaysPer100Years * 3 - 367;
        // Number of days from 1/1/0001 to 12/31/9999
        private const int DaysTo10000 = DaysPer400Years * 25 - 366;

        private const long MinTicks = 0;
        private const long MaxTicks = 441796895990000000;
        private const long MaxMillis = (long)DaysTo10000 * MillisPerDay;

        // This is mask to extract ticks from m_ticks
        private const ulong TickMask = 0x7FFFFFFFFFFFFFFFL;
        private const ulong UTCMask = 0x8000000000000000L;

        public static readonly DateTime MinValue = new DateTime(MinTicks);
        public static readonly DateTime MaxValue = new DateTime(MaxTicks);

        private const int DatePartYear = 0;
        private const int DatePartDayOfYear = 1;
        private const int DatePartMonth = 2;
        private const int DatePartDay = 3;

        private static readonly int[] DaysToMonth365 = {
            0, 31, 59, 90, 120, 151, 181, 212, 243, 273, 304, 334, 365};
        private static readonly int[] DaysToMonth366 = {
            0, 31, 60, 91, 121, 152, 182, 213, 244, 274, 305, 335, 366};

        private static readonly int[] timeBuffer = new int[4];

        private ulong m_ticks;

        public DateTime(long ticks)
        {
            if (((ticks & (long)TickMask) < MinTicks) || ((ticks & (long)TickMask) > MaxTicks))
            {
                throw new ArgumentOutOfRangeException("ticks", "Ticks must be between DateTime.MinValue.Ticks and DateTime.MaxValue.Ticks.");
            }

            m_ticks = (ulong)ticks;
        }

        public DateTime(long ticks, DateTimeKind kind)
            : this(ticks)
        {
            if (kind == DateTimeKind.Local)
            {
                m_ticks &= ~UTCMask;
            }
            else
            {
                m_ticks |= UTCMask;
            }
        }

        public DateTime(int year, int month, int day)
            : this(year, month, day, 0, 0, 0)
        {
        }

        public DateTime(int year, int month, int day, int hour, int minute, int second)
            : this(year, month, day, hour, minute, second, 0)
        {
        }

        public DateTime(int year, int month, int day, int hour, int minute, int second, int millisecond)
        {
            throw new NotImplementedException();
        }

        public DateTime Add(TimeSpan val)
        {
            return new DateTime((long)m_ticks + val.Ticks);
        }

        private DateTime Add(double val, int scale)
        {
            return new DateTime((long)((long)m_ticks + (long)(val * scale * TicksPerMillisecond + (val >= 0 ? 0.5 : -0.5))));
        }

        public DateTime AddDays(double val)
        {
            return Add(val, MillisPerDay);
        }

        public DateTime AddHours(double val)
        {
            return Add(val, MillisPerHour);
        }

        public DateTime AddMilliseconds(double val)
        {
            return Add(val, 1);
        }

        public DateTime AddMinutes(double val)
        {
            return Add(val, MillisPerMinute);
        }

        public DateTime AddSeconds(double val)
        {
            return Add(val, MillisPerSecond);
        }

        public DateTime AddTicks(long val)
        {
            return new DateTime((long)m_ticks + val);
        }

        public static int Compare(DateTime t1, DateTime t2)
        {
            // Get ticks, clear UTC mask
            ulong t1_ticks = t1.m_ticks & TickMask;
            ulong t2_ticks = t2.m_ticks & TickMask;

            // Compare ticks, ignore the Kind property.
            if (t1_ticks > t2_ticks)
            {
                return 1;
            }

            if (t1_ticks < t2_ticks)
            {
                return -1;
            }

            // Values are equal
            return 0;
        }

        public int CompareTo(Object val)
        {
            if (val == null) return 1;

            return DateTime.Compare(this, (DateTime)val);
        }


        public extern static int DaysInMonth(int year, int month);

        public override bool Equals(Object val)
        {
            if (val is DateTime)
            {
                // Call compare for proper comparison of 2 DateTime objects
                // Since DateTime is optimized value and internally represented by int64
                // "this" may still have type int64.
                // Convertion to object and back is a workaround.
                object o = this;
                DateTime thisTime = (DateTime)o;
                return Compare(thisTime, (DateTime)val) == 0;
            }

            return false;
        }

        public static bool Equals(DateTime t1, DateTime t2)
        {
            return Compare(t1, t2) == 0;
        }

        public DateTime Date
        {
            get
            {
                // Need to remove UTC mask before arithmetic operations. Then set it back.
                if ((m_ticks & UTCMask) != 0)
                {
                    return new DateTime((long)(((m_ticks & TickMask) - (m_ticks & TickMask) % TicksPerDay) | UTCMask));
                }
                else
                {
                    return new DateTime((long)(m_ticks - m_ticks % TicksPerDay));
                }
            }
        }

        public int Day
        {
            get
            {
                return GetDatePart(DatePartDay);
            }
        }

        public DayOfWeek DayOfWeek
        {
            get
            {
                return (DayOfWeek)((m_ticks / TicksPerDay + 1) % 7);
            }
        }

        public int DayOfYear
        {
            get
            {
                return GetDatePart(DatePartDayOfYear);
            }
        }

        /// Reduce size by calling a single method?
        public int Hour
        {
            get
            {
                return (int)((m_ticks / TicksPerHour) % 24);
            }
        }

        public DateTimeKind Kind
        {
            get
            {
                // If mask for UTC time is set - return UTC. If no maskk - return local.
                return (m_ticks & UTCMask) != 0 ? DateTimeKind.Utc : DateTimeKind.Local;
            }
        }

        public static DateTime SpecifyKind(DateTime value, DateTimeKind kind)
        {
            DateTime retVal = new DateTime((long)value.m_ticks);

            if (kind == DateTimeKind.Utc)
            {
                // Set UTC mask
                retVal.m_ticks = value.m_ticks | UTCMask;
            }
            else
            {   // Clear UTC mask
                retVal.m_ticks = value.m_ticks & ~UTCMask;
            }

            return retVal;
        }

        public int Millisecond
        {
            get
            {
                return (int)((m_ticks / TicksPerMillisecond) % 1000);
            }
        }

        public int Minute
        {
            get
            {
                return (int)((m_ticks / TicksPerMinute) % 60);
            }
        }

        public int Month
        {
            get
            {
                return 0;
            }
        }

        public static DateTime Now
        {
            get
            {
                unsafe
                {
                    fixed (int* p = &timeBuffer[0])
                    fixed (int* p2 = &timeBuffer[2])
                    {
                        if (gettimeofday(p, p2) == 0)
                        {
                            return new DateTime(p[0] * TicksPerSecond + p[1] * 10 + p2[2] * TicksPerMinute);
                        }
                    }

                    throw new Exception();
                }
            }
        }

        public static DateTime UtcNow
        {

            get
            {
                unsafe
                {
                    fixed (int* p = &timeBuffer[0])
                    {
                        if (gettimeofday(p, null) == 0)
                        {
                            return new DateTime(p[0] * TicksPerSecond + p[1] * 10, DateTimeKind.Utc);
                        }
                    }

                    throw new Exception();
                }
            }
        }

        public int Second
        {

            get
            {
                return (int)((m_ticks / TicksPerSecond) % 60);
            }
        }

        /// Our origin is at 1601/01/01:00:00:00.000
        /// While desktop CLR's origin is at 0001/01/01:00:00:00.000.
        /// There are 504911232000000000 ticks between them which we are subtracting.
        /// See DeviceCode\PAL\time_decl.h for explanation of why we are taking
        /// year 1601 as origin for our HAL, PAL, and CLR.
        // static Int64 ticksAtOrigin = 504911232000000000;
        static Int64 ticksAtOrigin = 0;
        public long Ticks
        {
            get
            {
                return (long)(m_ticks & TickMask) + ticksAtOrigin;
            }
        }

        public TimeSpan TimeOfDay
        {
            get
            {
                return new TimeSpan((long)((m_ticks & TickMask) % TicksPerDay));
            }
        }

        public static DateTime Today
        {

            get
            {
                return new DateTime();
            }
        }

        public int Year
        {

            get
            {
                return 0;
            }
        }

        // Constructs a DateTime from a string. The string must specify a
        // date and optionally a time in a culture-specific or universal format.
        // Leading and trailing whitespace characters are allowed.
        // 
        public static DateTime Parse(String s)
        {
            return (DateTimeParse.Parse(s, DateTimeFormatInfo.CurrentInfo, DateTimeStyles.None));
        }

        // Constructs a DateTime from a string. The string must specify a
        // date and optionally a time in a culture-specific or universal format.
        // Leading and trailing whitespace characters are allowed.
        // 
        public static DateTime Parse(String s, IFormatProvider provider)
        {
            return (DateTimeParse.Parse(s, DateTimeFormatInfo.GetInstance(provider), DateTimeStyles.None));
        }

        public static DateTime Parse(String s, IFormatProvider provider, DateTimeStyles styles)
        {
            DateTimeFormatInfo.ValidateStyles(styles, "styles");
            return (DateTimeParse.Parse(s, DateTimeFormatInfo.GetInstance(provider), styles));
        }

        // Constructs a DateTime from a string. The string must specify a
        // date and optionally a time in a culture-specific or universal format.
        // Leading and trailing whitespace characters are allowed.
        // 
        public static DateTime ParseExact(String s, String format, IFormatProvider provider)
        {
            return (DateTimeParse.ParseExact(s, format, DateTimeFormatInfo.GetInstance(provider), DateTimeStyles.None));
        }

        // Constructs a DateTime from a string. The string must specify a
        // date and optionally a time in a culture-specific or universal format.
        // Leading and trailing whitespace characters are allowed.
        // 
        public static DateTime ParseExact(String s, String format, IFormatProvider provider, DateTimeStyles style)
        {
            DateTimeFormatInfo.ValidateStyles(style, "style");
            return (DateTimeParse.ParseExact(s, format, DateTimeFormatInfo.GetInstance(provider), style));
        }

        public static DateTime ParseExact(String s, String[] formats, IFormatProvider provider, DateTimeStyles style)
        {
            DateTimeFormatInfo.ValidateStyles(style, "style");
            return DateTimeParse.ParseExactMultiple(s, formats, DateTimeFormatInfo.GetInstance(provider), style);
        }

        public TimeSpan Subtract(DateTime val)
        {
            return new TimeSpan((long)(m_ticks & TickMask) - (long)(val.m_ticks & TickMask));
        }

        public DateTime Subtract(TimeSpan val)
        {
            return new DateTime((long)(m_ticks - (ulong)val.m_ticks));
        }

        public DateTime ToLocalTime()
        {
            throw new NotImplementedException();
        }

        public override String ToString()
        {
            return DateTimeFormat.Format(this, null, DateTimeFormatInfo.CurrentInfo);
        }

        public String ToString(String format)
        {
            return DateTimeFormat.Format(this, format, DateTimeFormatInfo.CurrentInfo);
        }

        public String ToString(IFormatProvider provider)
        {
            return DateTimeFormat.Format(this, null, DateTimeFormatInfo.GetInstance(provider));
        }

        public String ToString(String format, IFormatProvider provider)
        {
            return DateTimeFormat.Format(this, format, DateTimeFormatInfo.GetInstance(provider));
        }

        public DateTime ToUniversalTime()
        {
            throw new NotImplementedException();
        }

        public static DateTime operator +(DateTime d, TimeSpan t)
        {
            return new DateTime((long)(d.m_ticks + (ulong)t.m_ticks));
        }

        public static DateTime operator -(DateTime d, TimeSpan t)
        {
            return new DateTime((long)(d.m_ticks - (ulong)t.m_ticks));
        }

        public static TimeSpan operator -(DateTime d1, DateTime d2)
        {
            return d1.Subtract(d2);
        }

        public static bool operator ==(DateTime d1, DateTime d2)
        {
            return Compare(d1, d2) == 0;
        }

        public static bool operator !=(DateTime t1, DateTime t2)
        {
            return Compare(t1, t2) != 0;
        }

        public static bool operator <(DateTime t1, DateTime t2)
        {
            return Compare(t1, t2) < 0;
        }

        public static bool operator <=(DateTime t1, DateTime t2)
        {
            return Compare(t1, t2) <= 0;
        }

        public static bool operator >(DateTime t1, DateTime t2)
        {
            return Compare(t1, t2) > 0;
        }

        public static bool operator >=(DateTime t1, DateTime t2)
        {
            return Compare(t1, t2) >= 0;
        }

        private int GetDatePart(int part)
        {
            var ticks = m_ticks;
            // n = number of days since 1/1/0001
            int n = (int)(ticks / TicksPerDay);
            // y400 = number of whole 400-year periods since 1/1/0001
            int y400 = n / DaysPer400Years;
            // n = day number within 400-year period
            n -= y400 * DaysPer400Years;
            // y100 = number of whole 100-year periods within 400-year period
            int y100 = n / DaysPer100Years;
            // Last 100-year period has an extra day, so decrement result if 4
            if (y100 == 4) y100 = 3;
            // n = day number within 100-year period
            n -= y100 * DaysPer100Years;
            // y4 = number of whole 4-year periods within 100-year period
            int y4 = n / DaysPer4Years;
            // n = day number within 4-year period
            n -= y4 * DaysPer4Years;
            // y1 = number of whole years within 4-year period
            int y1 = n / DaysPerYear;
            // Last year has an extra day, so decrement result if 4
            if (y1 == 4) y1 = 3;
            // If year was requested, compute and return it
            if (part == DatePartYear)
            {
                return y400 * 400 + y100 * 100 + y4 * 4 + y1 + 1;
            }
            // n = day number within year
            n -= y1 * DaysPerYear;
            // If day-of-year was requested, return it
            if (part == DatePartDayOfYear) return n + 1;
            // Leap year calculation looks different from IsLeapYear since y1, y4,
            // and y100 are relative to year 1, not year 0
            bool leapYear = y1 == 3 && (y4 != 24 || y100 == 3);
            int[] days = leapYear ? DaysToMonth366 : DaysToMonth365;
            // All months have less than 32 days, so n >> 5 is a good conservative
            // estimate for the month
            int m = n >> 5 + 1;
            // m = 1-based month number
            while (n >= days[m]) m++;
            // If month was requested, return it
            if (part == DatePartMonth) return m;
            // Return 1-based day-of-month
            return n - days[m - 1] + 1;
        }

        public TypeCode GetTypeCode()
        {
            return TypeCode.DateTime;
        }


        /// <internalonly/>
        bool IConvertible.ToBoolean(IFormatProvider provider)
        {
            throw new InvalidCastException(Environment.GetResourceString("InvalidCast_FromTo", "DateTime", "Boolean"));
        }

        /// <internalonly/>
        char IConvertible.ToChar(IFormatProvider provider)
        {
            throw new InvalidCastException(Environment.GetResourceString("InvalidCast_FromTo", "DateTime", "Char"));
        }

        /// <internalonly/>
        sbyte IConvertible.ToSByte(IFormatProvider provider)
        {
            throw new InvalidCastException(Environment.GetResourceString("InvalidCast_FromTo", "DateTime", "SByte"));
        }

        /// <internalonly/>
        byte IConvertible.ToByte(IFormatProvider provider)
        {
            throw new InvalidCastException(Environment.GetResourceString("InvalidCast_FromTo", "DateTime", "Byte"));
        }

        /// <internalonly/>
        short IConvertible.ToInt16(IFormatProvider provider)
        {
            throw new InvalidCastException(Environment.GetResourceString("InvalidCast_FromTo", "DateTime", "Int16"));
        }

        /// <internalonly/>
        ushort IConvertible.ToUInt16(IFormatProvider provider)
        {
            throw new InvalidCastException(Environment.GetResourceString("InvalidCast_FromTo", "DateTime", "UInt16"));
        }

        /// <internalonly/>
        int IConvertible.ToInt32(IFormatProvider provider)
        {
            throw new InvalidCastException(Environment.GetResourceString("InvalidCast_FromTo", "DateTime", "Int32"));
        }

        /// <internalonly/>
        uint IConvertible.ToUInt32(IFormatProvider provider)
        {
            throw new InvalidCastException(Environment.GetResourceString("InvalidCast_FromTo", "DateTime", "UInt32"));
        }

        /// <internalonly/>
        long IConvertible.ToInt64(IFormatProvider provider)
        {
            throw new InvalidCastException(Environment.GetResourceString("InvalidCast_FromTo", "DateTime", "Int64"));
        }

        /// <internalonly/>
        ulong IConvertible.ToUInt64(IFormatProvider provider)
        {
            throw new InvalidCastException(Environment.GetResourceString("InvalidCast_FromTo", "DateTime", "UInt64"));
        }

        /// <internalonly/>
        float IConvertible.ToSingle(IFormatProvider provider)
        {
            throw new InvalidCastException(Environment.GetResourceString("InvalidCast_FromTo", "DateTime", "Single"));
        }

        /// <internalonly/>
        double IConvertible.ToDouble(IFormatProvider provider)
        {
            throw new InvalidCastException(Environment.GetResourceString("InvalidCast_FromTo", "DateTime", "Double"));
        }

        /// <internalonly/>
        Decimal IConvertible.ToDecimal(IFormatProvider provider)
        {
            throw new InvalidCastException(Environment.GetResourceString("InvalidCast_FromTo", "DateTime", "Decimal"));
        }

        DateTime IConvertible.ToDateTime(IFormatProvider provider)
        {
            return this;
        }

        /// <internalonly/>
        Object IConvertible.ToType(Type type, IFormatProvider provider)
        {
            return Convert.DefaultToType((IConvertible)this, type, provider);
        }
    }
}


