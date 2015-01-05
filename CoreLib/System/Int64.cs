namespace System
{
    using System.Globalization;
    using System.Runtime.InteropServices;

    public struct Int64
    {
        internal long m_value;

        public const long MaxValue = 0x7fffffffffffffffL;
        public const long MinValue = unchecked((long)0x8000000000000000L);

        // Compares this object to another object, returning an integer that
        // indicates the relationship. 
        // Returns a value less than zero if this  object
        // null is considered to be less than any instance.
        // If object is not of type Int64, this method throws an ArgumentException.
        // 
        public int CompareTo(Object value)
        {
            if (value == null)
            {
                return 1;
            }
            if (value is Int64)
            {
                // Need to use compare because subtraction will wrap
                // to positive for very large neg numbers, etc.
                long i = (long)value;
                if (m_value < i) return -1;
                if (m_value > i) return 1;
                return 0;
            }
            throw new ArgumentException(Environment.GetResourceString("Arg_MustBeInt64"));
        }

        public int CompareTo(Int64 value)
        {
            // Need to use compare because subtraction will wrap
            // to positive for very large neg numbers, etc.
            if (m_value < value) return -1;
            if (m_value > value) return 1;
            return 0;
        }

        public override bool Equals(Object obj)
        {
            if (!(obj is Int64))
            {
                return false;
            }
            return m_value == ((Int64)obj).m_value;
        }

        public bool Equals(Int64 obj)
        {
            return m_value == obj;
        }

        // The value of the lower 32 bits XORed with the uppper 32 bits.
        public override int GetHashCode()
        {
            return (unchecked((int)((long)m_value)) ^ (int)(m_value >> 32));
        }

        public override String ToString()
        {
            return Number.FormatInt64(m_value, null, NumberFormatInfo.CurrentInfo);
        }

        public String ToString(IFormatProvider provider)
        {
            return Number.FormatInt64(m_value, null, NumberFormatInfo.GetInstance(provider));
        }

        public String ToString(String format)
        {
            return Number.FormatInt64(m_value, format, NumberFormatInfo.CurrentInfo);
        }

        public String ToString(String format, IFormatProvider provider)
        {
            return Number.FormatInt64(m_value, format, NumberFormatInfo.GetInstance(provider));
        }

        public static long Parse(String s)
        {
            return Number.ParseInt64(s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo);
        }

        public static long Parse(String s, NumberStyles style)
        {
            NumberFormatInfo.ValidateParseStyleInteger(style);
            return Number.ParseInt64(s, style, NumberFormatInfo.CurrentInfo);
        }

        public static long Parse(String s, IFormatProvider provider)
        {
            return Number.ParseInt64(s, NumberStyles.Integer, NumberFormatInfo.GetInstance(provider));
        }


        // Parses a long from a String in the given style.  If
        // a NumberFormatInfo isn't specified, the current culture's 
        // NumberFormatInfo is assumed.
        // 
        public static long Parse(String s, NumberStyles style, IFormatProvider provider)
        {
            NumberFormatInfo.ValidateParseStyleInteger(style);
            return Number.ParseInt64(s, style, NumberFormatInfo.GetInstance(provider));
        }

        public static Boolean TryParse(String s, out Int64 result)
        {
            return Number.TryParseInt64(s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out result);
        }

        public static Boolean TryParse(String s, NumberStyles style, IFormatProvider provider, out Int64 result)
        {
            NumberFormatInfo.ValidateParseStyleInteger(style);
            return Number.TryParseInt64(s, style, NumberFormatInfo.GetInstance(provider), out result);
        }
    }
}
