namespace System
{
    using System.Globalization;
    using System.Runtime.InteropServices;

    public struct Int32
    {
        internal int m_value;

        public const int MaxValue = 0x7fffffff;
        public const int MinValue = unchecked((int)0x80000000);

        // Compares this object to another object, returning an integer that
        // indicates the relationship. 
        // Returns a value less than zero if this  object
        // null is considered to be less than any instance.
        // If object is not of type Int32, this method throws an ArgumentException.
        // 
        public int CompareTo(Object value)
        {
            if (value == null)
            {
                return 1;
            }
            if (value is Int32)
            {
                // Need to use compare because subtraction will wrap
                // to positive for very large neg numbers, etc.
                int i = (int)value;
                if (m_value < i) return -1;
                if (m_value > i) return 1;
                return 0;
            }
            throw new ArgumentException(Environment.GetResourceString("Arg_MustBeInt32"));
        }

        public int CompareTo(int value)
        {
            // Need to use compare because subtraction will wrap
            // to positive for very large neg numbers, etc.
            if (m_value < value) return -1;
            if (m_value > value) return 1;
            return 0;
        }

        public override bool Equals(Object obj)
        {
            if (!(obj is Int32))
            {
                return false;
            }
            return m_value == ((Int32)obj).m_value;
        }

        public bool Equals(Int32 obj)
        {
            return m_value == obj;
        }

        // The absolute value of the int contained.
        public override int GetHashCode()
        {
            return m_value;
        }

        public override String ToString()
        {
            return Number.FormatInt32(m_value, null, NumberFormatInfo.CurrentInfo);
        }

        public String ToString(String format)
        {
            return Number.FormatInt32(m_value, format, NumberFormatInfo.CurrentInfo);
        }

        public String ToString(IFormatProvider provider)
        {
            return Number.FormatInt32(m_value, null, NumberFormatInfo.GetInstance(provider));
        }

        public String ToString(String format, IFormatProvider provider)
        {
            return Number.FormatInt32(m_value, format, NumberFormatInfo.GetInstance(provider));
        }

        public static int Parse(String s)
        {
            return Number.ParseInt32(s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo);
        }

        public static int Parse(String s, NumberStyles style)
        {
            NumberFormatInfo.ValidateParseStyleInteger(style);
            return Number.ParseInt32(s, style, NumberFormatInfo.CurrentInfo);
        }

        // Parses an integer from a String in the given style.  If
        // a NumberFormatInfo isn't specified, the current culture's 
        // NumberFormatInfo is assumed.
        // 
        public static int Parse(String s, IFormatProvider provider)
        {
            return Number.ParseInt32(s, NumberStyles.Integer, NumberFormatInfo.GetInstance(provider));
        }

        // Parses an integer from a String in the given style.  If
        // a NumberFormatInfo isn't specified, the current culture's 
        // NumberFormatInfo is assumed.
        // 
        public static int Parse(String s, NumberStyles style, IFormatProvider provider)
        {
            NumberFormatInfo.ValidateParseStyleInteger(style);
            return Number.ParseInt32(s, style, NumberFormatInfo.GetInstance(provider));
        }

        // Parses an integer from a String. Returns false rather
        // than throwing exceptin if input is invalid
        // 
        public static bool TryParse(String s, out Int32 result)
        {
            return Number.TryParseInt32(s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out result);
        }

        // Parses an integer from a String in the given style. Returns false rather
        // than throwing exceptin if input is invalid
        // 
        public static bool TryParse(String s, NumberStyles style, IFormatProvider provider, out Int32 result)
        {
            NumberFormatInfo.ValidateParseStyleInteger(style);
            return Number.TryParseInt32(s, style, NumberFormatInfo.GetInstance(provider), out result);
        }
    }
}
