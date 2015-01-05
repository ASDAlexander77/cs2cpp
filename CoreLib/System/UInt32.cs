namespace System
{
    using System.Globalization;
    using System.Runtime.InteropServices;

    public struct UInt32
    {
        private uint m_value;

        public const uint MaxValue = (uint)0xffffffff;
        public const uint MinValue = 0U;


        // Compares this object to another object, returning an integer that
        // indicates the relationship. 
        // Returns a value less than zero if this  object
        // null is considered to be less than any instance.
        // If object is not of type UInt32, this method throws an ArgumentException.
        // 
        public int CompareTo(Object value)
        {
            if (value == null)
            {
                return 1;
            }
            if (value is UInt32)
            {
                // Need to use compare because subtraction will wrap
                // to positive for very large neg numbers, etc.
                uint i = (uint)value;
                if (m_value < i) return -1;
                if (m_value > i) return 1;
                return 0;
            }
            throw new ArgumentException(Environment.GetResourceString("Arg_MustBeUInt32"));
        }

        public int CompareTo(UInt32 value)
        {
            // Need to use compare because subtraction will wrap
            // to positive for very large neg numbers, etc.
            if (m_value < value) return -1;
            if (m_value > value) return 1;
            return 0;
        }

        public override bool Equals(Object obj)
        {
            if (!(obj is UInt32))
            {
                return false;
            }
            return m_value == ((UInt32)obj).m_value;
        }

        public bool Equals(UInt32 obj)
        {
            return m_value == obj;
        }

        public override int GetHashCode()
        {
            return ((int)m_value);
        }

        public override String ToString()
        {
            return Number.FormatUInt32(m_value, null, NumberFormatInfo.CurrentInfo);
        }

        public String ToString(IFormatProvider provider)
        {
            return Number.FormatUInt32(m_value, null, NumberFormatInfo.GetInstance(provider));
        }

        public String ToString(String format)
        {
            return Number.FormatUInt32(m_value, format, NumberFormatInfo.CurrentInfo);
        }

        public String ToString(String format, IFormatProvider provider)
        {
            return Number.FormatUInt32(m_value, format, NumberFormatInfo.GetInstance(provider));
        }

        [CLSCompliant(false)]
        public static uint Parse(String s)
        {
            return Number.ParseUInt32(s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo);
        }

        [CLSCompliant(false)]
        public static uint Parse(String s, NumberStyles style)
        {
            NumberFormatInfo.ValidateParseStyleInteger(style);
            return Number.ParseUInt32(s, style, NumberFormatInfo.CurrentInfo);
        }


        [CLSCompliant(false)]
        public static uint Parse(String s, IFormatProvider provider)
        {
            return Number.ParseUInt32(s, NumberStyles.Integer, NumberFormatInfo.GetInstance(provider));
        }

        [CLSCompliant(false)]
        public static uint Parse(String s, NumberStyles style, IFormatProvider provider)
        {
            NumberFormatInfo.ValidateParseStyleInteger(style);
            return Number.ParseUInt32(s, style, NumberFormatInfo.GetInstance(provider));
        }

        [CLSCompliant(false)]
        public static bool TryParse(String s, out UInt32 result)
        {
            return Number.TryParseUInt32(s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out result);
        }

        [CLSCompliant(false)]
        public static bool TryParse(String s, NumberStyles style, IFormatProvider provider, out UInt32 result)
        {
            NumberFormatInfo.ValidateParseStyleInteger(style);
            return Number.TryParseUInt32(s, style, NumberFormatInfo.GetInstance(provider), out result);
        }

    }
}
