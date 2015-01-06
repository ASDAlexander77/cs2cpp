namespace System
{
    using System;
    using System.Globalization;
    using System.Runtime.CompilerServices;
    using System.Text;

    // The Number class implements methods for formatting and parsing
    // numeric values. To format and parse numeric values, applications should
    // use the Format and Parse methods provided by the numeric
    // classes (Byte, Int16, Int32, Int64,
    // Single, Double, Currency, and Decimal). Those
    // Format and Parse methods share a common implementation
    // provided by this class, and are thus documented in detail here.
    //
    // Formatting
    //
    // The Format methods provided by the numeric classes are all of the
    // form
    //
    //  public static String Format(XXX value, String format);
    //  public static String Format(XXX value, String format, NumberFormatInfo info);
    //
    // where XXX is the name of the particular numeric class. The methods convert
    // the numeric value to a string using the format string given by the
    // format parameter. If the format parameter is null or
    // an empty string, the number is formatted as if the string "G" (general
    // format) was specified. The info parameter specifies the
    // NumberFormatInfo instance to use when formatting the number. If the
    // info parameter is null or omitted, the numeric formatting information
    // is obtained from the current culture. The NumberFormatInfo supplies
    // such information as the characters to use for decimal and thousand
    // separators, and the spelling and placement of currency symbols in monetary
    // values.
    //
    // Format strings fall into two categories: Standard format strings and
    // user-defined format strings. A format string consisting of a single
    // alphabetic character (A-Z or a-z), optionally followed by a sequence of
    // digits (0-9), is a standard format string. All other format strings are
    // used-defined format strings.
    //
    // A standard format string takes the form Axx, where A is an
    // alphabetic character called the format specifier and xx is a
    // sequence of digits called the precision specifier. The format
    // specifier controls the type of formatting applied to the number and the
    // precision specifier controls the number of significant digits or decimal
    // places of the formatting operation. The following table describes the
    // supported standard formats.
    //
    // C c - Currency format. The number is
    // converted to a string that represents a currency amount. The conversion is
    // controlled by the currency format information of the NumberFormatInfo
    // used to format the number. The precision specifier indicates the desired
    // number of decimal places. If the precision specifier is omitted, the default
    // currency precision given by the NumberFormatInfo is used.
    //
    // D d - Decimal format. This format is
    // supported for integral types only. The number is converted to a string of
    // decimal digits, prefixed by a minus sign if the number is negative. The
    // precision specifier indicates the minimum number of digits desired in the
    // resulting string. If required, the number will be left-padded with zeros to
    // produce the number of digits given by the precision specifier.
    //
    // E e Engineering (scientific) format.
    // The number is converted to a string of the form
    // "-d.ddd...E+ddd" or "-d.ddd...e+ddd", where each
    // 'd' indicates a digit (0-9). The string starts with a minus sign if the
    // number is negative, and one digit always precedes the decimal point. The
    // precision specifier indicates the desired number of digits after the decimal
    // point. If the precision specifier is omitted, a default of 6 digits after
    // the decimal point is used. The format specifier indicates whether to prefix
    // the exponent with an 'E' or an 'e'. The exponent is always consists of a
    // plus or minus sign and three digits.
    //
    // F f Fixed point format. The number is
    // converted to a string of the form "-ddd.ddd....", where each
    // 'd' indicates a digit (0-9). The string starts with a minus sign if the
    // number is negative. The precision specifier indicates the desired number of
    // decimal places. If the precision specifier is omitted, the default numeric
    // precision given by the NumberFormatInfo is used.
    //
    // G g - General format. The number is
    // converted to the shortest possible decimal representation using fixed point
    // or scientific format. The precision specifier determines the number of
    // significant digits in the resulting string. If the precision specifier is
    // omitted, the number of significant digits is determined by the type of the
    // number being converted (10 for int, 19 for long, 7 for
    // float, 15 for double, 19 for Currency, and 29 for
    // Decimal). Trailing zeros after the decimal point are removed, and the
    // resulting string contains a decimal point only if required. The resulting
    // string uses fixed point format if the exponent of the number is less than
    // the number of significant digits and greater than or equal to -4. Otherwise,
    // the resulting string uses scientific format, and the case of the format
    // specifier controls whether the exponent is prefixed with an 'E' or an
    // 'e'.
    //
    // N n Number format. The number is
    // converted to a string of the form "-d,ddd,ddd.ddd....", where
    // each 'd' indicates a digit (0-9). The string starts with a minus sign if the
    // number is negative. Thousand separators are inserted between each group of
    // three digits to the left of the decimal point. The precision specifier
    // indicates the desired number of decimal places. If the precision specifier
    // is omitted, the default numeric precision given by the
    // NumberFormatInfo is used.
    //
    // X x - Hexadecimal format. This format is
    // supported for integral types only. The number is converted to a string of
    // hexadecimal digits. The format specifier indicates whether to use upper or
    // lower case characters for the hexadecimal digits above 9 ('X' for 'ABCDEF',
    // and 'x' for 'abcdef'). The precision specifier indicates the minimum number
    // of digits desired in the resulting string. If required, the number will be
    // left-padded with zeros to produce the number of digits given by the
    // precision specifier.
    //
    // Some examples of standard format strings and their results are shown in the
    // table below. (The examples all assume a default NumberFormatInfo.)
    //
    // Value        Format  Result
    // 12345.6789   C       $12,345.68
    // -12345.6789  C       ($12,345.68)
    // 12345        D       12345
    // 12345        D8      00012345
    // 12345.6789   E       1.234568E+004
    // 12345.6789   E10     1.2345678900E+004
    // 12345.6789   e4      1.2346e+004
    // 12345.6789   F       12345.68
    // 12345.6789   F0      12346
    // 12345.6789   F6      12345.678900
    // 12345.6789   G       12345.6789
    // 12345.6789   G7      12345.68
    // 123456789    G7      1.234568E8
    // 12345.6789   N       12,345.68
    // 123456789    N4      123,456,789.0000
    // 0x2c45e      x       2c45e
    // 0x2c45e      X       2C45E
    // 0x2c45e      X8      0002C45E
    //
    // Format strings that do not start with an alphabetic character, or that start
    // with an alphabetic character followed by a non-digit, are called
    // user-defined format strings. The following table describes the formatting
    // characters that are supported in user defined format strings.
    //
    // 
    // 0 - Digit placeholder. If the value being
    // formatted has a digit in the position where the '0' appears in the format
    // string, then that digit is copied to the output string. Otherwise, a '0' is
    // stored in that position in the output string. The position of the leftmost
    // '0' before the decimal point and the rightmost '0' after the decimal point
    // determines the range of digits that are always present in the output
    // string.
    //
    // # - Digit placeholder. If the value being
    // formatted has a digit in the position where the '#' appears in the format
    // string, then that digit is copied to the output string. Otherwise, nothing
    // is stored in that position in the output string.
    //
    // . - Decimal point. The first '.' character
    // in the format string determines the location of the decimal separator in the
    // formatted value; any additional '.' characters are ignored. The actual
    // character used as a the decimal separator in the output string is given by
    // the NumberFormatInfo used to format the number.
    //
    // , - Thousand separator and number scaling.
    // The ',' character serves two purposes. First, if the format string contains
    // a ',' character between two digit placeholders (0 or #) and to the left of
    // the decimal point if one is present, then the output will have thousand
    // separators inserted between each group of three digits to the left of the
    // decimal separator. The actual character used as a the decimal separator in
    // the output string is given by the NumberFormatInfo used to format the
    // number. Second, if the format string contains one or more ',' characters
    // immediately to the left of the decimal point, or after the last digit
    // placeholder if there is no decimal point, then the number will be divided by
    // 1000 times the number of ',' characters before it is formatted. For example,
    // the format string '0,,' will represent 100 million as just 100. Use of the
    // ',' character to indicate scaling does not also cause the formatted number
    // to have thousand separators. Thus, to scale a number by 1 million and insert
    // thousand separators you would use the format string '#,##0,,'.
    //
    // % - Percentage placeholder. The presence of
    // a '%' character in the format string causes the number to be multiplied by
    // 100 before it is formatted. The '%' character itself is inserted in the
    // output string where it appears in the format string.
    //
    // E+ E- e+ e-   - Scientific notation.
    // If any of the strings 'E+', 'E-', 'e+', or 'e-' are present in the format
    // string and are immediately followed by at least one '0' character, then the
    // number is formatted using scientific notation with an 'E' or 'e' inserted
    // between the number and the exponent. The number of '0' characters following
    // the scientific notation indicator determines the minimum number of digits to
    // output for the exponent. The 'E+' and 'e+' formats indicate that a sign
    // character (plus or minus) should always precede the exponent. The 'E-' and
    // 'e-' formats indicate that a sign character should only precede negative
    // exponents.
    //
    // \ - Literal character. A backslash character
    // causes the next character in the format string to be copied to the output
    // string as-is. The backslash itself isn't copied, so to place a backslash
    // character in the output string, use two backslashes (\\) in the format
    // string.
    //
    // 'ABC' "ABC" - Literal string. Characters
    // enclosed in single or double quotation marks are copied to the output string
    // as-is and do not affect formatting.
    //
    // ; - Section separator. The ';' character is
    // used to separate sections for positive, negative, and zero numbers in the
    // format string.
    //
    // Other - All other characters are copied to
    // the output string in the position they appear.
    //
    // For fixed point formats (formats not containing an 'E+', 'E-', 'e+', or
    // 'e-'), the number is rounded to as many decimal places as there are digit
    // placeholders to the right of the decimal point. If the format string does
    // not contain a decimal point, the number is rounded to the nearest
    // integer. If the number has more digits than there are digit placeholders to
    // the left of the decimal point, the extra digits are copied to the output
    // string immediately before the first digit placeholder.
    //
    // For scientific formats, the number is rounded to as many significant digits
    // as there are digit placeholders in the format string.
    //
    // To allow for different formatting of positive, negative, and zero values, a
    // user-defined format string may contain up to three sections separated by
    // semicolons. The results of having one, two, or three sections in the format
    // string are described in the table below.
    //
    // Sections:
    //
    // One - The format string applies to all values.
    //
    // Two - The first section applies to positive values
    // and zeros, and the second section applies to negative values. If the number
    // to be formatted is negative, but becomes zero after rounding according to
    // the format in the second section, then the resulting zero is formatted
    // according to the first section.
    //
    // Three - The first section applies to positive
    // values, the second section applies to negative values, and the third section
    // applies to zeros. The second section may be left empty (by having no
    // characters between the semicolons), in which case the first section applies
    // to all non-zero values. If the number to be formatted is non-zero, but
    // becomes zero after rounding according to the format in the first or second
    // section, then the resulting zero is formatted according to the third
    // section.
    //
    // For both standard and user-defined formatting operations on values of type
    // float and double, if the value being formatted is a NaN (Not
    // a Number) or a positive or negative infinity, then regardless of the format
    // string, the resulting string is given by the NaNSymbol,
    // PositiveInfinitySymbol, or NegativeInfinitySymbol property of
    // the NumberFormatInfo used to format the number.
    //
    // Parsing
    //
    // The Parse methods provided by the numeric classes are all of the form
    //
    //  public static XXX Parse(String s);
    //  public static XXX Parse(String s, int style);
    //  public static XXX Parse(String s, int style, NumberFormatInfo info);
    //
    // where XXX is the name of the particular numeric class. The methods convert a
    // string to a numeric value. The optional style parameter specifies the
    // permitted style of the numeric string. It must be a combination of bit flags
    // from the NumberStyles enumeration. The optional info parameter
    // specifies the NumberFormatInfo instance to use when parsing the
    // string. If the info parameter is null or omitted, the numeric
    // formatting information is obtained from the current culture.
    //
    // Numeric strings produced by the Format methods using the Currency,
    // Decimal, Engineering, Fixed point, General, or Number standard formats
    // (the C, D, E, F, G, and N format specifiers) are guaranteed to be parseable
    // by the Parse methods if the NumberStyles.Any style is
    // specified. Note, however, that the Parse methods do not accept
    // NaNs or Infinities.
    //
    //This class contains only static members and does not need to be serializable 

    internal class Number
    {
        public const int INT32_PRECISION = 10;
        public const int UINT32_PRECISION = INT32_PRECISION;
        public const int long_PRECISION = 19;
        public const int Ulong_PRECISION = 20;
        public const int FLOAT_PRECISION = 7;
        public const int DOUBLE_PRECISION = 15;
        public const int LARGE_BUFFER_SIZE = 600;
        public const int MIN_BUFFER_SIZE = 105;
        public const uint SCALE_NAN = 0x80000000;
        public const uint SCALE_INF = 0x7FFFFFFF;
        public const int DECIMAL_PRECISION = 29;

        [MethodImplAttribute(MethodImplOptions.Unmanaged)]
        public static extern double modf(double x, ref double intpart);

        private Number()
        {
        }

        public static String FormatDecimal(Decimal value, String format, NumberFormatInfo info)
        {
            throw new NotImplementedException();
        }

        public static String FormatInt32(int value, String format, NumberFormatInfo info)
        {
            int digits;
            var fmt = ParseFormatSpecifier(format, out digits);

            var retString = string.Empty;

            NUMBER number;

            //ANDing fmt with FFDF has the effect of uppercasing the character because
            //we've removed the bit that marks lower-case.
            switch (fmt & 0xFFDF)
            {
                case 'G':
                    if (digits > 0)
                    {
                        number = new NUMBER();
                        Int32ToNumber(value, ref number);
                        if (fmt != 0)
                        {
                            retString = NumberToString(ref number, fmt, digits, info);
                            break;
                        }
                        retString = NumberToStringFormat(ref number, format, info);
                        break;
                    }

                    goto case 'D';

                // fall through
                case 'D':
                    retString = Int32ToDecStr(value, digits, info.NegativeSign);
                    break;
                case 'X':
                    retString = Int32ToHexStr((uint)value, fmt - ('X' - 'A' + 10), digits);
                    break;
                default:
                    number = new NUMBER();
                    Int32ToNumber(value, ref number);
                    if (fmt != 0)
                    {
                        retString = NumberToString(ref number, fmt, digits, info);
                        break;
                    }
                    retString = NumberToStringFormat(ref number, format, info);
                    break;

            }

            return retString;
        }

        public static String FormatUInt32(uint value, String format, NumberFormatInfo info)
        {
            int digits;
            var fmt = ParseFormatSpecifier(format, out digits);

            var retString = string.Empty;

            NUMBER number;

            //ANDing fmt with FFDF has the effect of uppercasing the character because
            //we've removed the bit that marks lower-case.
            switch (fmt & 0xFFDF)
            {
                case 'G':
                    if (digits > 0)
                    {
                        number = new NUMBER();
                        UInt32ToNumber(value, ref number);
                        if (fmt != 0)
                        {
                            retString = NumberToString(ref number, fmt, digits, info);
                            break;
                        }
                        retString = NumberToStringFormat(ref number, format, info);
                        break;
                    }

                    goto case 'D';

                // fall through
                case 'D':
                    retString = UInt32ToDecStr(value, digits);
                    break;
                case 'X':
                    retString = Int32ToHexStr((uint)value, fmt - ('X' - 'A' + 10), digits);
                    break;
                default:
                    number = new NUMBER();
                    UInt32ToNumber(value, ref number);
                    if (fmt != 0)
                    {
                        retString = NumberToString(ref number, fmt, digits, info);
                        break;
                    }
                    retString = NumberToStringFormat(ref number, format, info);
                    break;

            }

            return retString;
        }

        public static String FormatInt64(long value, String format, NumberFormatInfo info)
        {
            int digits;
            var fmt = ParseFormatSpecifier(format, out digits);

            var retString = string.Empty;

            NUMBER number;

            //ANDing fmt with FFDF has the effect of uppercasing the character because
            //we've removed the bit that marks lower-case.
            switch (fmt & 0xFFDF)
            {
                case 'G':
                    if (digits > 0)
                    {
                        number = new NUMBER();
                        Int64ToNumber(value, ref number);
                        if (fmt != 0)
                        {
                            retString = NumberToString(ref number, fmt, digits, info);
                            break;
                        }
                        retString = NumberToStringFormat(ref number, format, info);
                        break;
                    }

                    goto case 'D';

                // fall through
                case 'D':
                    retString = Int64ToDecStr(value, digits, info.NegativeSign);
                    break;
                case 'X':
                    retString = Int64ToHexStr((uint)value, fmt - ('X' - 'A' + 10), digits);
                    break;
                default:
                    number = new NUMBER();
                    Int64ToNumber(value, ref number);
                    if (fmt != 0)
                    {
                        retString = NumberToString(ref number, fmt, digits, info);
                        break;
                    }
                    retString = NumberToStringFormat(ref number, format, info);
                    break;

            }

            return retString;
        }

        public static String FormatUInt64(ulong value, String format, NumberFormatInfo info)
        {
            int digits;
            var fmt = ParseFormatSpecifier(format, out digits);

            var retString = string.Empty;

            NUMBER number;

            //ANDing fmt with FFDF has the effect of uppercasing the character because
            //we've removed the bit that marks lower-case.
            switch (fmt & 0xFFDF)
            {
                case 'G':
                    if (digits > 0)
                    {
                        number = new NUMBER();
                        UInt64ToNumber(value, ref number);
                        if (fmt != 0)
                        {
                            retString = NumberToString(ref number, fmt, digits, info);
                            break;
                        }
                        retString = NumberToStringFormat(ref number, format, info);
                        break;
                    }

                    goto case 'D';

                // fall through
                case 'D':
                    retString = UInt64ToDecStr(value, digits);
                    break;
                case 'X':
                    retString = Int64ToHexStr((uint)value, fmt - ('X' - 'A' + 10), digits);
                    break;
                default:
                    number = new NUMBER();
                    UInt64ToNumber(value, ref number);
                    if (fmt != 0)
                    {
                        retString = NumberToString(ref number, fmt, digits, info);
                        break;
                    }
                    retString = NumberToStringFormat(ref number, format, info);
                    break;

            }

            return retString;
        }

        public static String FormatDouble(double value, String format, NumberFormatInfo info)
        {
            NUMBER number;
            int digits;
            double dTest;

            string retVal;

            char fmt = ParseFormatSpecifier(format, out digits);
            int precision = DOUBLE_PRECISION;

            number = new NUMBER();
            switch (fmt & 0xFFD)
            {
                case 'R':
                    //In order to give numbers that are both friendly to display and round-trippable,
                    //we parse the number using 7 digits and then determine if it round trips to the same
                    //value.  If it does, we convert that NUMBER to a string, otherwise we reparse using 9 digits
                    //and display that.
                    DoubleToNumber(value, DOUBLE_PRECISION, ref number);

                    if (number.scale == SCALE_NAN)
                    {
                        retVal = info.NaNSymbol;
                        goto lExit;
                    }

                    if (number.scale == SCALE_INF)
                    {
                        retVal = (number.sign > 0 ? info.NegativeInfinitySymbol : info.PositiveInfinitySymbol);
                        goto lExit;
                    }

                    unsafe
                    {
                        NumberToDouble(ref number, &dTest);
                    }

                    var fTest = (float)dTest;

                    if (fTest == value)
                    {
                        retVal = NumberToString(ref number, 'G', FLOAT_PRECISION, info);
                        goto lExit;
                    }

                    DoubleToNumber(value, 9, ref number);
                    retVal = NumberToString(ref number, 'G', 17, info);
                    goto lExit;

                case 'E':
                    // Here we round values less than E14 to 15 digits
                    if (digits > 14)
                    {
                        precision = 17;
                    }
                    break;

                case 'G':
                    // Here we round values less than G15 to 15 digits, G16 and G17 will not be touched
                    if (digits > 15)
                    {
                        precision = 17;
                    }
                    break;

            }

            DoubleToNumber(value, precision, ref number);

            if (number.scale == SCALE_NAN)
            {
                retVal = info.NaNSymbol;
                goto lExit;
            }

            if (number.scale == SCALE_INF)
            {
                retVal = (number.sign > 0 ? info.NegativeInfinitySymbol : info.PositiveInfinitySymbol);
                goto lExit;
            }

            if (fmt != 0)
            {
                retVal = NumberToString(ref number, fmt, digits, info);
            }
            else
            {
                retVal = NumberToStringFormat(ref number, format, info);
            }

        lExit:
            return retVal;

        }

        public static String FormatSingle(float value, String format, NumberFormatInfo info)
        {
            NUMBER number;
            int digits;
            double dTest;
            double argsValue = value;

            string retVal;

            char fmt = ParseFormatSpecifier(format, out digits);
            int precision = FLOAT_PRECISION;

            number = new NUMBER();
            switch (fmt & 0xFFD)
            {
                case 'R':
                    //In order to give numbers that are both friendly to display and round-trippable,
                    //we parse the number using 7 digits and then determine if it round trips to the same
                    //value.  If it does, we convert that NUMBER to a string, otherwise we reparse using 9 digits
                    //and display that.
                    DoubleToNumber(argsValue, FLOAT_PRECISION, ref number);

                    if (number.scale == SCALE_NAN)
                    {
                        retVal = info.NaNSymbol;
                        goto lExit;
                    }

                    if (number.scale == SCALE_INF)
                    {
                        retVal = (number.sign > 0 ? info.NegativeInfinitySymbol : info.PositiveInfinitySymbol);
                        goto lExit;
                    }

                    unsafe
                    {
                        NumberToDouble(ref number, &dTest);
                    }

                    var fTest = (float)dTest;

                    if (fTest == value)
                    {
                        retVal = NumberToString(ref number, 'G', FLOAT_PRECISION, info);
                        goto lExit;
                    }

                    DoubleToNumber(argsValue, 9, ref number);
                    retVal = NumberToString(ref number, 'G', 9, info);
                    goto lExit;

                case 'E':
                    // Here we round values less than E14 to 15 digits
                    if (digits > 6)
                    {
                        precision = 9;
                    }
                    break;

                case 'G':
                    // Here we round values less than G15 to 15 digits, G16 and G17 will not be touched
                    if (digits > 7)
                    {
                        precision = 9;
                    }
                    break;

            }

            DoubleToNumber(value, precision, ref number);

            if (number.scale == SCALE_NAN)
            {
                retVal = info.NaNSymbol;
                goto lExit;
            }

            if (number.scale == SCALE_INF)
            {
                retVal = (number.sign > 0 ? info.NegativeInfinitySymbol : info.PositiveInfinitySymbol);
                goto lExit;
            }

            if (fmt != 0)
            {
                retVal = NumberToString(ref number, fmt, digits, info);
            }
            else
            {
                retVal = NumberToStringFormat(ref number, format, info);
            }

        lExit:
            return retVal;

        }

        public unsafe static Boolean NumberBufferToDecimal(byte* number, ref Decimal value)
        {
            throw new NotImplementedException();
        }

        internal unsafe static Boolean NumberBufferToDouble(byte* number, ref Double value)
        {
            throw new NotImplementedException();
        }

        internal static unsafe string FormatNumberBuffer(byte* number, string format, NumberFormatInfo info, char* allDigits)
        {
            throw new NotImplementedException();
        }

        // Constants used by number parsing
        private const Int32 NumberMaxDigits = 50;

        private const Int32 Int32Precision = 10;
        private const Int32 UInt32Precision = Int32Precision;
        private const Int32 Int64Precision = 19;
        private const Int32 UInt64Precision = 20;

        // NumberBuffer is a partial wrapper around a stack pointer that maps on to
        // the native NUMBER struct so that it can be passed to native directly. It 
        // must be initialized with a stack Byte * of size NumberBufferBytes.
        // For performance, this structure should attempt to be completely inlined.
        // 
        // It should always be initialized like so:
        //
        // Byte * numberBufferBytes = stackalloc Byte[NumberBuffer.NumberBufferBytes];
        // NumberBuffer number = new NumberBuffer(numberBufferBytes);
        //
        // For performance, when working on the buffer in managed we use the values in this
        // structure, except for the digits, and pack those values into the byte buffer
        // if called out to managed.

        internal unsafe struct NumberBuffer
        {

            // Enough space for NumberMaxDigit characters plus null and 3 32 bit integers and a pointer
            public static readonly Int32 NumberBufferBytes = 12 + ((NumberMaxDigits + 1) * 2) + IntPtr.Size;


            private Byte* baseAddress;

            public Char* digits;
            public Int32 precision;
            public Int32 scale;
            public Boolean sign;


            public NumberBuffer(Byte* stackBuffer)
            {
                this.baseAddress = stackBuffer;
                this.digits = (((Char*)stackBuffer) + 6);
                this.precision = 0;
                this.scale = 0;
                this.sign = false;
            }


            public Byte* PackForNative()
            {
                Int32* baseInteger = (Int32*)baseAddress;
                baseInteger[0] = precision;
                baseInteger[1] = scale;
                baseInteger[2] = sign ? 1 : 0;
                return baseAddress;
            }
        }

        private static Boolean HexNumberToInt32(ref NumberBuffer number, ref Int32 value)
        {
            UInt32 passedValue = 0;
            Boolean returnValue = HexNumberToUInt32(ref number, ref passedValue);
            value = (Int32)passedValue;
            return returnValue;
        }

        private static Boolean HexNumberToInt64(ref NumberBuffer number, ref Int64 value)
        {
            UInt64 passedValue = 0;
            Boolean returnValue = HexNumberToUInt64(ref number, ref passedValue);
            value = (Int64)passedValue;
            return returnValue;
        }


        private unsafe static Boolean HexNumberToUInt32(ref NumberBuffer number, ref UInt32 value)
        {

            Int32 i = number.scale;
            if (i > UInt32Precision || i < number.precision)
            {
                return false;
            }
            Char* p = number.digits;


            UInt32 n = 0;
            while (--i >= 0)
            {
                if (n > ((UInt32)0xFFFFFFFF / 16))
                {
                    return false;
                }
                n *= 16;
                if (*p != '\0')
                {
                    UInt32 newN = n;
                    if (*p != '\0')
                    {
                        if (*p >= '0' && *p <= '9')
                        {
                            newN += (UInt32)(*p - '0');
                        }
                        else
                        {
                            if (*p >= 'A' && *p <= 'F')
                            {
                                newN += (UInt32)((*p - 'A') + 10);
                            }
                            else
                            {

                                newN += (UInt32)((*p - 'a') + 10);
                            }
                        }
                        p++;
                    }

                    // Detect an overflow here...
                    if (newN < n)
                    {
                        return false;
                    }
                    n = newN;
                }
            }
            value = n;
            return true;
        }


        private unsafe static Boolean HexNumberToUInt64(ref NumberBuffer number, ref UInt64 value)
        {

            Int32 i = number.scale;
            if (i > UInt64Precision || i < number.precision)
            {
                return false;
            }
            Char* p = number.digits;


            UInt64 n = 0;
            while (--i >= 0)
            {
                if (n > (0xFFFFFFFFFFFFFFFF / 16))
                {
                    return false;
                }
                n *= 16;
                if (*p != '\0')
                {
                    UInt64 newN = n;
                    if (*p != '\0')
                    {
                        if (*p >= '0' && *p <= '9')
                        {
                            newN += (UInt64)(*p - '0');
                        }
                        else
                        {
                            if (*p >= 'A' && *p <= 'F')
                            {
                                newN += (UInt64)((*p - 'A') + 10);
                            }
                            else
                            {
                                newN += (UInt64)((*p - 'a') + 10);
                            }
                        }
                        p++;
                    }

                    // Detect an overflow here...
                    if (newN < n)
                    {
                        return false;
                    }
                    n = newN;
                }
            }
            value = n;
            return true;
        }

        private static Boolean IsWhite(char ch)
        {
            return (((ch) == 0x20) || ((ch) >= 0x09 && (ch) <= 0x0D));
        }


        private unsafe static Boolean NumberToInt32(ref NumberBuffer number, ref Int32 value)
        {

            Int32 i = number.scale;
            if (i > Int32Precision || i < number.precision)
            {
                return false;
            }
            char* p = number.digits;

            Int32 n = 0;
            while (--i >= 0)
            {
                if ((UInt32)n > (0x7FFFFFFF / 10))
                {
                    return false;
                }
                n *= 10;
                if (*p != '\0')
                {
                    n += (Int32)(*p++ - '0');
                }
            }
            if (number.sign)
            {
                n = -n;
                if (n > 0)
                {
                    return false;
                }
            }
            else
            {
                if (n < 0)
                {
                    return false;
                }
            }
            value = n;
            return true;
        }


        private unsafe static Boolean NumberToInt64(ref NumberBuffer number, ref Int64 value)
        {

            Int32 i = number.scale;
            if (i > Int64Precision || i < number.precision)
            {
                return false;
            }
            char* p = number.digits;

            Int64 n = 0;
            while (--i >= 0)
            {
                if ((UInt64)n > (0x7FFFFFFFFFFFFFFF / 10))
                {
                    return false;
                }
                n *= 10;
                if (*p != '\0')
                {
                    n += (Int32)(*p++ - '0');
                }
            }
            if (number.sign)
            {
                n = -n;
                if (n > 0)
                {
                    return false;
                }
            }
            else
            {
                if (n < 0)
                {
                    return false;
                }
            }
            value = n;
            return true;
        }


        private unsafe static Boolean NumberToUInt32(ref NumberBuffer number, ref UInt32 value)
        {

            Int32 i = number.scale;
            if (i > UInt32Precision || i < number.precision || number.sign)
            {
                return false;
            }
            char* p = number.digits;

            UInt32 n = 0;
            while (--i >= 0)
            {
                if (n > (0xFFFFFFFF / 10))
                {
                    return false;
                }
                n *= 10;
                if (*p != '\0')
                {
                    UInt32 newN = n + (UInt32)(*p++ - '0');
                    // Detect an overflow here...
                    if (newN < n)
                    {
                        return false;
                    }
                    n = newN;
                }
            }
            value = n;
            return true;
        }


        private unsafe static bool NumberToUInt64(ref NumberBuffer number, ref UInt64 value)
        {
            Int32 i = number.scale;
            if (i > UInt64Precision || i < number.precision || number.sign)
            {
                return false;
            }
            char* p = number.digits;

            UInt64 n = 0;
            while (--i >= 0)
            {
                if (n > (0xFFFFFFFFFFFFFFFF / 10))
                {
                    return false;
                }
                n *= 10;
                if (*p != '\0')
                {
                    UInt64 newN = n + (UInt64)(*p++ - '0');
                    // Detect an overflow here...
                    if (newN < n)
                    {
                        return false;
                    }
                    n = newN;
                }
            }
            value = n;
            return true;
        }

        private unsafe static string NumberToString(ref NUMBER number, char format, int digits, NumberFormatInfo numfmt, bool bDecimal = false)
        {
            long newBufferLen = MIN_BUFFER_SIZE;

            //CQuickBytesSpecifySize<LARGE_BUFFER_SIZE * sizeof(WCHAR)> buf;

            char* buffer = stackalloc char[LARGE_BUFFER_SIZE];
            char* dst = null;
            int digCount = 0;

            Console.WriteLine(format);
            Console.WriteLine((char) format & 0xFFDF);

            switch (format & 0xFFDF)
            {
                case 'C':
                    if (digits < 0) digits = numfmt.CurrencyDecimalDigits;
                    if (number.scale < 0)
                        digCount = 0;
                    else
                        digCount = number.scale + digits;

                    newBufferLen += digCount;
                    newBufferLen += numfmt.NegativeSign.Length; // For number and exponent
                    newBufferLen += ((long)numfmt.CurrencyGroupSizes.Length * digCount); // For all the grouping sizes
                    newBufferLen += numfmt.CurrencyDecimalSeparator.Length;
                    newBufferLen += numfmt.CurrencySymbol.Length;

                    if (newBufferLen > Int32.MaxValue)
                    {
                        throw new IndexOutOfRangeException();
                    }
                    dst = buffer;

                    RoundNumber(ref number, number.scale + digits); // Don't change this line to use digPos since digCount could have its sign changed.
                    dst = FormatCurrency(dst, ref number, digits, numfmt);
                    break;
                case 'F':
                    if (digits < 0) digits = numfmt.NumberDecimalDigits;

                    if (number.scale < 0)
                        digCount = 0;
                    else
                        digCount = number.scale + digits;


                    newBufferLen += digCount;
                    newBufferLen += numfmt.NegativeSign.Length; // For number and exponent
                    newBufferLen += numfmt.NumberDecimalSeparator.Length;

                    if (newBufferLen > Int32.MaxValue)
                    {
                        throw new IndexOutOfRangeException();
                    }
                    dst = buffer;

                    RoundNumber(ref number, number.scale + digits);
                    if (number.sign > 0)
                    {
                        AddStringRef(&dst, numfmt.NegativeSign);
                    }

                    dst = FormatFixed(dst, ref number, digits, null, numfmt.NumberDecimalSeparator, null);
                    break;
                case 'N':
                    if (digits < 0) digits = numfmt.NumberDecimalDigits; // Since we are using digits in our calculation

                    if (number.scale < 0)
                        digCount = 0;
                    else
                        digCount = number.scale + digits;


                    newBufferLen += digCount;
                    newBufferLen += numfmt.NegativeSign.Length; // For number and exponent
                    newBufferLen += ((long)numfmt.NumberGroupSizes.Length) * digCount; // For all the grouping sizes
                    newBufferLen += numfmt.NumberDecimalSeparator.Length;

                    if (newBufferLen > Int32.MaxValue)
                    {
                        throw new IndexOutOfRangeException();
                    }
                    dst = buffer;

                    RoundNumber(ref number, number.scale + digits);
                    dst = FormatNumber(dst, ref number, digits, numfmt);
                    break;
                case 'E':
                    if (digits < 0) digits = 6;
                    digits++;

                    newBufferLen += digits;
                    newBufferLen += (((long)numfmt.NegativeSign.Length + numfmt.PositiveSign.Length) * 2); // For number and exponent
                    newBufferLen += numfmt.NumberDecimalSeparator.Length;

                    if (newBufferLen > Int32.MaxValue)
                    {
                        throw new IndexOutOfRangeException();
                    }
                    dst = buffer;

                    RoundNumber(ref number, digits);
                    if (number.sign > 0)
                    {
                        AddStringRef(&dst, numfmt.NegativeSign);
                    }
                    dst = FormatScientific(dst, ref number, digits, format, numfmt);
                    break;
                case 'G':
                    {
                        bool enableRounding = true;
                        if (digits < 1)
                        {
                            if (bDecimal && (digits == -1))
                            { // Default to 29 digits precision only for G formatting without a precision specifier
                                digits = DECIMAL_PRECISION;
                                enableRounding = false;  // Turn off rounding for ECMA compliance to output trailing 0's after decimal as significant
                            }
                            else
                            {
                                digits = number.precision;
                            }
                        }

                        newBufferLen += digits;
                        newBufferLen += ((numfmt.NegativeSign.Length + numfmt.PositiveSign.Length) * 2); // For number and exponent
                        newBufferLen += numfmt.NumberDecimalSeparator.Length;

                        if (newBufferLen > Int32.MaxValue)
                        {
                            throw new IndexOutOfRangeException();
                        }
                        dst = buffer;

                        if (enableRounding) // Don't round for G formatting without precision
                            RoundNumber(ref number, digits); // This also fixes up the minus zero case
                        else
                        {
                            fixed (char* digitsPtr = number.digits)
                            {
                                if (bDecimal && (digitsPtr[0] == 0))
                                {
                                    // Minus zero should be formatted as 0
                                    number.sign = 0;
                                }
                            }
                        }
                        if (number.sign > 0)
                        {
                            AddStringRef(&dst, numfmt.NegativeSign);
                        }
                        dst = FormatGeneral(dst, ref number, digits, (char)(format - ('G' - 'E')), numfmt, !enableRounding);
                    }
                    break;
                case 'P':
                    if (digits < 0) digits = numfmt.PercentDecimalDigits;
                    number.scale += 2;

                    if (number.scale < 0)
                        digCount = 0;
                    else
                        digCount = number.scale + digits;


                    newBufferLen += digCount;
                    newBufferLen += numfmt.NegativeSign.Length; // For number and exponent
                    newBufferLen += ((long)numfmt.PercentGroupSeparator.Length) * digCount; // For all the grouping sizes
                    newBufferLen += numfmt.PercentDecimalSeparator.Length;
                    newBufferLen += numfmt.PercentSymbol.Length;

                    if (newBufferLen > Int32.MaxValue)
                    {
                        throw new IndexOutOfRangeException();
                    }
                    dst = buffer;

                    RoundNumber(ref number, number.scale + digits);
                    dst = FormatPercent(dst, ref number, digits, numfmt);
                    break;
                default:
                    throw new FormatException("Format_BadFormatSpecifier");
            }

            if (!((dst - buffer >= 0) && (dst - buffer) <= newBufferLen))
            {
                throw new OutOfMemoryException();
            }

            return new String(buffer, 0, (int)(dst - buffer));
        }

        private unsafe static string NumberToStringFormat(ref NUMBER number, string str, NumberFormatInfo numfmt)
        {
            int digitCount;
            int decimalPos;
            int firstDigit;
            int lastDigit;
            int digPos;
            int scientific;
            int percent;
            int permille;
            int thousandPos;
            int thousandCount = 0;
            int thousandSeps;
            int scaleAdjust;
            int adjust;
            char* section = null;
            char* src = null;
            char* dst = null;
            char* dig = null;
            char ch;

            fixed (char* format = str)
            fixed (char* digits = number.digits)
            {
                section = FindSection(format, digits[0] == 0 ? 2 : number.sign > 0 ? 1 : 0);

            ParseSection:
                digitCount = 0;
                decimalPos = -1;
                firstDigit = 0x7FFFFFFF;
                lastDigit = 0;
                scientific = 0;
                percent = 0;
                permille = 0;
                thousandPos = -1;
                thousandSeps = 0;
                scaleAdjust = 0;
                src = section;

                while ((ch = *src++) != 0 && ch != ';')
                {
                    switch (ch)
                    {
                        case '#':
                            digitCount++;
                            break;
                        case '0':
                            if (firstDigit == 0x7FFFFFFF)
                                firstDigit = digitCount;
                            digitCount++;
                            lastDigit = digitCount;
                            break;
                        case '.':
                            if (decimalPos < 0)
                            {
                                decimalPos = digitCount;
                            }
                            break;
                        case ',':
                            if (digitCount > 0 && decimalPos < 0)
                            {
                                if (thousandPos >= 0)
                                {
                                    if (thousandPos == digitCount)
                                    {
                                        thousandCount++;
                                        break;
                                    }
                                    thousandSeps = 1;
                                }
                                thousandPos = digitCount;
                                thousandCount = 1;
                            }
                            break;
                        case '%':
                            percent++;
                            scaleAdjust += 2;
                            break;
                        case (char)0x2030:
                            permille++;
                            scaleAdjust += 3;
                            break;
                        case '\'':
                        case '"':
                            while (*src != 0 && *src++ != ch)
                                ;
                            break;
                        case '\\':
                            if (*src != 0)
                                src++;
                            break;
                        case 'E':
                        case 'e':
                            if (*src == '0' || ((*src == '+' || *src == '-') && src[1] == '0'))
                            {
                                while (*++src == '0')
                                    ;
                                scientific = 1;
                            }
                            break;
                    }
                }

                if (decimalPos < 0)
                    decimalPos = digitCount;
                if (thousandPos >= 0)
                {
                    if (thousandPos == decimalPos)
                    {
                        scaleAdjust -= thousandCount * 3;
                    }
                    else
                    {
                        thousandSeps = 1;
                    }
                }
                if (digits[0] != 0)
                {
                    number.scale += scaleAdjust;
                    int pos = scientific > 0 ? digitCount : number.scale + digitCount - decimalPos;
                    RoundNumber(ref number, pos);
                    if (digits[0] == 0)
                    {
                        src = FindSection(format, 2);
                        if (src != section)
                        {
                            section = src;
                            goto ParseSection;
                        }
                    }
                }
                else
                {
                    number.sign = 0; // We need to format -0 without the sign set.
                    number.scale = 0; // Decimals with scale ('0.00') should be rounded.
                }

                firstDigit = firstDigit < decimalPos ? decimalPos - firstDigit : 0;
                lastDigit = lastDigit > decimalPos ? decimalPos - lastDigit : 0;
                if (scientific > 0)
                {
                    digPos = decimalPos;
                    adjust = 0;
                }
                else
                {
                    digPos = number.scale > decimalPos ? number.scale : decimalPos;
                    adjust = number.scale - decimalPos;
                }
                src = section;
                dig = digits;

                // Find maximum number of characters that the destination string can grow by
                // in the following while loop.  Use this to avoid buffer overflows.
                // Longest strings are potentially +/- signs with 10 digit exponents,
                // or decimal numbers, or the while loops copying from a quote or a \ onwards.
                // Check for positive and negative
                int maxStrIncLen = 0;
                // We need this to be UINT64 since the percent computation could go beyond a UINT.
                if (number.sign > 0)
                {
                    maxStrIncLen = numfmt.NegativeSign.Length;
                }
                else
                {
                    maxStrIncLen = numfmt.PositiveSign.Length;
                }

                // Add for any big decimal seperator
                maxStrIncLen += numfmt.NumberDecimalSeparator.Length;

                // Add for scientific
                if (scientific > 0)
                {
                    int inc1 = numfmt.PositiveSign.Length;
                    int inc2 = numfmt.NegativeSign.Length;
                    maxStrIncLen += (inc1 > inc2) ? inc1 : inc2;
                }

                // Add for percent separator
                if (percent > 0)
                {
                    maxStrIncLen += numfmt.PercentSymbol.Length * percent;
                }

                // Add for permilli separator
                if (permille > 0)
                {
                    maxStrIncLen += numfmt.PerMilleSymbol.Length * permille;
                }

                //adjust can be negative, so we make this an int instead of an unsigned int.
                // adjust represents the number of characters over the formatting eg. format string is "0000" and you are trying to
                // format 100000 (6 digits). Means adjust will be 2. On the other hand if you are trying to format 10 adjust will be
                // -2 and we'll need to fixup these digits with 0 padding if we have 0 formatting as in this example.
                int adjustLen = (adjust > 0) ? adjust : 0; // We need to add space for these extra characters anyway.

                int bufferLen2 = 125;
                int* thousandsSepPos = stackalloc int[bufferLen2];
                int thousandsSepCtr = -1;

                if (thousandSeps > 0)
                {
                    // Fixup possible buffer overrun problems
                    // We need to precompute this outside the number formatting loop
                    if (numfmt.NumberGroupSizes.Length == 0)
                    {
                        thousandSeps = 0; // Nothing to add
                    }
                    else
                    {
                        ////thousandsSepPos = (int*)thousands.AllocThrows(bufferLen2 * sizeof (INT32));
                        //                        - We need this array to figure out where to insert the thousands seperator. We would have to traverse the string
                        // backwords. PIC formatting always traverses forwards. These indices are precomputed to tell us where to insert
                        // the thousands seperator so we can get away with traversing forwards. Note we only have to compute upto digPos.
                        // The max is not bound since you can have formatting strings of the form "000,000..", and this
                        // should handle that case too.

                        int[] groupDigits = numfmt.NumberGroupSizes;

                        int groupSizeIndex = 0; // index into the groupDigits array.
                        int groupTotalSizeCount = 0;
                        int groupSizeLen = numfmt.NumberGroupSizes.Length; // the length of groupDigits array.
                        if (groupSizeLen != 0)
                            groupTotalSizeCount = groupDigits[groupSizeIndex];
                        // the current running total of group size.
                        int groupSize = groupTotalSizeCount;

                        int totalDigits = digPos + ((adjust < 0) ? adjust : 0); // actual number of digits in o/p
                        int numDigits = (firstDigit > totalDigits) ? firstDigit : totalDigits;
                        while (numDigits > groupTotalSizeCount)
                        {
                            if (groupSize == 0)
                                break;
                            thousandsSepPos[++thousandsSepCtr] = groupTotalSizeCount;
                            if (groupSizeIndex < groupSizeLen - 1)
                            {
                                groupSizeIndex++;
                                groupSize = groupDigits[groupSizeIndex];
                            }
                            groupTotalSizeCount += groupSize;
                            if (bufferLen2 - thousandsSepCtr < 10)
                            {
                                // Slack of 10
                                throw new OutOfMemoryException();
                            }
                        }

                        // We already have computed the number of separators above. Simply add space for them.
                        adjustLen += ((thousandsSepCtr + 1) * numfmt.NumberGroupSeparator.Length);
                    }
                }

                maxStrIncLen += adjustLen;

                // Allocate temp buffer - gotta deal with Schertz' 500 MB strings.
                // Some computations like when you specify Int32.MaxValue-2 %'s and each percent is setup to be Int32.MaxValue in length
                // will generate a result that will be larget than an unsigned int can hold. This is to protect against overflow.
                int tempLen = str.Length + maxStrIncLen + 10; // Include a healthy amount of temp space.
                if ((uint)tempLen > 0x7FFFFFFF)
                {
                    throw new OverflowException();
                }

                int bufferLen = tempLen;
                if (bufferLen < 250) // Stay under 512 bytes
                    bufferLen = 250; // This is to prevent unneccessary calls to resize
                char* buffer = stackalloc char[bufferLen];
                dst = buffer;

                if (number.sign > 0 && section == format)
                {
                    AddStringRef(&dst, numfmt.NegativeSign);
                }

                bool decimalWritten = false;

                while ((ch = *src++) != 0 && ch != ';')
                {
                    // Make sure temp buffer is big enough, else resize it.
                    if (bufferLen - (uint)(dst - buffer) < 10)
                    {
                        throw new OutOfMemoryException();
                    }

                    if (adjust > 0)
                    {
                        switch (ch)
                        {
                            case '#':
                            case '0':
                            case '.':
                                while (adjust > 0)
                                {
                                    // digPos will be one greater than thousandsSepPos[thousandsSepCtr] since we are at
                                    // the character after which the groupSeparator needs to be appended.
                                    *dst++ = *dig != 0 ? *dig++ : '0';
                                    if (thousandSeps > 0 && digPos > 1 && thousandsSepCtr >= 0)
                                    {
                                        if (digPos == thousandsSepPos[thousandsSepCtr] + 1)
                                        {
                                            AddStringRef(&dst, numfmt.NumberGroupSeparator);
                                            thousandsSepCtr--;
                                        }
                                    }
                                    digPos--;
                                    adjust--;
                                }
                                break;
                        }
                    }

                    switch (ch)
                    {
                        case '#':
                        case '0':
                            {
                                if (adjust < 0)
                                {
                                    adjust++;
                                    ch = (char)(digPos <= firstDigit ? '0' : 0);
                                }
                                else
                                {
                                    ch = (char)(*dig != 0 ? *dig++ : digPos > lastDigit ? '0' : 0);
                                }
                                if (ch != 0)
                                {
                                    *dst++ = ch;
                                    if (thousandSeps > 0 && digPos > 1 && thousandsSepCtr >= 0)
                                    {
                                        if (digPos == thousandsSepPos[thousandsSepCtr] + 1)
                                        {
                                            AddStringRef(&dst, numfmt.NumberGroupSeparator);
                                            thousandsSepCtr--;
                                        }
                                    }
                                }

                                digPos--;
                                break;
                            }
                        case '.':
                            {
                                if (digPos != 0 || decimalWritten)
                                {
                                    // For compatability, don't echo repeated decimals
                                    break;
                                }
                                // If the format has trailing zeros or the format has a decimal and digits remain
                                if (lastDigit < 0
                                    || (decimalPos < digitCount && *dig != 0))
                                {
                                    AddStringRef(&dst, numfmt.NumberDecimalSeparator);
                                    decimalWritten = true;
                                }
                                break;
                            }
                        case (char)0x2030:
                            AddStringRef(&dst, numfmt.PerMilleSymbol);
                            break;
                        case '%':
                            AddStringRef(&dst, numfmt.PercentSymbol);
                            break;
                        case ',':
                            break;
                        case '\'':
                        case '"':
                            // Buffer overflow possibility
                            while (*src != 0 && *src != ch)
                            {
                                *dst++ = *src++;
                                if ((uint)(dst - buffer) == bufferLen - 1)
                                {
                                    if (bufferLen - (uint)(dst - buffer) < maxStrIncLen)
                                    {
                                        throw new OutOfMemoryException();
                                    }
                                }
                            }
                            if (*src != 0)
                                src++;
                            break;
                        case '\\':
                            if (*src != 0)
                                *dst++ = *src++;
                            break;
                        case 'E':
                        case 'e':
                            {
                                string sign = null;
                                int i = 0;
                                if (scientific > 0)
                                {
                                    if (*src == '0')
                                    {
                                        //Handles E0, which should format the same as E-0
                                        i++;
                                    }
                                    else if (*src == '+' && src[1] == '0')
                                    {
                                        //Handles E+0
                                        sign = numfmt.PositiveSign;
                                    }
                                    else if (*src == '-' && src[1] == '0')
                                    {
                                        //Handles E-0
                                        //Do nothing, this is just a place holder s.t. we don't break out of the loop.
                                    }
                                    else
                                    {
                                        *dst++ = ch;
                                        break;
                                    }
                                    while (*++src == '0')
                                        i++;
                                    if (i > 10)
                                        i = 10;
                                    int exp = digits[0] == 0 ? 0 : number.scale - decimalPos;
                                    dst = FormatExponent(dst, exp, ch, sign, numfmt.NegativeSign, i);
                                    scientific = 0;
                                }
                                else
                                {
                                    *dst++ = ch; // Copy E or e to output
                                    if (*src == '+' || *src == '-')
                                    {
                                        *dst++ = *src++;
                                    }
                                    while (*src == '0')
                                    {
                                        *dst++ = *src++;
                                    }
                                }
                                break;
                            }
                        default:
                            *dst++ = ch;
                            break;
                    }
                }
                if (!((dst - buffer >= 0) && (dst - buffer <= (int)bufferLen)))
                {
                    throw new OutOfMemoryException();
                }

                return new String(buffer, 0, (int)(dst - buffer));
            }
        }

        private static unsafe char* FindSection(char* format, int section)
        {
            char* src;
            char ch;
            if (section == 0) return format;
            src = format;
            for (; ; )
            {
                switch (ch = *src++)
                {
                    case '\'':
                    case '"':
                        while (*src != 0 && *src++ != ch) ;
                        break;
                    case '\\':
                        if (*src != 0) src++;
                        break;
                    case ';':
                        if (--section != 0) break;
                        if (*src != 0 && *src != ';') return src;

                        goto case '\0';

                    case '\0':
                        return format;
                }
            }
        }

        private static string[] posCurrencyFormats = new[]
        {
            "$#",
            "#$",
            "$ #",
            "# $"
        };

        private static string[] negCurrencyFormats = new[]
        {
            "($#)",
            "-$#",
            "$-#",
            "$#-",
            "(#$)",
            "-#$",
            "#-$",
            "#$-",
            "-# $",
            "-$ #",
            "# $-",
            "$ #-",
            "$ -#",
            "#- $",
            "($ #)",
            "(# $)"
        };

        private static string[] posPercentFormats = new[]
        {
            "# %",
            "#%",
            "%#",
            "% #"
        };

        private static string[] negPercentFormats = new[]
        {
            "-# %",
            "-#%",
            "-%#",
            "%-#",
            "%#-",
            "#-%",
            "#%-",
            "-% #",
            "# %-",
            "% #-",
            "% -#",
            "#- %"
        };

        private static string[] negNumberFormats = new[]
        {
            "(#)",
            "-#",
            "- #",
            "#-",
            "# -",
        };

        private static string posNumberFormat = "#";

        private unsafe static char* FormatNumber(char* buffer, ref NUMBER number, int digits, NumberFormatInfo numfmt)
        {
            char ch;
            var fmtStr = number.sign > 0
                ? negNumberFormats[numfmt.NumberNegativePattern]
                : posNumberFormat;

            fixed (char* fmtPtr = fmtStr)
            {
                char* fmt = fmtPtr;
                while ((ch = *fmt++) != 0)
                {
                    switch (ch)
                    {
                        case '#':
                            buffer = FormatFixed(
                                buffer,
                                ref number,
                                digits,
                                numfmt.NumberGroupSizes,
                                numfmt.NumberDecimalSeparator,
                                numfmt.NumberGroupSeparator);
                            break;
                        case '-':
                            AddStringRef(&buffer, numfmt.NegativeSign);
                            break;
                        default:
                            *buffer++ = ch;
                            break;
                    }
                }
            }

            return buffer;
        }

        private unsafe static char* FormatFixed(char* buffer, ref NUMBER number, int digits,
            int[] groupDigits, string sDecimal, string sGroup)
        {
            int digPos = number.scale;
            fixed (char* digPtr = number.digits)
            {
                char* dig = digPtr;
                if (digPos > 0)
                {
                    if (groupDigits != null)
                    {

                        int groupSizeIndex = 0; // index into the groupDigits array.
                        int groupSizeCount = groupDigits[groupSizeIndex]; // the current total of group size.
                        int groupSizeLen = groupDigits.Length; // the length of groupDigits array.
                        int bufferSize = digPos; // the length of the result buffer string.
                        int groupSeparatorLen = sGroup.Length; // the length of the group separator string.
                        int groupSize = 0; // the current group size.

                        //
                        // Find out the size of the string buffer for the result.
                        //
                        if (groupSizeLen != 0) // You can pass in 0 length arrays
                        {
                            while (digPos > groupSizeCount)
                            {
                                groupSize = groupDigits[groupSizeIndex];
                                if (groupSize == 0)
                                {
                                    break;
                                }

                                bufferSize += groupSeparatorLen;
                                if (groupSizeIndex < groupSizeLen - 1)
                                {
                                    groupSizeIndex++;
                                }
                                groupSizeCount += groupDigits[groupSizeIndex];
                                if (groupSizeCount < 0 || bufferSize < 0)
                                {
                                    throw new ArgumentOutOfRangeException(); // if we overflow
                                }
                            }
                            if (groupSizeCount == 0)
                                // If you passed in an array with one entry as 0, groupSizeCount == 0
                                groupSize = 0;
                            else
                                groupSize = groupDigits[0];
                        }

                        groupSizeIndex = 0;
                        int digitCount = 0;
                        int digStart;
                        int digLength = (int)wcslen(dig);
                        digStart = (digPos < digLength) ? digPos : digLength;
                        char* p = buffer + bufferSize - 1;
                        for (int i = digPos - 1; i >= 0; i--)
                        {
                            *(p--) = (i < digStart) ? dig[i] : '0';

                            if (groupSize > 0)
                            {
                                digitCount++;
                                if (digitCount == groupSize && i != 0)
                                {
                                    for (int j = groupSeparatorLen - 1; j >= 0; j--)
                                    {
                                        *(p--) = sGroup[j];
                                    }

                                    if (groupSizeIndex < groupSizeLen - 1)
                                    {
                                        groupSizeIndex++;
                                        groupSize = groupDigits[groupSizeIndex];
                                    }
                                    digitCount = 0;
                                }
                            }
                        }
                        if (p < buffer - 1)
                        {
                            // This indicates a buffer underflow since we write in backwards. 
                            throw new OutOfMemoryException();
                        }
                        buffer += bufferSize;
                        dig += digStart;
                    }
                    else
                    {
                        do
                        {
                            *buffer++ = *dig != 0 ? *dig++ : '0';
                        } while (--digPos > 0);
                    }
                }
                else
                {
                    *buffer++ = '0';
                }
                if (digits > 0)
                {
                    AddStringRef(&buffer, sDecimal);
                    while (digPos < 0 && digits > 0)
                    {
                        *buffer++ = '0';
                        digPos++;
                        digits--;
                    }
                    while (digits > 0)
                    {
                        *buffer++ = *dig != 0 ? *dig++ : '0';
                        digits--;
                    }
                }
            }

            return buffer;
        }

        private unsafe static char* FormatGeneral(char* buffer, ref NUMBER number, int digits, char expChar,
            NumberFormatInfo numfmt, bool bSuppressScientific = false)
        {
            int digPos = number.scale;
            int scientific = 0;
            if (!bSuppressScientific)
            { // Don't switch to scientific notation
                if (digPos > digits || digPos < -3)
                {
                    digPos = 1;
                    scientific = 1;
                }
            }
            fixed (char* digPtr = number.digits)
            {
                char* dig = digPtr;
                if (digPos > 0)
                {
                    do
                    {
                        *buffer++ = *dig != 0 ? *dig++ : '0';
                    } while (--digPos > 0);
                }
                else
                {
                    *buffer++ = '0';
                }
                if (*dig != 0 || digPos < 0)
                {
                    AddStringRef(&buffer, numfmt.NumberDecimalSeparator);
                    while (digPos < 0)
                    {
                        *buffer++ = '0';
                        digPos++;
                    }
                    while (*dig != 0)
                    {
                        *buffer++ = *dig++;
                    }
                }
                if (scientific > 0)
                    buffer = FormatExponent(
                        buffer,
                        number.scale - 1,
                        expChar,
                        numfmt.PositiveSign,
                        numfmt.NegativeSign,
                        2);
            }

            return buffer;
        }

        private unsafe static char* FormatExponent(char* buffer, int value, char expChar,
            string posSignStr, string negSignStr, int minDigits)
        {
            char* digits = stackalloc char[11];
            *buffer++ = expChar;
            if (value < 0)
            {
                AddStringRef(&buffer, negSignStr);
            }
            else if (posSignStr != null)
            {
                AddStringRef(&buffer, posSignStr);
            }

            char* p = Int32ToDecChars(digits + 10, value, minDigits);
            int i = (int)(digits + 10 - p);
            while (--i >= 0) *buffer++ = *p++;
            return buffer;
        }

        private unsafe static char* FormatScientific(char* buffer, ref NUMBER number, int digits, char expChar,
            NumberFormatInfo numfmt)
        {
            fixed (char* digPtr = number.digits)
            {
                char* dig = digPtr;
                *buffer++ = *dig != 0 ? *dig++ : '0';
                if (digits != 1) // For E0 we would like to suppress the decimal point
                    AddStringRef(&buffer, numfmt.NumberDecimalSeparator);
                while (--digits > 0)
                    *buffer++ = *dig != 0 ? *dig++ : '0';
                int e = digPtr[0] == 0 ? 0 : number.scale - 1;
                buffer = FormatExponent(buffer, e, expChar, numfmt.PositiveSign, numfmt.NegativeSign, 3);
                return buffer;
            }
        }

        private unsafe static char* FormatCurrency(char* buffer, ref NUMBER number, int digits, NumberFormatInfo numfmt)
        {
            char ch;
            var fmtStr = number.sign > 0
                ? negCurrencyFormats[numfmt.CurrencyNegativePattern]
                : posCurrencyFormats[numfmt.CurrencyPositivePattern];

            fixed (char* fmtPtr = fmtStr)
            {
                char* fmt = fmtPtr;
                while ((ch = *fmt++) != 0)
                {
                    switch (ch)
                    {
                        case '#':
                            buffer = FormatFixed(
                                buffer,
                                ref number,
                                digits,
                                numfmt.CurrencyGroupSizes,
                                numfmt.CurrencyDecimalSeparator,
                                numfmt.CurrencyGroupSeparator);
                            break;
                        case '-':
                            AddStringRef(&buffer, numfmt.NegativeSign);
                            break;
                        case '$':
                            AddStringRef(&buffer, numfmt.CurrencySymbol);
                            break;
                        default:
                            *buffer++ = ch;
                            break;
                    }
                }
            }
            return buffer;
        }

        private unsafe static char* FormatPercent(char* buffer, ref NUMBER number, int digits, NumberFormatInfo numfmt)
        {
            char ch;
            var fmtStr = number.sign > 0
                ? negPercentFormats[numfmt.PercentNegativePattern]
                : posPercentFormats[numfmt.PercentPositivePattern];

            fixed (char* fmtPtr = fmtStr)
            {
                char* fmt = fmtPtr;
                while ((ch = *fmt++) != 0)
                {
                    switch (ch)
                    {
                        case '#':
                            buffer = FormatFixed(
                                buffer,
                                ref number,
                                digits,
                                numfmt.PercentGroupSizes,
                                numfmt.PercentDecimalSeparator,
                                numfmt.PercentGroupSeparator);
                            break;
                        case '-':
                            AddStringRef(&buffer, numfmt.NegativeSign);
                            break;
                        case '%':
                            AddStringRef(&buffer, numfmt.PercentSymbol);
                            break;
                        default:
                            *buffer++ = ch;
                            break;
                    }
                }
            }

            return buffer;
        }

        private unsafe static void AddStringRef(char** ppBuffer, string strRef)
        {
            fixed (char* buffer = strRef)
            {
                int length = strRef.Length;
                for (char* str = buffer; str < buffer + length; (*ppBuffer)++, str++)
                {
                    **ppBuffer = *str;
                }
            }
        }

        private unsafe static void RoundNumber(ref NUMBER number, int pos)
        {
            fixed (char* digits = number.digits)
            {
                int i = 0;
                while (i < pos && digits[i] != 0)
                    i++;
                if (i == pos && digits[i] >= '5')
                {
                    while (i > 0 && digits[i - 1] == '9')
                        i--;
                    if (i > 0)
                    {
                        digits[i - 1]++;
                    }
                    else
                    {
                        number.scale++;
                        digits[0] = '1';
                        i = 1;
                    }
                }
                else
                {
                    while (i > 0 && digits[i - 1] == '0')
                        i--;
                }
                if (i == 0)
                {
                    number.scale = 0;
                    number.sign = 0;
                }
                digits[i] = '\0';
            }
        }


        private static byte[] rgexp64Power10 = new byte[]
        {
            // exponents for both powers of 10 and 0.1
            /*1*/ 4, /*2*/ 7, /*3*/ 10, /*4*/ 14, /*5*/ 17, /*6*/ 20, /*7*/ 24, /*8*/ 27, /*9*/ 30, /*10*/
                    34, /*11*/ 37, /*12*/ 40, /*13*/ 44, /*14*/ 47, /*15*/ 50,
        };

        private static ulong[] rgval64Power10 = new ulong[]
        {
            // powers of 10
            /*1*/ (0xa000000000000000), /*2*/ (0xc800000000000000), /*3*/ (0xfa00000000000000), /*4*/
                    (0x9c40000000000000), /*5*/ (0xc350000000000000), /*6*/
                    (0xf424000000000000), /*7*/ (0x9896800000000000), /*8*/ (0xbebc200000000000), /*9*/
                    (0xee6b280000000000), /*10*/ (0x9502f90000000000), /*11*/
                    (0xba43b74000000000), /*12*/ (0xe8d4a51000000000), /*13*/ (0x9184e72a00000000), /*14*/
                    (0xb5e620f480000000), /*15*/ (0xe35fa931a0000000),

                    // powers of 0.1
                    /*1*/ (0xcccccccccccccccd), /*2*/ (0xa3d70a3d70a3d70b), /*3*/
                    (0x83126e978d4fdf3c), /*4*/ (0xd1b71758e219652e), /*5*/ (0xa7c5ac471b478425), /*6*/
                    (0x8637bd05af6c69b7), /*7*/ (0xd6bf94d5e57a42be), /*8*/
                    (0xabcc77118461ceff), /*9*/ (0x89705f4136b4a599), /*10*/ (0xdbe6fecebdedd5c2), /*11*/
                    (0xafebff0bcb24ab02), /*12*/ (0x8cbccc096f5088cf), /*13*/
                    (0xe12e13424bb40e18), /*14*/ (0xb424dc35095cd813), /*15*/ (0x901d7cf73ab0acdc),
        };

        private static short[] rgexp64Power10By16 = new short[] 
        { 
            // exponents for both powers of 10^16 and 0.1^16
            /*1*/ 54, /*2*/ 107, /*3*/ 160, /*4*/ 213, /*5*/ 266, /*6*/ 319, /*7*/ 373, /*8*/ 426, /*9*/
                    479, /*10*/ 532, /*11*/ 585, /*12*/ 638, /*13*/ 691, /*14*/ 745, /*15*/
                    798, /*16*/ 851, /*17*/ 904, /*18*/ 957, /*19*/ 1010, /*20*/ 1064, /*21*/ 1117, 
        };

        private static ulong[] rgval64Power10By16 = new ulong[]
        {
            // powers of 10^16
            /*1*/ (0x8e1bc9bf04000000), /*2*/ (0x9dc5ada82b70b59e), /*3*/ (0xaf298d050e4395d6), /*4*/
                    (0xc2781f49ffcfa6d4), /*5*/ (0xd7e77a8f87daf7fa), /*6*/
                    (0xefb3ab16c59b14a0), /*7*/ (0x850fadc09923329c), /*8*/ (0x93ba47c980e98cde), /*9*/
                    (0xa402b9c5a8d3a6e6), /*10*/ (0xb616a12b7fe617a8), /*11*/
                    (0xca28a291859bbf90), /*12*/ (0xe070f78d39275566), /*13*/
                    (0xf92e0c3537826140), /*14*/ (0x8a5296ffe33cc92c), /*15*/
                    (0x9991a6f3d6bf1762), /*16*/ (0xaa7eebfb9df9de8a), /*17*/
                    (0xbd49d14aa79dbc7e), /*18*/ (0xd226fc195c6a2f88), /*19*/
                    (0xe950df20247c83f8), /*20*/ (0x81842f29f2cce373), /*21*/
                    (0x8fcac257558ee4e2),

                    // powers of 0.1^16
                    /*1*/ (0xe69594bec44de160), /*2*/ (0xcfb11ead453994c3), /*3*/
                    (0xbb127c53b17ec165), /*4*/ (0xa87fea27a539e9b3), /*5*/ (0x97c560ba6b0919b5), /*6*/
                    (0x88b402f7fd7553ab), /*7*/ (0xf64335bcf065d3a0), /*8*/
                    (0xddd0467c64bce4c4), /*9*/ (0xc7caba6e7c5382ed), /*10*/ (0xb3f4e093db73a0b7), /*11*/
                    (0xa21727db38cb0053), /*12*/ (0x91ff83775423cc29), /*13*/
                    (0x8380dea93da4bc82), /*14*/ (0xece53cec4a314f00), /*15*/
                    (0xd5605fcdcf32e217), /*16*/ (0xc0314325637a1978), /*17*/
                    (0xad1c8eab5ee43ba2), /*18*/ (0x9becce62836ac5b0), /*19*/
                    (0x8c71dcd9ba0b495c), /*20*/ (0xfd00b89747823938), /*21*/
                    (0xe3e27a444d8d991a),
        };

        private unsafe static void NumberToDouble(ref NUMBER number, double* value)
        {
            fixed (char* srcPtr = number.digits)
            {
                char* src = srcPtr;
                ulong val;
                int exp;
                int remaining;
                int total;
                int count;
                int scale;
                int absscale;
                int index;

                total = (int)wcslen(src);
                remaining = total;

                // skip the leading zeros
                while (*src == '0')
                {
                    remaining--;
                    src++;
                }

                if (remaining == 0)
                {
                    *value = 0.0;
                    goto done;
                }

                count = Math.Min(remaining, 9);
                remaining -= count;
                val = (ulong)DigitsToInt(src, count);

                if (remaining > 0)
                {
                    count = Math.Min(remaining, 9);
                    remaining -= count;

                    // get the denormalized power of 10
                    uint mult = (uint)(rgval64Power10[count - 1] >> (64 - rgexp64Power10[count - 1]));
                    val = (ulong)((uint)val * (uint)mult) + (ulong)DigitsToInt(src + 9, count);
                }

                scale = number.scale - (total - remaining);
                absscale = Math.Abs(scale);
                if (absscale >= 22 * 16)
                {
                    // overflow / underflow
                    *(ulong*)&value = (scale > 0) ? 0x7FF0000000000000U : 0;
                    goto done;
                }

                exp = 64;

                // normalize the mantisa
                if ((val & 0xFFFFFFFF00000000) == 0)
                {
                    val <<= 32;
                    exp -= 32;
                }
                if ((val & 0xFFFF000000000000) == 0)
                {
                    val <<= 16;
                    exp -= 16;
                }
                if ((val & 0xFF00000000000000) == 0)
                {
                    val <<= 8;
                    exp -= 8;
                }
                if ((val & 0xF000000000000000) == 0)
                {
                    val <<= 4;
                    exp -= 4;
                }
                if ((val & 0xC000000000000000) == 0)
                {
                    val <<= 2;
                    exp -= 2;
                }
                if ((val & 0x8000000000000000) == 0)
                {
                    val <<= 1;
                    exp -= 1;
                }

                index = absscale & 15;
                if (index > 0)
                {
                    int multexp = rgexp64Power10[index - 1];
                    // the exponents are shared between the inverted and regular table
                    exp += (scale < 0) ? (-multexp + 1) : multexp;

                    ulong multval = rgval64Power10[index + ((scale < 0) ? 15 : 0) - 1];
                    val = Mul64Lossy(val, multval, &exp);
                }

                index = absscale >> 4;
                if (index > 0)
                {
                    int multexp = rgexp64Power10By16[index - 1];
                    // the exponents are shared between the inverted and regular table
                    exp += (scale < 0) ? (-multexp + 1) : multexp;

                    ulong multval = rgval64Power10By16[index + ((scale < 0) ? 21 : 0) - 1];
                    val = Mul64Lossy(val, multval, &exp);
                }

                // round & scale down
                if (((uint)val & (1 << 10)) > 0)
                {
                    // IEEE round to even
                    ulong tmp = val + ((1 << 10) - 1) + (((uint)val >> 11) & 1);
                    if (tmp < val)
                    {
                        // overflow
                        tmp = (tmp >> 1) | (0x8000000000000000);
                        exp += 1;
                    }
                    val = tmp;
                }
                val >>= 11;

                exp += 0x3FE;

                if (exp <= 0)
                {
                    if (exp <= -52)
                    {
                        // underflow
                        val = 0;
                    }
                    else
                    {
                        // denormalized
                        val >>= (-exp + 1);
                    }
                }
                else if (exp >= 0x7FF)
                {
                    // overflow
                    val = (0x7FF0000000000000);
                }
                else
                {
                    val = ((ulong)exp << 52) + (val & (0x000FFFFFFFFFFFFF));
                }

                *(ulong*)&value = val;

            done:
                if (number.sign > 0) *(ulong*)&value |= (0x8000000000000000);
            }
        }

        private unsafe static ulong Mul64Lossy(ulong a, ulong b, int* pexp)
        {
            // it's ok to losse some precision here - Mul64 will be called
            // at most twice during the conversion, so the error won't propagate
            // to any of the 53 significant bits of the result
            ulong val = ((ulong)((uint)(a >> 32) * (uint)(b >> 32))) +
                (((ulong)((uint)(a >> 32) * (uint)b)) >> 32) +
                (((ulong)((uint)a * (uint)(b >> 32))) >> 32);

            // normalize
            if ((val & (0x8000000000000000)) == 0) { val <<= 1; *pexp -= 1; }

            return val;
        }

        private unsafe static int DigitsToInt(char* p, int count)
        {
            char* end = p + count;
            int res = *p - '0';
            for (p = p + 1; p < end; p++)
            {
                res = 10 * res + *p - '0';
            }

            return res;
        }

        private unsafe static long wcslen(char* s)
        {
            char* p = s;
            while (*p > 0)
                p++;

            return p - s;
        }

        private unsafe static char* MatchChars(char* p, string str)
        {
            fixed (char* stringPointer = str)
            {
                return MatchChars(p, stringPointer);
            }
        }

        private unsafe static char* MatchChars(char* p, char* str)
        {


            if (*str == '\0')
            {
                return null;
            }
            for (; (*str != '\0'); p++, str++)
            {
                if (*p != *str)
                { //We only hurt the failure case
                    if ((*str == '\u00A0') && (*p == '\u0020'))
                    {// This fix is for French or Kazakh cultures. Since a user cannot type 0xA0 as a 
                        // space character we use 0x20 space character instead to mean the same.
                        continue;
                    }
                    return null;
                }
            }
            return p;
        }


        internal unsafe static Decimal ParseDecimal(String value, NumberStyles options, NumberFormatInfo numfmt)
        {

            Byte* numberBufferBytes = stackalloc Byte[NumberBuffer.NumberBufferBytes];
            NumberBuffer number = new NumberBuffer(numberBufferBytes);
            Decimal result = 0;

            StringToNumber(value, options, ref number, numfmt, true);

            if (!NumberBufferToDecimal(number.PackForNative(), ref result))
            {
                throw new OverflowException(Environment.GetResourceString("Overflow_Decimal"));
            }
            return result;
        }


        internal unsafe static Double ParseDouble(String value, NumberStyles options, NumberFormatInfo numfmt)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            Byte* numberBufferBytes = stackalloc Byte[NumberBuffer.NumberBufferBytes];
            NumberBuffer number = new NumberBuffer(numberBufferBytes);
            Double d = 0;

            if (!TryStringToNumber(value, options, ref number, numfmt, false))
            {
                //If we failed TryStringToNumber, it may be from one of our special strings.
                //Check the three with which we're concerned and rethrow if it's not one of
                //those strings.
                String sTrim = value.Trim();
                if (sTrim.Equals(numfmt.PositiveInfinitySymbol))
                {
                    return Double.PositiveInfinity;
                }
                if (sTrim.Equals(numfmt.NegativeInfinitySymbol))
                {
                    return Double.NegativeInfinity;
                }
                if (sTrim.Equals(numfmt.NaNSymbol))
                {
                    return Double.NaN;
                }
                throw new FormatException(Environment.GetResourceString("Format_InvalidString"));
            }

            if (!NumberBufferToDouble(number.PackForNative(), ref d))
            {
                throw new OverflowException(Environment.GetResourceString("Overflow_Double"));
            }

            return d;
        }


        internal unsafe static Int32 ParseInt32(String s, NumberStyles style, NumberFormatInfo info)
        {

            Byte* numberBufferBytes = stackalloc Byte[NumberBuffer.NumberBufferBytes];
            NumberBuffer number = new NumberBuffer(numberBufferBytes);
            Int32 i = 0;

            StringToNumber(s, style, ref number, info, false);

            if ((style & NumberStyles.AllowHexSpecifier) != 0)
            {
                if (!HexNumberToInt32(ref number, ref i))
                {
                    throw new OverflowException(Environment.GetResourceString("Overflow_Int32"));
                }
            }
            else
            {
                if (!NumberToInt32(ref number, ref i))
                {
                    throw new OverflowException(Environment.GetResourceString("Overflow_Int32"));
                }
            }
            return i;
        }


        internal unsafe static Int64 ParseInt64(String value, NumberStyles options, NumberFormatInfo numfmt)
        {
            Byte* numberBufferBytes = stackalloc Byte[NumberBuffer.NumberBufferBytes];
            NumberBuffer number = new NumberBuffer(numberBufferBytes);
            Int64 i = 0;

            StringToNumber(value, options, ref number, numfmt, false);

            if ((options & NumberStyles.AllowHexSpecifier) != 0)
            {
                if (!HexNumberToInt64(ref number, ref i))
                {
                    throw new OverflowException(Environment.GetResourceString("Overflow_Int64"));
                }
            }
            else
            {
                if (!NumberToInt64(ref number, ref i))
                {
                    throw new OverflowException(Environment.GetResourceString("Overflow_Int64"));
                }
            }
            return i;
        }


        private unsafe static Boolean ParseNumber(ref char* str, NumberStyles options, ref NumberBuffer number, StringBuilder sb, NumberFormatInfo numfmt, Boolean parseDecimal)
        {

            const Int32 StateSign = 0x0001;
            const Int32 StateParens = 0x0002;
            const Int32 StateDigits = 0x0004;
            const Int32 StateNonZero = 0x0008;
            const Int32 StateDecimal = 0x0010;
            const Int32 StateCurrency = 0x0020;

            number.scale = 0;
            number.sign = false;
            string decSep;                  // decimal separator from NumberFormatInfo.
            string groupSep;                // group separator from NumberFormatInfo.
            string currSymbol = null;       // currency symbol from NumberFormatInfo.

            // The alternative currency symbol used in ANSI codepage, that can not roundtrip between ANSI and Unicode.
            // Currently, only ja-JP and ko-KR has non-null values (which is U+005c, backslash)
            string ansicurrSymbol = null;   // currency symbol from NumberFormatInfo.
            string altdecSep = null;        // decimal separator from NumberFormatInfo as a decimal
            string altgroupSep = null;      // group separator from NumberFormatInfo as a decimal

            Boolean parsingCurrency = false;
            if ((options & NumberStyles.AllowCurrencySymbol) != 0)
            {
                currSymbol = numfmt.CurrencySymbol;
                if (numfmt.ansiCurrencySymbol != null)
                {
                    ansicurrSymbol = numfmt.ansiCurrencySymbol;
                }
                // The idea here is to match the currency separators and on failure match the number separators to keep the perf of VB's IsNumeric fast.
                // The values of decSep are setup to use the correct relevant separator (currency in the if part and decimal in the else part).
                altdecSep = numfmt.NumberDecimalSeparator;
                altgroupSep = numfmt.NumberGroupSeparator;
                decSep = numfmt.CurrencyDecimalSeparator;
                groupSep = numfmt.CurrencyGroupSeparator;
                parsingCurrency = true;
            }
            else
            {
                decSep = numfmt.NumberDecimalSeparator;
                groupSep = numfmt.NumberGroupSeparator;
            }

            Int32 state = 0;
            Boolean signflag = false; // Cache the results of "options & PARSE_LEADINGSIGN && !(state & STATE_SIGN)" to avoid doing this twice
            Boolean bigNumber = (sb != null); // When a StringBuilder is provided then we use it in place of the number.digits char[50]
            Boolean bigNumberHex = (bigNumber && ((options & NumberStyles.AllowHexSpecifier) != 0));
            Int32 maxParseDigits = bigNumber ? Int32.MaxValue : NumberMaxDigits;

            char* p = str;
            char ch = *p;
            char* next;

            while (true)
            {
                // Eat whitespace unless we've found a sign which isn't followed by a currency symbol.
                // "-Kr 1231.47" is legal but "- 1231.47" is not.
                if (IsWhite(ch) && ((options & NumberStyles.AllowLeadingWhite) != 0) && (((state & StateSign) == 0) || (((state & StateSign) != 0) && (((state & StateCurrency) != 0) || numfmt.numberNegativePattern == 2))))
                {
                    // Do nothing here. We will increase p at the end of the loop.
                }
                else if ((signflag = (((options & NumberStyles.AllowLeadingSign) != 0) && ((state & StateSign) == 0))) && ((next = MatchChars(p, numfmt.positiveSign)) != null))
                {
                    state |= StateSign;
                    p = next - 1;
                }
                else if (signflag && (next = MatchChars(p, numfmt.negativeSign)) != null)
                {
                    state |= StateSign;
                    number.sign = true;
                    p = next - 1;
                }
                else if (ch == '(' && ((options & NumberStyles.AllowParentheses) != 0) && ((state & StateSign) == 0))
                {
                    state |= StateSign | StateParens;
                    number.sign = true;
                }
                else if ((currSymbol != null && (next = MatchChars(p, currSymbol)) != null) || (ansicurrSymbol != null && (next = MatchChars(p, ansicurrSymbol)) != null))
                {
                    state |= StateCurrency;
                    currSymbol = null;
                    ansicurrSymbol = null;
                    // We already found the currency symbol. There should not be more currency symbols. Set
                    // currSymbol to null so that we won't search it again in the later code path.
                    p = next - 1;
                }
                else
                {
                    break;
                }
                ch = *++p;
            }
            Int32 digCount = 0;
            Int32 digEnd = 0;
            while (true)
            {
                if ((ch >= '0' && ch <= '9') || (((options & NumberStyles.AllowHexSpecifier) != 0) && ((ch >= 'a' && ch <= 'f') || (ch >= 'A' && ch <= 'F'))))
                {
                    state |= StateDigits;

                    if (ch != '0' || (state & StateNonZero) != 0 || bigNumberHex)
                    {
                        if (digCount < maxParseDigits)
                        {
                            if (bigNumber)
                                sb.Append(ch);
                            else
                                number.digits[digCount++] = ch;
                            if (ch != '0' || parseDecimal)
                            {
                                digEnd = digCount;
                            }
                        }
                        if ((state & StateDecimal) == 0)
                        {
                            number.scale++;
                        }
                        state |= StateNonZero;
                    }
                    else if ((state & StateDecimal) != 0)
                    {
                        number.scale--;
                    }
                }
                else if (((options & NumberStyles.AllowDecimalPoint) != 0) && ((state & StateDecimal) == 0) && ((next = MatchChars(p, decSep)) != null || ((parsingCurrency) && (state & StateCurrency) == 0) && (next = MatchChars(p, altdecSep)) != null))
                {
                    state |= StateDecimal;
                    p = next - 1;
                }
                else if (((options & NumberStyles.AllowThousands) != 0) && ((state & StateDigits) != 0) && ((state & StateDecimal) == 0) && ((next = MatchChars(p, groupSep)) != null || ((parsingCurrency) && (state & StateCurrency) == 0) && (next = MatchChars(p, altgroupSep)) != null))
                {
                    p = next - 1;
                }
                else
                {
                    break;
                }
                ch = *++p;
            }

            Boolean negExp = false;
            number.precision = digEnd;
            if (bigNumber)
                sb.Append('\0');
            else
                number.digits[digEnd] = '\0';
            if ((state & StateDigits) != 0)
            {
                if ((ch == 'E' || ch == 'e') && ((options & NumberStyles.AllowExponent) != 0))
                {
                    char* temp = p;
                    ch = *++p;
                    if ((next = MatchChars(p, numfmt.positiveSign)) != null)
                    {
                        ch = *(p = next);
                    }
                    else if ((next = MatchChars(p, numfmt.negativeSign)) != null)
                    {
                        ch = *(p = next);
                        negExp = true;
                    }
                    if (ch >= '0' && ch <= '9')
                    {
                        Int32 exp = 0;
                        do
                        {
                            exp = exp * 10 + (ch - '0');
                            ch = *++p;
                            if (exp > 1000)
                            {
                                exp = 9999;
                                while (ch >= '0' && ch <= '9')
                                {
                                    ch = *++p;
                                }
                            }
                        } while (ch >= '0' && ch <= '9');
                        if (negExp)
                        {
                            exp = -exp;
                        }
                        number.scale += exp;
                    }
                    else
                    {
                        p = temp;
                        ch = *p;
                    }
                }
                while (true)
                {
                    if (IsWhite(ch) && ((options & NumberStyles.AllowTrailingWhite) != 0))
                    {
                    }
                    else if ((signflag = (((options & NumberStyles.AllowTrailingSign) != 0) && ((state & StateSign) == 0))) && (next = MatchChars(p, numfmt.positiveSign)) != null)
                    {
                        state |= StateSign;
                        p = next - 1;
                    }
                    else if (signflag && (next = MatchChars(p, numfmt.negativeSign)) != null)
                    {
                        state |= StateSign;
                        number.sign = true;
                        p = next - 1;
                    }
                    else if (ch == ')' && ((state & StateParens) != 0))
                    {
                        state &= ~StateParens;
                    }
                    else if ((currSymbol != null && (next = MatchChars(p, currSymbol)) != null) || (ansicurrSymbol != null && (next = MatchChars(p, ansicurrSymbol)) != null))
                    {
                        currSymbol = null;
                        ansicurrSymbol = null;
                        p = next - 1;
                    }
                    else
                    {
                        break;
                    }
                    ch = *++p;
                }
                if ((state & StateParens) == 0)
                {
                    if ((state & StateNonZero) == 0)
                    {
                        if (!parseDecimal)
                        {
                            number.scale = 0;
                        }
                        if ((state & StateDecimal) == 0)
                        {
                            number.sign = false;
                        }
                    }
                    str = p;
                    return true;
                }
            }
            str = p;
            return false;
        }


        internal unsafe static Single ParseSingle(String value, NumberStyles options, NumberFormatInfo numfmt)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            Byte* numberBufferBytes = stackalloc Byte[NumberBuffer.NumberBufferBytes];
            NumberBuffer number = new NumberBuffer(numberBufferBytes);
            Double d = 0;

            if (!TryStringToNumber(value, options, ref number, numfmt, false))
            {
                //If we failed TryStringToNumber, it may be from one of our special strings.
                //Check the three with which we're concerned and rethrow if it's not one of
                //those strings.
                String sTrim = value.Trim();
                if (sTrim.Equals(numfmt.PositiveInfinitySymbol))
                {
                    return Single.PositiveInfinity;
                }
                if (sTrim.Equals(numfmt.NegativeInfinitySymbol))
                {
                    return Single.NegativeInfinity;
                }
                if (sTrim.Equals(numfmt.NaNSymbol))
                {
                    return Single.NaN;
                }
                throw new FormatException(Environment.GetResourceString("Format_InvalidString"));
            }

            if (!NumberBufferToDouble(number.PackForNative(), ref d))
            {
                throw new OverflowException(Environment.GetResourceString("Overflow_Single"));
            }
            Single castSingle = (Single)d;
            if (Single.IsInfinity(castSingle))
            {
                throw new OverflowException(Environment.GetResourceString("Overflow_Single"));
            }
            return castSingle;
        }


        internal unsafe static UInt32 ParseUInt32(String value, NumberStyles options, NumberFormatInfo numfmt)
        {

            Byte* numberBufferBytes = stackalloc Byte[NumberBuffer.NumberBufferBytes];
            NumberBuffer number = new NumberBuffer(numberBufferBytes);
            UInt32 i = 0;

            StringToNumber(value, options, ref number, numfmt, false);

            if ((options & NumberStyles.AllowHexSpecifier) != 0)
            {
                if (!HexNumberToUInt32(ref number, ref i))
                {
                    throw new OverflowException(Environment.GetResourceString("Overflow_UInt32"));
                }
            }
            else
            {
                if (!NumberToUInt32(ref number, ref i))
                {
                    throw new OverflowException(Environment.GetResourceString("Overflow_UInt32"));
                }
            }

            return i;
        }


        internal unsafe static UInt64 ParseUInt64(String value, NumberStyles options, NumberFormatInfo numfmt)
        {
            Byte* numberBufferBytes = stackalloc Byte[NumberBuffer.NumberBufferBytes];
            NumberBuffer number = new NumberBuffer(numberBufferBytes);
            UInt64 i = 0;

            StringToNumber(value, options, ref number, numfmt, false);
            if ((options & NumberStyles.AllowHexSpecifier) != 0)
            {
                if (!HexNumberToUInt64(ref number, ref i))
                {
                    throw new OverflowException(Environment.GetResourceString("Overflow_UInt64"));
                }
            }
            else
            {
                if (!NumberToUInt64(ref number, ref i))
                {
                    throw new OverflowException(Environment.GetResourceString("Overflow_UInt64"));
                }
            }
            return i;
        }


        private unsafe static void StringToNumber(String str, NumberStyles options, ref NumberBuffer number, NumberFormatInfo info, Boolean parseDecimal)
        {

            if (str == null)
            {
                throw new ArgumentNullException("String");
            }

            fixed (char* stringPointer = str)
            {
                char* p = stringPointer;
                if (!ParseNumber(ref p, options, ref number, null, info, parseDecimal)
                    || (p - stringPointer < str.Length && !TrailingZeros(str, (int)(p - stringPointer))))
                {
                    throw new FormatException(Environment.GetResourceString("Format_InvalidString"));
                }
            }
        }

        private static Boolean TrailingZeros(String s, Int32 index)
        {
            // For compatability, we need to allow trailing zeros at the end of a number string
            for (int i = index; i < s.Length; i++)
            {
                if (s[i] != '\0')
                {
                    return false;
                }
            }
            return true;
        }


        internal unsafe static Boolean TryParseDecimal(String value, NumberStyles options, NumberFormatInfo numfmt, out Decimal result)
        {

            Byte* numberBufferBytes = stackalloc Byte[NumberBuffer.NumberBufferBytes];
            NumberBuffer number = new NumberBuffer(numberBufferBytes);
            result = 0;

            if (!TryStringToNumber(value, options, ref number, numfmt, true))
            {
                return false;
            }

            if (!NumberBufferToDecimal(number.PackForNative(), ref result))
            {
                return false;
            }
            return true;
        }


        internal unsafe static Boolean TryParseDouble(String value, NumberStyles options, NumberFormatInfo numfmt, out Double result)
        {
            Byte* numberBufferBytes = stackalloc Byte[NumberBuffer.NumberBufferBytes];
            NumberBuffer number = new NumberBuffer(numberBufferBytes);
            result = 0;


            if (!TryStringToNumber(value, options, ref number, numfmt, false))
            {
                return false;
            }
            if (!NumberBufferToDouble(number.PackForNative(), ref result))
            {
                return false;
            }
            return true;
        }


        internal unsafe static Boolean TryParseInt32(String s, NumberStyles style, NumberFormatInfo info, out Int32 result)
        {

            Byte* numberBufferBytes = stackalloc Byte[NumberBuffer.NumberBufferBytes];
            NumberBuffer number = new NumberBuffer(numberBufferBytes);
            result = 0;

            if (!TryStringToNumber(s, style, ref number, info, false))
            {
                return false;
            }

            if ((style & NumberStyles.AllowHexSpecifier) != 0)
            {
                if (!HexNumberToInt32(ref number, ref result))
                {
                    return false;
                }
            }
            else
            {
                if (!NumberToInt32(ref number, ref result))
                {
                    return false;
                }
            }
            return true;
        }


        internal unsafe static Boolean TryParseInt64(String s, NumberStyles style, NumberFormatInfo info, out Int64 result)
        {

            Byte* numberBufferBytes = stackalloc Byte[NumberBuffer.NumberBufferBytes];
            NumberBuffer number = new NumberBuffer(numberBufferBytes);
            result = 0;

            if (!TryStringToNumber(s, style, ref number, info, false))
            {
                return false;
            }

            if ((style & NumberStyles.AllowHexSpecifier) != 0)
            {
                if (!HexNumberToInt64(ref number, ref result))
                {
                    return false;
                }
            }
            else
            {
                if (!NumberToInt64(ref number, ref result))
                {
                    return false;
                }
            }
            return true;
        }


        internal unsafe static Boolean TryParseSingle(String value, NumberStyles options, NumberFormatInfo numfmt, out Single result)
        {
            Byte* numberBufferBytes = stackalloc Byte[NumberBuffer.NumberBufferBytes];
            NumberBuffer number = new NumberBuffer(numberBufferBytes);
            result = 0;
            Double d = 0;

            if (!TryStringToNumber(value, options, ref number, numfmt, false))
            {
                return false;
            }
            if (!NumberBufferToDouble(number.PackForNative(), ref d))
            {
                return false;
            }
            Single castSingle = (Single)d;
            if (Single.IsInfinity(castSingle))
            {
                return false;
            }

            result = castSingle;
            return true;
        }


        internal unsafe static Boolean TryParseUInt32(String s, NumberStyles style, NumberFormatInfo info, out UInt32 result)
        {

            Byte* numberBufferBytes = stackalloc Byte[NumberBuffer.NumberBufferBytes];
            NumberBuffer number = new NumberBuffer(numberBufferBytes);
            result = 0;

            if (!TryStringToNumber(s, style, ref number, info, false))
            {
                return false;
            }

            if ((style & NumberStyles.AllowHexSpecifier) != 0)
            {
                if (!HexNumberToUInt32(ref number, ref result))
                {
                    return false;
                }
            }
            else
            {
                if (!NumberToUInt32(ref number, ref result))
                {
                    return false;
                }
            }
            return true;
        }


        internal unsafe static Boolean TryParseUInt64(String s, NumberStyles style, NumberFormatInfo info, out UInt64 result)
        {

            Byte* numberBufferBytes = stackalloc Byte[NumberBuffer.NumberBufferBytes];
            NumberBuffer number = new NumberBuffer(numberBufferBytes);
            result = 0;

            if (!TryStringToNumber(s, style, ref number, info, false))
            {
                return false;
            }

            if ((style & NumberStyles.AllowHexSpecifier) != 0)
            {
                if (!HexNumberToUInt64(ref number, ref result))
                {
                    return false;
                }
            }
            else
            {
                if (!NumberToUInt64(ref number, ref result))
                {
                    return false;
                }
            }
            return true;
        }

        internal static Boolean TryStringToNumber(String str, NumberStyles options, ref NumberBuffer number, NumberFormatInfo numfmt, Boolean parseDecimal)
        {
            return TryStringToNumber(str, options, ref number, null, numfmt, parseDecimal);
        }



        internal unsafe static Boolean TryStringToNumber(String str, NumberStyles options, ref NumberBuffer number, StringBuilder sb, NumberFormatInfo numfmt, Boolean parseDecimal)
        {

            if (str == null)
            {
                return false;
            }

            fixed (char* stringPointer = str)
            {
                char* p = stringPointer;
                if (!ParseNumber(ref p, options, ref number, sb, numfmt, parseDecimal)
                    || (p - stringPointer < str.Length && !TrailingZeros(str, (int)(p - stringPointer))))
                {
                    return false;
                }
            }

            return true;
        }

        private static char ParseFormatSpecifier(string str, out int digits)
        {
            if (str != null)
            {
                var i = 0;
                char ch = str[i];
                if (i < str.Length)
                {
                    if (ch >= 'A' && ch <= 'Z' || ch >= 'a' && ch <= 'z')
                    {
                        i++;
                        int n = -1;
                        if (str[i] >= '0' && str[i] <= '9')
                        {
                            n = str[i++] - '0';
                            while (str[i] >= '0' && str[i] <= '9')
                            {
                                n = n * 10 + str[i++] - '0';
                                if (n >= 10) break;
                            }
                        }
                        if (i >= str.Length)
                        {
                            digits = n;
                            return ch;
                        }
                    }

                    digits = -1;
                    return '\0';
                }
            }

            digits = -1;
            return 'G';
        }

        private static unsafe void Int32ToNumber(int value, ref NUMBER number)
        {
            char* buffer = stackalloc char[INT32_PRECISION + 1];
            number.precision = INT32_PRECISION;
            if (value >= 0)
            {
                number.sign = 0;
            }
            else
            {
                number.sign = 1;
            }

            fixed (char* dstPtr = number.digits)
            {
                char* dst = dstPtr;
                char* p = Int32ToDecChars(buffer + INT32_PRECISION, value, 0);
                int i = (int)(buffer + INT32_PRECISION - p);
                number.scale = i;

                Console.WriteLine("scale");
                Console.WriteLine(i);

                while (--i >= 0) *dst++ = *p++;
                *dst = '\0';
            }
        }

        private static unsafe void UInt32ToNumber(uint value, ref NUMBER number)
        {
            char* buffer = stackalloc char[UINT32_PRECISION + 1];
            number.precision = UINT32_PRECISION;
            number.sign = 0;

            fixed (char* dstPtr = number.digits)
            {
                char* dst = dstPtr;
                char* p = Int32ToDecChars(buffer + UINT32_PRECISION, value, 0);
                int i = (int)(buffer + UINT32_PRECISION - p);
                number.scale = i;
                while (--i >= 0) *dst++ = *p++;
                *dst = '\0';
            }
        }

        private static unsafe void Int64ToNumber(long value, ref NUMBER number)
        {
            char* buffer = stackalloc char[long_PRECISION + 1];
            number.precision = long_PRECISION;
            if (value >= 0)
            {
                number.sign = 0;
            }
            else
            {
                number.sign = 1;
            }

            fixed (char* dstPtr = number.digits)
            {
                char* dst = dstPtr;
                char* p = buffer + long_PRECISION;
                while (((ulong)value & 0xFFFFFFFF00000000) > 0)
                {
                    p = Int32ToDecChars(p, Int64DivMod1E9((ulong*)&value), 9);
                }
                p = Int32ToDecChars(p, value, 0);
                int i = (int)(buffer + long_PRECISION - p);
                number.scale = i;
                while (--i >= 0) *dst++ = *p++;
                *dst = '\0';
            }
        }

        private static unsafe void UInt64ToNumber(ulong value, ref NUMBER number)
        {
            char* buffer = stackalloc char[Ulong_PRECISION + 1];
            number.precision = Ulong_PRECISION;
            number.sign = 0;

            fixed (char* dstPtr = number.digits)
            {
                char* dst = dstPtr;
                char* p = buffer + Ulong_PRECISION;
                while (((ulong)value & 0xFFFFFFFF00000000) > 0)
                {
                    p = Int32ToDecChars(p, Int64DivMod1E9((ulong*)&value), 9);
                }
                p = Int32ToDecChars(p, value, 0);
                int i = (int)(buffer + Ulong_PRECISION - p);
                number.scale = i;
                while (--i >= 0) *dst++ = *p++;
                *dst = '\0';
            }
        }

        private unsafe static void DoubleToNumber(double value, int precision, ref NUMBER number)
        {
            number.precision = precision;
            fixed (char* dstPtr = number.digits)
            {
                char* dst = dstPtr;
                char* src = DoubleHelper.ecvt(value, precision, ref number.scale, ref number.sign);
                if (*src != '0')
                {
                    while (*src > 0) *dst++ = *src++;
                }

                *dst = '\0';
            }
        }

        private unsafe static uint Int64DivMod1E9(ulong* value)
        {
            var rem = (uint)(*value % 1000000000);
            *value /= 1000000000;
            return rem;
        }

        private unsafe static char* Int32ToDecChars(char* p, uint value, int digits)
        {
            while (--digits >= 0 || value != 0)
            {
                *--p = (char)(value % 10 + '0');
                value /= 10;
            }

            return p;
        }

        private unsafe static char* Int32ToDecChars(char* p, ulong value, int digits)
        {
            while (--digits >= 0 || value != 0)
            {
                *--p = (char)(value % 10 + '0');
                value /= 10;
            }

            return p;
        }

        private unsafe static char* Int32ToDecChars(char* p, int value, int digits)
        {
            if (value >= 0)
            {
                while (--digits >= 0 || value != 0)
                {
                    *--p = (char)(value % 10 + '0');
                    value /= 10;
                }
            }
            else
            {
                while (--digits >= 0 || value != 0)
                {
                    *--p = (char)('0' - value % 10);
                    value /= 10;
                }
            }

            return p;
        }

        private unsafe static char* Int32ToDecChars(char* p, long value, int digits)
        {
            if (value >= 0)
            {
                while (--digits >= 0 || value != 0)
                {
                    *--p = (char)(value % 10 + '0');
                    value /= 10;
                }
            }
            else
            {
                while (--digits >= 0 || value != 0)
                {
                    *--p = (char)('0' - value % 10);
                    value /= 10;
                }
            }

            return p;
        }

        private unsafe static char* Int32ToHexChars(char* p, uint value, int hexBase, int digits)
        {
            while (--digits >= 0 || value != 0)
            {
                int digit = (int)(value & 0xF);
                *--p = (char)(digit + (digit < 10 ? '0' : hexBase));
                value >>= 4;
            }

            return p;
        }

        private unsafe static string Int32ToHexStr(uint value, int hexBase, int digits)
        {
            char* buffer = stackalloc char[100];
            if (digits < 1) digits = 1;
            char* p = Int32ToHexChars(buffer + 100, value, hexBase, digits);
            return new String(p, 0, (int)(buffer + 100 - p));
        }

        private unsafe static string Int64ToHexStr(ulong value, int hexBase, int digits)
        {
            char* buffer = stackalloc char[100];
            char* p;
            if ((value & 0xffffffff00000000) > 0)
            {
                Int32ToHexChars(buffer + 100, (uint)value, hexBase, 8);
                p = Int32ToHexChars(buffer + 100 - 8, (uint)(value & 0xffffffff00000000) >> 32, hexBase, digits - 8);
            }
            else
            {
                if (digits < 1) digits = 1;
                p = Int32ToHexChars(buffer + 100, (uint)value, hexBase, digits);
            }

            return new String(p, 0, (int)(buffer + 100 - p));
        }

        private unsafe static string Int32ToDecStr(int value, int digits, string sNegative)
        {
            if (digits < 1) digits = 1;

            int maxDigitsLength = (digits > 15) ? digits : 15; // Since an int32 can have maximum of 10 chars as a String
            int bufferLength = (maxDigitsLength > 100) ? maxDigitsLength : 100;
            int negLength = 0;
            fixed (char* src = sNegative)
            {
                if (value < 0)
                {
                    negLength = sNegative.Length;
                    if (negLength > bufferLength - maxDigitsLength)
                    {
                        bufferLength = negLength + maxDigitsLength;
                    }
                }

                char* buffer = stackalloc char[bufferLength];
                char* p = Int32ToDecChars(buffer + bufferLength, value, digits);
                if (value < 0)
                {
                    for (int i = negLength - 1; i >= 0; i--)
                    {
                        *(--p) = *(src + i);
                    }
                }

                return new String(p, 0, (int)(buffer + bufferLength - p));
            }
        }

        private unsafe static string UInt32ToDecStr(uint value, int digits)
        {
            char* buffer = stackalloc char[100];
            if (digits < 1) digits = 1;
            char* p = Int32ToDecChars(buffer + 100, value, digits);
            return new String(p, 0, (int)(buffer + 100 - p));
        }

        private unsafe static string Int64ToDecStr(long value, int digits, string sNegative)
        {
            if (digits < 1) digits = 1;
            int sign = (int)((ulong)value & 0xffffffff00000000) >> 32;

            int maxDigitsLength = (digits > 20) ? digits : 20;
            int bufferLength = (maxDigitsLength > 100) ? maxDigitsLength : 100;
            int negLength = 0;
            fixed (char* src = sNegative)
            {
                if (sign < 0)
                {
                    negLength = sNegative.Length;
                    if (negLength > bufferLength - maxDigitsLength)
                    {
                        bufferLength = negLength + maxDigitsLength;
                    }
                }

                char* buffer = stackalloc char[bufferLength];
                char* p = buffer + bufferLength;
                while (((ulong)value & 0xffffffff00000000) > 0)
                {
                    p = Int32ToDecChars(p, Int64DivMod1E9((ulong*)&value), 9);
                    digits -= 9;
                }
                p = Int32ToDecChars(p, value, digits);

                if (sign < 0)
                {
                    for (int i = negLength - 1; i >= 0; i--)
                    {
                        *(--p) = *(src + i);
                    }
                }

                return new String(p, 0, (int)(buffer + bufferLength - p));
            }
        }

        private unsafe static string UInt64ToDecStr(ulong value, int digits)
        {
            char* buffer = stackalloc char[100];
            if (digits < 1) digits = 1;

            char* p = buffer + 100;
            while ((value & 0xffffffff00000000) > 0)
            {
                p = Int32ToDecChars(p, Int64DivMod1E9(&value), 9);
                digits -= 9;
            }

            p = Int32ToDecChars(p, value, digits);
            return new String(p, 0, (int)(buffer + 100 - p));
        }

        public unsafe struct NUMBER
        {
            public const int NUMBER_MAXDIGITS = 50;

            public int precision;
            public int scale;
            public int sign;
            public fixed char digits[NUMBER_MAXDIGITS + 2];
        }

        internal unsafe static class DoubleHelper
        {
            private const int NDIG = 350;

            internal static char* ecvt(double arg, int ndigits, ref int decptp, ref int signp)
            {
                return cvt(arg, ndigits, ref decptp, ref signp, 1);
            }

            internal static char* fcvt(double arg, int ndigits, ref int decptp, ref int signp)
            {
                return cvt(arg, ndigits, ref decptp, ref signp, 0);
            }

            private static char* cvt(double arg, int ndigits, ref int decptp, ref int signp, int eflag)
            {
                int decpt;
                double fi = 0.0, fj = 0.0;
                char* p;
                char* p1;
                char* buf = stackalloc char[NDIG];

                if (ndigits < 0)
                    ndigits = 0;
                if (ndigits >= NDIG - 1)
                    ndigits = NDIG - 2;

                decpt = 0;
                signp = 0;
                p = &buf[0];

                if (arg == 0)
                {
                    decptp = 0;
                    while (p < &buf[ndigits])
                        *p++ = '0';
                    *p = '\0';
                    return (buf);
                }
                else if (arg < 0)
                {
                    signp = 1;
                    arg = -arg;
                }

                arg = modf(arg, ref fi);
                p1 = &buf[NDIG];

                /* Do integer part */

                if (fi != 0)
                {
                    while (fi != 0)
                    {
                        fj = modf(fi / 10, ref fi);
                        *--p1 = (char)((int)((fj + 0.03) * 10) + '0');
                        decpt++;
                    }
                    while (p1 < &buf[NDIG])
                        *p++ = *p1++;
                }
                else if (arg > 0)
                {
                    while ((fj = arg * 10) < 1)
                    {
                        arg = fj;
                        decpt--;
                    }
                }
                decptp = decpt;

                /* Do the fractional part.
                 * p pts to where fraction should be concatenated.
                 * p1 is how far conversion must go to.
                 */
                p1 = &buf[ndigits];
                if (eflag == 0)
                {
                    /* fcvt must provide ndigits after decimal pt */
                    p1 += decpt;
                    /* if decpt was negative, we might be done for fcvt */
                    if (p1 < &buf[0])
                    {
                        buf[0] = '\0';
                        return (buf);
                    }
                }

                while (p <= p1 && p < &buf[NDIG])
                {
                    arg *= 10;
                    arg = modf(arg, ref fj);
                    *p++ = (char)((int)fj + '0');
                }

                /* If we converted all the way to the end of the buf, don't mess with
                 * rounding since there's nothing significant out here anyway.
                 */
                if (p1 >= &buf[NDIG])
                {
                    buf[NDIG - 1] = '\0';
                    return (buf);
                }

                /* Round by adding 5 to last digit and propagating carries. */
                p = p1;
                *p1 += (char)5;
                while (*p1 > '9')
                {
                    *p1 = '0';
                    if (p1 > buf)
                    {
                        ++*--p1;
                    }
                    else
                    {
                        *p1 = '1';
                        (decptp)++;
                        if (eflag == 0)
                        {
                            if (p > buf)
                                *p = '0';
                            p++;
                        }
                    }
                }
                *p = '\0';
                return (buf);
            }
        }
    }
}
