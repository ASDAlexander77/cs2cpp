namespace System
{
    using System.Globalization;

    public struct Single
    {
        internal float m_value;

        //
        // Public constants
        //
        public const float MinValue = (float)-3.40282346638528859e+38;
        public const float Epsilon = (float)1.4e-45;
        public const float MaxValue = (float)3.40282346638528859e+38;
        public const float PositiveInfinity = (float)1.0 / (float)0.0;
        public const float NegativeInfinity = (float)-1.0 / (float)0.0;
        public const float NaN = (float)0.0 / (float)0.0;

        public unsafe static bool IsInfinity(float f)
        {
            return (*(int*)(&f) & 0x7FFFFFFF) == 0x7F800000;
        }

        public unsafe static bool IsPositiveInfinity(float f)
        {
            return *(int*)(&f) == 0x7F800000;
        }

        public unsafe static bool IsNegativeInfinity(float f)
        {
            return *(int*)(&f) == unchecked((int)0xFF800000);
        }

        public unsafe static bool IsNaN(float f)
        {
            return (*(int*)(&f) & 0x7FFFFFFF) > 0x7F800000;
        }

        // Compares this object to another object, returning an integer that
        // indicates the relationship.
        // Returns a value less than zero if this  object
        // null is considered to be less than any instance.
        // If object is not of type Single, this method throws an ArgumentException.
        //
        public int CompareTo(Object value)
        {
            if (value == null)
            {
                return 1;
            }
            if (value is Single)
            {
                float f = (float)value;
                if (m_value < f) return -1;
                if (m_value > f) return 1;
                if (m_value == f) return 0;

                // At least one of the values is NaN.
                if (IsNaN(m_value))
                    return (IsNaN(f) ? 0 : -1);
                else // f is NaN.
                    return 1;
            }
            throw new ArgumentException(Environment.GetResourceString("Arg_MustBeSingle"));
        }


        public int CompareTo(Single value)
        {
            if (m_value < value) return -1;
            if (m_value > value) return 1;
            if (m_value == value) return 0;

            // At least one of the values is NaN.
            if (IsNaN(m_value))
                return (IsNaN(value) ? 0 : -1);
            else // f is NaN.
                return 1;
        }

        public static bool operator ==(Single left, Single right)
        {
            return left == right;
        }

        public static bool operator !=(Single left, Single right)
        {
            return left != right;
        }

        public static bool operator <(Single left, Single right)
        {
            return left < right;
        }

        public static bool operator >(Single left, Single right)
        {
            return left > right;
        }

        public static bool operator <=(Single left, Single right)
        {
            return left <= right;
        }

        public static bool operator >=(Single left, Single right)
        {
            return left >= right;
        }

        public override bool Equals(Object obj)
        {
            if (!(obj is Single))
            {
                return false;
            }
            float temp = ((Single)obj).m_value;
            if (temp == m_value)
            {
                return true;
            }

            return IsNaN(temp) && IsNaN(m_value);
        }

        public bool Equals(Single obj)
        {
            if (obj == m_value)
            {
                return true;
            }

            return IsNaN(obj) && IsNaN(m_value);
        }

        public unsafe override int GetHashCode()
        {
            float f = m_value;
            if (f == 0)
            {
                // Ensure that 0 and -0 have the same hash code
                return 0;
            }
            int v = *(int*)(&f);
            return v;
        }

        public override String ToString()
        {
            return Number.FormatSingle(m_value, null, NumberFormatInfo.CurrentInfo);
        }

        public String ToString(IFormatProvider provider)
        {
            return Number.FormatSingle(m_value, null, NumberFormatInfo.GetInstance(provider));
        }

        public String ToString(String format)
        {
            return Number.FormatSingle(m_value, format, NumberFormatInfo.CurrentInfo);
        }

        public String ToString(String format, IFormatProvider provider)
        {
            return Number.FormatSingle(m_value, format, NumberFormatInfo.GetInstance(provider));
        }

        // Parses a float from a String in the given style.  If
        // a NumberFormatInfo isn't specified, the current culture's
        // NumberFormatInfo is assumed.
        //
        // This method will not throw an OverflowException, but will return
        // PositiveInfinity or NegativeInfinity for a number that is too
        // large or too small.
        //
        public static float Parse(String s)
        {
            return Parse(s, NumberStyles.Float | NumberStyles.AllowThousands, NumberFormatInfo.CurrentInfo);
        }

        public static float Parse(String s, NumberStyles style)
        {
            NumberFormatInfo.ValidateParseStyleFloatingPoint(style);
            return Parse(s, style, NumberFormatInfo.CurrentInfo);
        }

        public static float Parse(String s, IFormatProvider provider)
        {
            return Parse(s, NumberStyles.Float | NumberStyles.AllowThousands, NumberFormatInfo.GetInstance(provider));
        }

        public static float Parse(String s, NumberStyles style, IFormatProvider provider)
        {
            NumberFormatInfo.ValidateParseStyleFloatingPoint(style);
            return Parse(s, style, NumberFormatInfo.GetInstance(provider));
        }

        private static float Parse(String s, NumberStyles style, NumberFormatInfo info)
        {
            return Number.ParseSingle(s, style, info);
        }

        public static Boolean TryParse(String s, out Single result)
        {
            return TryParse(s, NumberStyles.Float | NumberStyles.AllowThousands, NumberFormatInfo.CurrentInfo, out result);
        }

        public static Boolean TryParse(String s, NumberStyles style, IFormatProvider provider, out Single result)
        {
            NumberFormatInfo.ValidateParseStyleFloatingPoint(style);
            return TryParse(s, style, NumberFormatInfo.GetInstance(provider), out result);
        }

        private static Boolean TryParse(String s, NumberStyles style, NumberFormatInfo info, out Single result)
        {
            if (s == null)
            {
                result = 0;
                return false;
            }
            bool success = Number.TryParseSingle(s, style, info, out result);
            if (!success)
            {
                String sTrim = s.Trim();
                if (sTrim.Equals(info.PositiveInfinitySymbol))
                {
                    result = PositiveInfinity;
                }
                else if (sTrim.Equals(info.NegativeInfinitySymbol))
                {
                    result = NegativeInfinity;
                }
                else if (sTrim.Equals(info.NaNSymbol))
                {
                    result = NaN;
                }
                else
                    return false; // We really failed
            }
            return true;

        }
    }
}
