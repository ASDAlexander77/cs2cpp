namespace System
{
    using System.Globalization;
    using System.Runtime.InteropServices;

    public struct SByte
    {
        private sbyte m_value;

        // The maximum value that a Byte may represent: 127.
        public const sbyte MaxValue = (sbyte)0x7F;

        // The minimum value that a Byte may represent: -128.
        public const sbyte MinValue = unchecked((sbyte)0x80);


        // Compares this object to another object, returning an integer that
        // indicates the relationship. 
        // Returns a value less than zero if this  object
        // null is considered to be less than any instance.
        // If object is not of type SByte, this method throws an ArgumentException.
        // 
        public int CompareTo(Object obj)
        {
            if (obj == null)
            {
                return 1;
            }
            if (!(obj is SByte))
            {
                throw new ArgumentException(Environment.GetResourceString("Arg_MustBeSByte"));
            }
            return m_value - ((SByte)obj).m_value;
        }

        public int CompareTo(SByte value)
        {
            return m_value - value;
        }

        // Determines whether two Byte objects are equal.
        public override bool Equals(Object obj)
        {
            if (!(obj is SByte))
            {
                return false;
            }
            return m_value == ((SByte)obj).m_value;
        }

        public bool Equals(SByte obj)
        {
            return m_value == obj;
        }

        // Gets a hash code for this instance.
        public override int GetHashCode()
        {
            return ((int)m_value ^ (int)m_value << 8);
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
                uint temp = (uint)(m_value & 0x000000FF);
                return Number.FormatUInt32(temp, format, info);
            }
            return Number.FormatInt32(m_value, format, info);
        }

        [CLSCompliant(false)]
        public static sbyte Parse(String s)
        {
            return Parse(s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo);
        }

        [CLSCompliant(false)]
        public static sbyte Parse(String s, NumberStyles style)
        {
            NumberFormatInfo.ValidateParseStyleInteger(style);
            return Parse(s, style, NumberFormatInfo.CurrentInfo);
        }

        [CLSCompliant(false)]
        public static sbyte Parse(String s, IFormatProvider provider)
        {
            return Parse(s, NumberStyles.Integer, NumberFormatInfo.GetInstance(provider));
        }

        // Parses a signed byte from a String in the given style.  If
        // a NumberFormatInfo isn't specified, the current culture's 
        // NumberFormatInfo is assumed.
        // 
        [CLSCompliant(false)]
        public static sbyte Parse(String s, NumberStyles style, IFormatProvider provider)
        {
            NumberFormatInfo.ValidateParseStyleInteger(style);
            return Parse(s, style, NumberFormatInfo.GetInstance(provider));
        }

        private static sbyte Parse(String s, NumberStyles style, NumberFormatInfo info)
        {

            int i = 0;
            try
            {
                i = Number.ParseInt32(s, style, info);
            }
            catch (OverflowException e)
            {
                throw new OverflowException(Environment.GetResourceString("Overflow_SByte"));
            }

            if ((style & NumberStyles.AllowHexSpecifier) != 0)
            { // We are parsing a hexadecimal number
                if ((i < 0) || i > Byte.MaxValue)
                {
                    throw new OverflowException(Environment.GetResourceString("Overflow_SByte"));
                }
                return (sbyte)i;
            }

            if (i < MinValue || i > MaxValue) throw new OverflowException(Environment.GetResourceString("Overflow_SByte"));
            return (sbyte)i;
        }

        [CLSCompliant(false)]
        public static bool TryParse(String s, out SByte result)
        {
            return TryParse(s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out result);
        }

        [CLSCompliant(false)]
        public static bool TryParse(String s, NumberStyles style, IFormatProvider provider, out SByte result)
        {
            NumberFormatInfo.ValidateParseStyleInteger(style);
            return TryParse(s, style, NumberFormatInfo.GetInstance(provider), out result);
        }

        private static bool TryParse(String s, NumberStyles style, NumberFormatInfo info, out SByte result)
        {

            result = 0;
            int i;
            if (!Number.TryParseInt32(s, style, info, out i))
            {
                return false;
            }

            if ((style & NumberStyles.AllowHexSpecifier) != 0)
            { // We are parsing a hexadecimal number
                if ((i < 0) || i > Byte.MaxValue)
                {
                    return false;
                }
                result = (sbyte)i;
                return true;
            }

            if (i < MinValue || i > MaxValue)
            {
                return false;
            }
            result = (sbyte)i;
            return true;
        }
    }
}
