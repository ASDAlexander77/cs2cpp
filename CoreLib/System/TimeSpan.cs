////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Apache License 2.0 (Apache)
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////namespace System
namespace System
{
    using System;
    using System.Runtime.CompilerServices;

    /**
     * TimeSpan represents a duration of time.  A TimeSpan can be negative
     * or positive.</p>
     *
     * <p>TimeSpan is internally represented as a number of milliseconds.  While
     * this maps well into units of time such as hours and days, any
     * periods longer than that aren't representable in a nice fashion.
     * For instance, a month can be between 28 and 31 days, while a year
     * can contain 365 or 364 days.  A decade can have between 1 and 3 leapyears,
     * depending on when you map the TimeSpan into the calendar.  This is why
     * we do not provide Years() or Months().</p>
     *
     * @see System.DateTime
     */
    [Serializable]
    public struct TimeSpan
    {
        internal long m_ticks;

        public const long TicksPerMillisecond = 10000;
        public const long TicksPerSecond = TicksPerMillisecond * 1000;
        public const long TicksPerMinute = TicksPerSecond * 60;
        public const long TicksPerHour = TicksPerMinute * 60;
        public const long TicksPerDay = TicksPerHour * 24;

        public static readonly TimeSpan Zero = new TimeSpan(0);

        public static readonly TimeSpan MaxValue = new TimeSpan(Int64.MaxValue);
        public static readonly TimeSpan MinValue = new TimeSpan(Int64.MinValue);

        internal const long MaxSeconds = Int64.MaxValue / TicksPerSecond;
        internal const long MinSeconds = Int64.MinValue / TicksPerSecond;

        private const double MillisecondsPerTick = 1.0 / TicksPerMillisecond;
        private const double SecondsPerTick = 1.0 / TicksPerSecond;
        private const double MinutesPerTick = 1.0 / TicksPerMinute;
        private const double HoursPerTick = 1.0 / TicksPerHour;
        private const double DaysPerTick = 1.0 / TicksPerDay;

        private const long MaxMilliSeconds = Int64.MaxValue / TicksPerMillisecond;
        private const long MinMilliSeconds = Int64.MinValue / TicksPerMillisecond;

        public TimeSpan(long ticks)
        {
            m_ticks = ticks;
        }


        public TimeSpan(int hours, int minutes, int seconds)
        {
            m_ticks = TimeToTicks(hours, minutes, seconds);
        }

        public TimeSpan(int days, int hours, int minutes, int seconds)
            : this(days, hours, minutes, seconds, 0)
        {
        }

        public TimeSpan(int days, int hours, int minutes, int seconds, int milliseconds)
        {
            Int64 totalMilliSeconds = ((Int64)days * 3600 * 24 + (Int64)hours * 3600 + (Int64)minutes * 60 + seconds) * 1000 + milliseconds;
            if (totalMilliSeconds > MaxMilliSeconds || totalMilliSeconds < MinMilliSeconds)
                throw new ArgumentOutOfRangeException(null, Environment.GetResourceString("Overflow_TimeSpanTooLong"));
            m_ticks = (long)totalMilliSeconds * TicksPerMillisecond;
        }

        public long Ticks
        {
            get
            {
                return m_ticks;
            }
        }

        public int Days
        {
            get
            {
                return (int)(m_ticks / TicksPerDay);
            }
        }

        public int Hours
        {
            get
            {
                return (int)((m_ticks / TicksPerHour) % 24);
            }
        }

        public int Milliseconds
        {
            get
            {
                return (int)((m_ticks / TicksPerMillisecond) % 1000);
            }
        }

        public int Minutes
        {
            get
            {
                return (int)((m_ticks / TicksPerMinute) % 60);
            }
        }

        public int Seconds
        {
            get
            {
                return (int)((m_ticks / TicksPerSecond) % 60);
            }
        }

        public double TotalDays
        {
            get { return ((double)m_ticks) * DaysPerTick; }
        }

        public double TotalHours
        {
            get { return (double)m_ticks * HoursPerTick; }
        }

        public double TotalMilliseconds
        {
            get
            {
                double temp = (double)m_ticks * MillisecondsPerTick;
                if (temp > MaxMilliSeconds)
                    return (double)MaxMilliSeconds;

                if (temp < MinMilliSeconds)
                    return (double)MinMilliSeconds;

                return temp;
            }
        }

        public double TotalMinutes
        {
            get { return (double)m_ticks * MinutesPerTick; }
        }

        public double TotalSeconds
        {
            get { return (double)m_ticks * SecondsPerTick; }
        }

        public TimeSpan Add(TimeSpan ts)
        {
            return new TimeSpan(m_ticks + ts.m_ticks);
        }

        
        extern public static int Compare(TimeSpan t1, TimeSpan t2);

        
        extern public int CompareTo(Object value);

        public TimeSpan Duration()
        {
            return new TimeSpan(m_ticks >= 0 ? m_ticks : -m_ticks);
        }


        public override bool Equals(Object value)
        {
            throw new NotImplementedException();
        }

        
        extern public static bool Equals(TimeSpan t1, TimeSpan t2);

        public TimeSpan Negate()
        {
            return new TimeSpan(-m_ticks);
        }

        public TimeSpan Subtract(TimeSpan ts)
        {
            return new TimeSpan(m_ticks - ts.m_ticks);
        }

        public static TimeSpan FromTicks(long val)
        {
            return new TimeSpan(val);
        }

        
        public override String ToString()
        {
            throw new NotImplementedException();
        }

        public static TimeSpan operator -(TimeSpan t)
        {
            return new TimeSpan(-t.m_ticks);
        }

        public static TimeSpan operator -(TimeSpan t1, TimeSpan t2)
        {
            return new TimeSpan(t1.m_ticks - t2.m_ticks);
        }

        public static TimeSpan operator +(TimeSpan t)
        {
            return t;
        }

        public static TimeSpan operator +(TimeSpan t1, TimeSpan t2)
        {
            return new TimeSpan(t1.m_ticks + t2.m_ticks);
        }

        public static bool operator ==(TimeSpan t1, TimeSpan t2)
        {
            return t1.m_ticks == t2.m_ticks;
        }

        public static bool operator !=(TimeSpan t1, TimeSpan t2)
        {
            return t1.m_ticks != t2.m_ticks;
        }

        public static bool operator <(TimeSpan t1, TimeSpan t2)
        {
            return t1.m_ticks < t2.m_ticks;
        }

        public static bool operator <=(TimeSpan t1, TimeSpan t2)
        {
            return t1.m_ticks <= t2.m_ticks;
        }

        public static bool operator >(TimeSpan t1, TimeSpan t2)
        {
            return t1.m_ticks > t2.m_ticks;
        }

        public static bool operator >=(TimeSpan t1, TimeSpan t2)
        {
            return t1.m_ticks >= t2.m_ticks;
        }

        internal static long TimeToTicks(int hour, int minute, int second)
        {
            // totalSeconds is bounded by 2^31 * 2^12 + 2^31 * 2^8 + 2^31,
            // which is less than 2^44, meaning we won't overflow totalSeconds.
            long totalSeconds = (long)hour * 3600 + (long)minute * 60 + (long)second;
            if (totalSeconds > MaxSeconds || totalSeconds < MinSeconds)
                throw new ArgumentOutOfRangeException(null, Environment.GetResourceString("Overflow_TimeSpanTooLong"));
            return totalSeconds * TicksPerSecond;
        }
    }
}


