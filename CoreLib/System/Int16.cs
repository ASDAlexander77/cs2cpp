namespace System
{
    using System.Globalization;
    using System.Runtime.InteropServices;

    public struct Int16
    {
        internal short m_value;

        public const short MaxValue = (short)0x7FFF;
        public const short MinValue = unchecked((short)0x8000);

        // Compares this object to another object, returning an integer that
        // indicates the relationship. 
        // Returns a value less than zero if this  object
        // null is considered to be less than any instance.
        // If object is not of type Int16, this method throws an ArgumentException.
        // 
        public int CompareTo(Object value)
        {
            if (value == null)
            {
                return 1;
            }

            if (value is Int16)
            {
                return m_value - ((Int16)value).m_value;
            }

            throw new ArgumentException(Environment.GetResourceString("Arg_MustBeInt16"));
        }

        public int CompareTo(Int16 value)
        {
            return m_value - value;
        }

        public override bool Equals(Object obj)
        {
            if (!(obj is Int16))
            {
                return false;
            }
            return m_value == ((Int16)obj).m_value;
        }

        public bool Equals(Int16 obj)
        {
            return m_value == obj;
        }

        // Returns a HashCode for the Int16
        public override int GetHashCode()
        {
            return ((int)((ushort)m_value) | (((int)m_value) << 16));
        }


        public override String ToString()
        {
            return Number.FormatInt32(m_value, null, NumberFormatInfo.CurrentInfo);
        }

        public String ToString(IFormatProvider provider)
        {
            return Number.FormatInt32(m_value, null, NumberFormatInfo.GetInstance(provider));
        }

        public String ToString(String format)
        {
            return ToString(format, NumberFormatInfo.CurrentInfo);
        }

        public String ToString(String format, IFormatProvider provider)
        {
            return ToString(format, NumberFormatInfo.GetInstance(provider));
        }

        private String ToString(String format, NumberFormatInfo info)
        {
            if (m_value < 0 && format != null && format.Length > 0 && (format[0] == 'X' || format[0] == 'x'))
            {
                uint temp = (uint)(m_value & 0x0000FFFF);
                return Number.FormatUInt32(temp, format, info);
            }
            return Number.FormatInt32(m_value, format, info);
        }

        public static short Parse(String s)
        {
            return Parse(s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo);
        }

        public static short Parse(String s, NumberStyles style)
        {
            NumberFormatInfo.ValidateParseStyleInteger(style);
            return Parse(s, style, NumberFormatInfo.CurrentInfo);
        }

        public static short Parse(String s, IFormatProvider provider)
        {
            return Parse(s, NumberStyles.Integer, NumberFormatInfo.GetInstance(provider));
        }

        public static short Parse(String s, NumberStyles style, IFormatProvider provider)
        {
            NumberFormatInfo.ValidateParseStyleInteger(style);
            return Parse(s, style, NumberFormatInfo.GetInstance(provider));
        }

        private static short Parse(String s, NumberStyles style, NumberFormatInfo info)
        {

            int i = 0;
            try
            {
                i = Number.ParseInt32(s, style, info);
            }
            catch (OverflowException e)
            {
                throw new OverflowException(Environment.GetResourceString("Overflow_Int16"));
            }

            // We need this check here since we don't allow signs to specified in hex numbers. So we fixup the result
            // for negative numbers
            if ((style & NumberStyles.AllowHexSpecifier) != 0)
            { // We are parsing a hexadecimal number
                if ((i < 0) || (i > UInt16.MaxValue))
                {
                    throw new OverflowException(Environment.GetResourceString("Overflow_Int16"));
                }
                return (short)i;
            }

            if (i < MinValue || i > MaxValue) throw new OverflowException(Environment.GetResourceString("Overflow_Int16"));
            return (short)i;
        }

        public static bool TryParse(String s, out Int16 result)
        {
            return TryParse(s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out result);
        }

        public static bool TryParse(String s, NumberStyles style, IFormatProvider provider, out Int16 result)
        {
            NumberFormatInfo.ValidateParseStyleInteger(style);
            return TryParse(s, style, NumberFormatInfo.GetInstance(provider), out result);
        }

        private static bool TryParse(String s, NumberStyles style, NumberFormatInfo info, out Int16 result)
        {

            result = 0;
            int i;
            if (!Number.TryParseInt32(s, style, info, out i))
            {
                return false;
            }

            // We need this check here since we don't allow signs to specified in hex numbers. So we fixup the result
            // for negative numbers
            if ((style & NumberStyles.AllowHexSpecifier) != 0)
            { // We are parsing a hexadecimal number
                if ((i < 0) || i > UInt16.MaxValue)
                {
                    return false;
                }
                result = (Int16)i;
                return true;
            }

            if (i < MinValue || i > MaxValue)
            {
                return false;
            }
            result = (Int16)i;
            return true;
        }
    }
}
