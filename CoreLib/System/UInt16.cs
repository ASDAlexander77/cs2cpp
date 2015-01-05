namespace System
{
    using System.Globalization;
    using System.Runtime.InteropServices;

    public struct UInt16
    {
        private ushort m_value;

        public const ushort MaxValue = (ushort)0xFFFF;
        public const ushort MinValue = 0;


        // Compares this object to another object, returning an integer that
        // indicates the relationship. 
        // Returns a value less than zero if this  object
        // null is considered to be less than any instance.
        // If object is not of type UInt16, this method throws an ArgumentException.
        // 
        public int CompareTo(Object value)
        {
            if (value == null)
            {
                return 1;
            }
            if (value is UInt16)
            {
                return ((int)m_value - (int)(((UInt16)value).m_value));
            }
            throw new ArgumentException(Environment.GetResourceString("Arg_MustBeUInt16"));
        }

        public int CompareTo(UInt16 value)
        {
            return ((int)m_value - (int)value);
        }

        public override bool Equals(Object obj)
        {
            if (!(obj is UInt16))
            {
                return false;
            }
            return m_value == ((UInt16)obj).m_value;
        }

        public bool Equals(UInt16 obj)
        {
            return m_value == obj;
        }

        // Returns a HashCode for the UInt16
        public override int GetHashCode()
        {
            return (int)m_value;
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
        public static ushort Parse(String s)
        {
            return Parse(s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo);
        }

        [CLSCompliant(false)]
        public static ushort Parse(String s, NumberStyles style)
        {
            NumberFormatInfo.ValidateParseStyleInteger(style);
            return Parse(s, style, NumberFormatInfo.CurrentInfo);
        }


        [CLSCompliant(false)]
        public static ushort Parse(String s, IFormatProvider provider)
        {
            return Parse(s, NumberStyles.Integer, NumberFormatInfo.GetInstance(provider));
        }

        [CLSCompliant(false)]
        public static ushort Parse(String s, NumberStyles style, IFormatProvider provider)
        {
            NumberFormatInfo.ValidateParseStyleInteger(style);
            return Parse(s, style, NumberFormatInfo.GetInstance(provider));
        }

        private static ushort Parse(String s, NumberStyles style, NumberFormatInfo info)
        {

            uint i = 0;
            try
            {
                i = Number.ParseUInt32(s, style, info);
            }
            catch (OverflowException e)
            {
                throw new OverflowException(Environment.GetResourceString("Overflow_UInt16"));
            }

            if (i > MaxValue) throw new OverflowException(Environment.GetResourceString("Overflow_UInt16"));
            return (ushort)i;
        }

        [CLSCompliant(false)]
        public static bool TryParse(String s, out UInt16 result)
        {
            return TryParse(s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out result);
        }

        [CLSCompliant(false)]
        public static bool TryParse(String s, NumberStyles style, IFormatProvider provider, out UInt16 result)
        {
            NumberFormatInfo.ValidateParseStyleInteger(style);
            return TryParse(s, style, NumberFormatInfo.GetInstance(provider), out result);
        }

        private static bool TryParse(String s, NumberStyles style, NumberFormatInfo info, out UInt16 result)
        {

            result = 0;
            UInt32 i;
            if (!Number.TryParseUInt32(s, style, info, out i))
            {
                return false;
            }
            if (i > MaxValue)
            {
                return false;
            }
            result = (UInt16)i;
            return true;
        }
    }
}
