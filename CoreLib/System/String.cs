////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Apache License 2.0 (Apache)
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
namespace System
{
    using System;
    using System.Globalization;
    using System.Threading;
    using System.Collections;
    using System.Runtime.CompilerServices;
    using System.Text;
    /**
     * <p>The <code>String</code> class represents a static string of characters.  Many of
     * the <code>String</code> methods perform some type of transformation on the current
     * instance and return the result as a new <code>String</code>. All comparison methods are
     * implemented as a part of <code>String</code>.</p>  As with arrays, character positions
     * (indices) are zero-based.
     *
     * <p>When passing a null string into a constructor in VJ and VC, the null should be
     * explicitly type cast to a <code>String</code>.</p>
     * <p>For Example:<br>
     * <pre>String s = new String((String)null);
     * Text.Out.WriteLine(s);</pre></p>
     *
     * @author Jay Roxe (jroxe)
     * @version
     */
    [Serializable]
    public sealed class String : IComparable
    {
        public static readonly String Empty = "";

        private const int TrimHead = 0;
        private const int TrimTail = 1;
        private const int TrimBoth = 2;

        private int m_stringLength;
        private char m_firstChar;

        public override int GetHashCode()
        {
            unsafe
            {
                fixed (char* src = this)
                {
                    int hash1 = 5381;
                    int hash2 = hash1;

                    int c;
                    char* s = src;
                    while ((c = s[0]) != 0)
                    {
                        hash1 = ((hash1 << 5) + hash1) ^ c;
                        c = s[1];
                        if (c == 0)
                            break;
                        hash2 = ((hash2 << 5) + hash2) ^ c;
                        s += 2;
                    }

                    return hash1 + (hash2 * 1566083941);
                }
            }
        }

        public override bool Equals(object obj)
        {
            var s = obj as string;
            if (s != null)
            {
                return Equals(this, s);
            }

            return false;
        }

        public static bool Equals(String a, String b)
        {
            if ((Object)a == (Object)b)
            {
                return true;
            }

            if ((Object)a == null || (Object)b == null)
            {
                return false;
            }

            if (a.Length != b.Length)
                return false;

            return EqualsHelper(a, b);
        }

        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        internal extern static String FastAllocateString(int length);

        public static String Format(String format, Object arg0)
        {
            return Format(null, format, new Object[] { arg0 });
        }

        public static String Format(String format, Object arg0, Object arg1)
        {
            return Format(null, format, new Object[] { arg0, arg1 });
        }

        public static String Format(String format, Object arg0, Object arg1, Object arg2)
        {
            return Format(null, format, new Object[] { arg0, arg1, arg2 });
        }

        public static String Format(String format, params Object[] args)
        {
            return Format(null, format, args);
        }

        public static String Format(IFormatProvider provider, String format, params Object[] args)
        {
            var sb = new Text.StringBuilder();
            sb.AppendFormat(provider, format, args);
            return sb.ToString();
        }

        public static bool operator ==(String a, String b)
        {
            return Equals(a, b);
        }


        public static bool operator !=(String a, String b)
        {
            return !Equals(a, b);
        }

        [System.Runtime.CompilerServices.IndexerName("Chars")]
        public char this[int index]
        {
            get
            {
                if (index < 0 || index >= m_stringLength)
                {
                    throw new ArgumentOutOfRangeException("index");
                }

                unsafe
                {
                    fixed (char* p = this)
                    {
                        return *(p + index);
                    }
                }
            }
        }

        public unsafe char[] ToCharArray()
        {
            int length = Length;
            char[] chars = new char[length];
            if (length > 0)
            {
                fixed (char* src = &this.m_firstChar)
                fixed (char* dest = chars)
                {
                    wstrcpy(dest, src, length);
                }
            }
            return chars;
        }

        public unsafe char[] ToCharArray(int startIndex, int length)
        {
            // Range check everything.
            if (startIndex < 0 || startIndex > Length || startIndex > Length - length)
                throw new ArgumentOutOfRangeException("startIndex");
            if (length < 0)
                throw new ArgumentOutOfRangeException("length");

            char[] chars = new char[length];
            if (length > 0)
            {
                fixed (char* src = &this.m_firstChar)
                fixed (char* dest = chars)
                {
                    wstrcpy(dest, src + startIndex, length);
                }
            }

            return chars;
        }

        public int Length
        {
            get
            {
                return m_stringLength;
            }
        }

        public String[] Split(params char[] separator)
        {
            throw new NotImplementedException();
        }

        public String[] Split(char[] separator, int count)
        {
            throw new NotImplementedException();
        }

        public String Substring(int startIndex)
        {
            throw new NotImplementedException();
        }


        public String Substring(int startIndex, int length)
        {
            throw new NotImplementedException();
        }


        public String Trim(params char[] trimChars)
        {
            return this.TrimHelper(trimChars, TrimBoth);
        }


        public String TrimStart(params char[] trimChars)
        {
            return this.TrimHelper(trimChars, TrimHead);
        }

        public String TrimEnd(params char[] trimChars)
        {
            return this.TrimHelper(trimChars, TrimTail);
        }

        private unsafe static bool EqualsHelper(String strA, String strB)
        {
            int length = strA.Length;

            fixed (char* ap = &strA.m_firstChar) fixed (char* bp = &strB.m_firstChar)
            {
                char* a = ap;
                char* b = bp;

                while (length >= 10)
                {
                    if (*(int*)a != *(int*)b) return false;
                    if (*(int*)(a + 2) != *(int*)(b + 2)) return false;
                    if (*(int*)(a + 4) != *(int*)(b + 4)) return false;
                    if (*(int*)(a + 6) != *(int*)(b + 6)) return false;
                    if (*(int*)(a + 8) != *(int*)(b + 8)) return false;
                    a += 10; b += 10; length -= 10;
                }

                // This depends on the fact that the String objects are
                // always zero terminated and that the terminating zero is not included
                // in the length. For odd string sizes, the last compare will include
                // the zero terminator.
                while (length > 0)
                {
                    if (*(int*)a != *(int*)b) break;
                    a += 2; b += 2; length -= 2;
                }

                return (length <= 0);
            }
        }

        private String TrimHelper(char[] trimChars, int trimType)
        {
            throw new NotImplementedException();
        }

        public String(char[] value, int startIndex, int length)
        {
            throw new NotImplementedException();
        }

        public String(char[] value)
        {
            throw new NotImplementedException();
        }

        public String(char c, int count)
        {
            throw new NotImplementedException();
        }

        public unsafe String(char* src, int startIndex, int length)
        {
            throw new NotImplementedException();
        }

        public unsafe String(sbyte* src)
        {
            throw new NotImplementedException();
        }

        public unsafe String(sbyte* src, int startIndex, int length)
        {
            throw new NotImplementedException();
        }

        public unsafe String(sbyte* src, int startIndex, int length, Encoding enc)
        {
            throw new NotImplementedException();
        }

        public unsafe static int Compare(string strA, int indexA, string strB, int indexB, int length)
        {
            fixed (char* ap = &strA.m_firstChar) fixed (char* bp = &strB.m_firstChar)
            {
                char* a = ap;
                char* b = bp;

                a += indexA;
                b += indexB;

                // This depends on the fact that the String objects are
                // always zero terminated and that the terminating zero is not included
                // in the length. For odd string sizes, the last compare will include
                // the zero terminator.
                while (length > 0)
                {
                    if (*(int*)a != *(int*)b) break;
                    a += 2; b += 2; length -= 2;
                }

                if (length == 0)
                {
                    return length;
                }

                if (*a > *b)
                {
                    return 1;
                }

                return -1;
            }
        }

        public unsafe static int Compare(String strA, String strB)
        {
            int length = strA.Length;

            fixed (char* ap = &strA.m_firstChar) fixed (char* bp = &strB.m_firstChar)
            {
                char* a = ap;
                char* b = bp;

                // This depends on the fact that the String objects are
                // always zero terminated and that the terminating zero is not included
                // in the length. For odd string sizes, the last compare will include
                // the zero terminator.
                while (length > 0)
                {
                    if (*(int*)a != *(int*)b) break;
                    a += 2; b += 2; length -= 2;
                }

                if (length == 0)
                {
                    return length;
                }

                if (*a > *b)
                {
                    return 1;
                }

                return -1;
            }
        }

        public bool Equals(String value, StringComparison comparisonType)
        {
            if (comparisonType < StringComparison.CurrentCulture || comparisonType > StringComparison.OrdinalIgnoreCase)
                throw new ArgumentException(Environment.GetResourceString("NotSupported_StringComparison"), "comparisonType");

            if ((Object)this == (Object)value)
            {
                return true;
            }

            if ((Object)value == null)
            {
                return false;
            }

            switch (comparisonType)
            {
                case StringComparison.CurrentCulture:
                case StringComparison.CurrentCultureIgnoreCase:
                case StringComparison.InvariantCulture:
                case StringComparison.InvariantCultureIgnoreCase:
                    throw new NotImplementedException();

                case StringComparison.Ordinal:
                    if (this.Length != value.Length)
                        return false;
                    return EqualsHelper(this, value);

                case StringComparison.OrdinalIgnoreCase:
                    if (this.Length != value.Length)
                        return false;

                    return (CompareOrdinalIgnoreCaseHelper(this, value) == 0);

                default:
                    throw new ArgumentException(Environment.GetResourceString("NotSupported_StringComparison"), "comparisonType");
            }
        }

        private unsafe static int CompareOrdinalIgnoreCaseHelper(String strA, String strB)
        {
            int length = Math.Min(strA.Length, strB.Length);

            fixed (char* ap = strA) fixed (char* bp = strB)
            {
                char* a = ap;
                char* b = bp;

                while (length != 0)
                {
                    int charA = *a;
                    int charB = *b;

                    // uppercase both chars - notice that we need just one compare per char
                    if ((uint)(charA - 'a') <= (uint)('z' - 'a')) charA -= 0x20;
                    if ((uint)(charB - 'a') <= (uint)('z' - 'a')) charB -= 0x20;

                    //Return the (case-insensitive) difference between them.
                    if (charA != charB)
                        return charA - charB;

                    // Next char
                    a++; b++;
                    length--;
                }

                return strA.Length - strB.Length;
            }
        }

        public int CompareTo(Object value)
        {
            throw new NotImplementedException();
        }

        public int CompareTo(String strB)
        {
            throw new NotImplementedException();
        }

        public int IndexOf(char value)
        {
            unsafe
            {
                fixed (char* ap = this)
                {
                    char* p = ap;

                    for (var index = 0; index < m_stringLength; index++)
                    {
                        if (*p++ == value)
                        {
                            return index;
                        }
                    }
                }
            }

            return -1;
        }

        public int IndexOf(char value, int startIndex)
        {
            unsafe
            {
                fixed (char* ap = this)
                {
                    char* p = ap;
                    p += startIndex;

                    for (var index = 0; index < m_stringLength; index++)
                    {
                        if (*p++ == value)
                        {
                            return index;
                        }
                    }
                }
            }

            return -1;
        }

        public int IndexOf(char value, int startIndex, int count)
        {
            unsafe
            {
                fixed (char* ap = this)
                {
                    char* p = ap;
                    p += startIndex;

                    for (var index = 0; index < count; index++)
                    {
                        if (*p++ == value)
                        {
                            return index;
                        }
                    }
                }
            }

            return -1;
        }

        public extern int IndexOfAny(char[] anyOf);


        public extern int IndexOfAny(char[] anyOf, int startIndex);


        public extern int IndexOfAny(char[] anyOf, int startIndex, int count);

        public int IndexOf(String value)
        {
            throw new NotImplementedException();
        }

        public static bool IsNullOrEmpty(String value)
        {
            return (value == null || value.Length == 0);
        }

        public String Replace(String oldValue, String newValue)
        {
            if (oldValue == null)
                throw new ArgumentNullException("oldValue");

            StringBuilder sb = new StringBuilder(this, this.Length);
            sb.Replace(oldValue, newValue);
            return sb.ToString();
        }

        public static string Join(string separator, params Object[] values)
        {
            if (values == null)
                throw new ArgumentNullException("values");

            if (values.Length == 0 || values[0] == null)
                return String.Empty;

            if (separator == null)
                separator = String.Empty;

            StringBuilder result = new StringBuilder();

            string value = values[0].ToString();
            if (value != null)
                result.Append(value);

            for (int i = 1; i < values.Length; i++)
            {
                result.Append(separator);
                if (values[i] != null)
                {
                    // handle the case where their ToString() override is broken
                    value = values[i].ToString();
                    if (value != null)
                        result.Append(value);
                }
            }

            return result.ToString();
        }

        public int LastIndexOf(char value)
        {
            throw new NotImplementedException();
        }


        public extern int LastIndexOf(char value, int startIndex);


        public extern int LastIndexOf(char value, int startIndex, int count);


        public extern int LastIndexOfAny(char[] anyOf);


        public extern int LastIndexOfAny(char[] anyOf, int startIndex);


        public extern int LastIndexOfAny(char[] anyOf, int startIndex, int count);

        public int LastIndexOf(String value)
        {
            throw new NotImplementedException();
        }

        public String ToLower()
        {
            throw new NotImplementedException();
        }

        public String ToUpper()
        {
            throw new NotImplementedException();
        }

        public override String ToString()
        {
            return this;
        }


        public String Trim()
        {
            throw new NotImplementedException();
        }

        ////// This method contains the same functionality as StringBuilder Replace. The only difference is that
        ////// a new String has to be allocated since Strings are immutable
        public static String Concat(Object arg0)
        {
            if (arg0 == null)
            {
                return String.Empty;
            }

            return arg0.ToString();
        }

        public static String Concat(Object arg0, Object arg1)
        {
            if (arg0 == null)
            {
                arg0 = String.Empty;
            }

            if (arg1 == null)
            {
                arg1 = String.Empty;
            }

            return Concat(arg0.ToString(), arg1.ToString());
        }

        public static String Concat(Object arg0, Object arg1, Object arg2)
        {
            if (arg0 == null)
            {
                arg0 = String.Empty;
            }

            if (arg1 == null)
            {
                arg1 = String.Empty;
            }

            if (arg2 == null)
            {
                arg2 = String.Empty;
            }

            return Concat(arg0.ToString(), arg1.ToString(), arg2.ToString());
        }

        public static String Concat(params Object[] args)
        {
            if (args == null)
            {
                throw new ArgumentNullException("args");
            }

            int length = args.Length;
            String[] sArgs = new String[length];
            int totalLength = 0;

            for (int i = 0; i < length; i++)
            {
                sArgs[i] = ((args[i] == null) ? (String.Empty) : (args[i].ToString()));
                totalLength += sArgs[i].Length;
            }

            return String.Concat(sArgs);
        }


        public static String Concat(String str0, String str1)
        {
            var sb = new Text.StringBuilder();
            sb.Append(str0);
            sb.Append(str1);
            return sb.ToString();
        }


        public static String Concat(String str0, String str1, String str2)
        {
            var sb = new Text.StringBuilder();
            sb.Append(str0);
            sb.Append(str1);
            sb.Append(str2);
            return sb.ToString();
        }


        public static String Concat(String str0, String str1, String str2, String str3)
        {
            var sb = new Text.StringBuilder();
            sb.Append(str0);
            sb.Append(str1);
            sb.Append(str2);
            sb.Append(str3);
            return sb.ToString();
        }


        public static String Concat(params String[] values)
        {
            var sb = new Text.StringBuilder();
            foreach (var value in values)
            {
                sb.Append(value);
            }

            return sb.ToString();
        }

        public static String Intern(String str)
        {
            // We don't support "interning" of strings. So simply return the string.
            return str;
        }

        public static String IsInterned(String str)
        {
            // We don't support "interning" of strings. So simply return the string.
            return str;
        }

        private String CtorCharArray(char[] value)
        {
            if (value != null && value.Length != 0)
            {
                String result = FastAllocateString(value.Length);

                unsafe
                {
                    fixed (char* dest = result, source = value)
                    {
                        wstrcpy(dest, source, value.Length);
                    }
                }
                return result;
            }
            else
                return String.Empty;
        }

        private String CtorCharArrayStartLength(char[] value, int startIndex, int length)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            if (startIndex < 0)
                throw new ArgumentOutOfRangeException("startIndex");

            if (length < 0)
                throw new ArgumentOutOfRangeException("length");

            if (startIndex > value.Length - length)
                throw new ArgumentOutOfRangeException("startIndex");

            if (length > 0)
            {
                String result = FastAllocateString(length);

                unsafe
                {
                    fixed (char* dest = result, source = value)
                    {
                        wstrcpy(dest, source + startIndex, length);
                    }
                }
                return result;
            }
            else
                return String.Empty;
        }

        private String CtorCharCount(char c, int count)
        {
            if (count > 0)
            {
                String result = FastAllocateString(count);
                if (c != 0)
                {
                    unsafe
                    {
                        fixed (char* dest = result)
                        {
                            char* dmem = dest;
                            while (((uint)dmem & 3) != 0 && count > 0)
                            {
                                *dmem++ = c;
                                count--;
                            }
                            uint cc = (uint)((c << 16) | c);
                            if (count >= 4)
                            {
                                count -= 4;
                                do
                                {
                                    ((uint*)dmem)[0] = cc;
                                    ((uint*)dmem)[1] = cc;
                                    dmem += 4;
                                    count -= 4;
                                } while (count >= 0);
                            }
                            if ((count & 2) != 0)
                            {
                                ((uint*)dmem)[0] = cc;
                                dmem += 2;
                            }
                            if ((count & 1) != 0)
                                dmem[0] = c;
                        }
                    }
                }
                return result;
            }
            else if (count == 0)
                return String.Empty;
            else
                throw new ArgumentOutOfRangeException("count");
        }

        private unsafe String CtorCharPtr(char* ptr)
        {
            if (ptr == null)
                return String.Empty;

            try
            {
                int count = wcslen(ptr);
                if (count == 0)
                    return String.Empty;

                String result = FastAllocateString(count);
                fixed (char* dest = result)
                    wstrcpy(dest, ptr, count);
                return result;
            }
            catch (NullReferenceException)
            {
                throw new ArgumentOutOfRangeException("ptr");
            }
        }

        private unsafe String CtorCharPtrStartLength(char* ptr, int startIndex, int length)
        {
            if (length < 0)
            {
                throw new ArgumentOutOfRangeException("length");
            }

            if (startIndex < 0)
            {
                throw new ArgumentOutOfRangeException("startIndex");
            }

            char* pFrom = ptr + startIndex;
            if (pFrom < ptr)
            {
                // This means that the pointer operation has had an overflow
                throw new ArgumentOutOfRangeException("startIndex");
            }

            if (length == 0)
                return String.Empty;

            String result = FastAllocateString(length);

            try
            {
                fixed (char* dest = result)
                    wstrcpy(dest, pFrom, length);
                return result;
            }
            catch (NullReferenceException)
            {
                throw new ArgumentOutOfRangeException("ptr");
            }
        }

        unsafe static internal String CreateStringFromEncoding(
            byte* bytes, int byteLength, Encoding encoding)
        {
            // Get our string length
            int stringLength = encoding.GetCharCount(bytes, byteLength, null);

            // They gave us an empty string if they needed one
            // 0 bytelength might be possible if there's something in an encoder
            if (stringLength == 0)
                return String.Empty;

            String s = FastAllocateString(stringLength);
            fixed (char* pTempChars = &s.m_firstChar)
            {
                int doubleCheck = encoding.GetChars(bytes, byteLength, pTempChars, stringLength, null);
            }

            return s;
        }

        /// <summary>
        /// Returns a new string of a specified length in which the beginning of the current string is padded with spaces or with a specified character.
        /// </summary>
        /// <param name="original"></param>
        /// <param name="totalWidth">The number of characters in the resulting string, equal to the number of original characters plus any additional padding characters.</param>
        /// <param name="paddingChar">The padding character.</param>
        /// <returns></returns>
        public string PadLeft(int totalWidth, char paddingChar = ' ')
        {
            return PadHelper(this, totalWidth, paddingChar, true);
        }

        /// <summary>
        /// Returns a new string of a specified length in which the end of the current string is padded with spaces or with a specified character.
        /// </summary>
        /// <param name="original"></param>
        /// <param name="totalWidth">The number of characters in the resulting string, equal to the number of original characters plus any additional padding characters.</param>
        /// <param name="paddingChar">The padding character.</param>
        /// <returns></returns>
        public string PadRight(int totalWidth, char paddingChar = ' ')
        {
            return PadHelper(this, totalWidth, paddingChar, false);
        }

        private static string PadHelper(string original, int totalWidth, char paddingChar, bool appendLeft)
        {
            // this seems to be quicker than a simple while (length < totalWidth) pad
            if (original.Length >= totalWidth)
                return original;
            StringBuilder result = new StringBuilder(totalWidth);
            int padCount = totalWidth - original.Length;
            for (int i = 0; i < padCount; i++)
                result.Append(paddingChar);
            if (appendLeft)
                return result + original;
            return original + result;
        }

        unsafe public void CopyTo(int sourceIndex, char[] destination, int destinationIndex, int count)
        {
            if (destination == null)
                throw new ArgumentNullException("destination");
            if (count < 0)
                throw new ArgumentOutOfRangeException("count", "NegativeCount");
            if (sourceIndex < 0)
                throw new ArgumentOutOfRangeException("sourceIndex", "Index");
            if (count > Length - sourceIndex)
                throw new ArgumentOutOfRangeException("sourceIndex", "IndexCount");
            if (destinationIndex > destination.Length - count || destinationIndex < 0)
                throw new ArgumentOutOfRangeException("destinationIndex", "IndexCount");

            // Note: fixed does not like empty arrays
            if (count > 0)
            {
                fixed (char* src = this)
                fixed (char* dest = destination)
                    wstrcpy(dest + destinationIndex, src + sourceIndex, count);
            }
        }

        internal static unsafe void wstrcpy(char* dmem, char* smem, int charCount)
        {
            Buffer.Memcpy((byte*)dmem, (byte*)smem, charCount * 2); // 2 used everywhere instead of sizeof(char)
        }

        private static unsafe int wcslen(char* ptr)
        {
            char* end = ptr;

            // The following code is (somewhat surprisingly!) significantly faster than a naive loop,
            // at least on x86 and the current jit.

            // First make sure our pointer is aligned on a dword boundary
            while (((uint)end & 3) != 0 && *end != 0)
                end++;
            if (*end != 0)
            {
                // The loop condition below works because if "end[0] & end[1]" is non-zero, that means
                // neither operand can have been zero. If is zero, we have to look at the operands individually,
                // but we hope this going to fairly rare.

                // In general, it would be incorrect to access end[1] if we haven't made sure
                // end[0] is non-zero. However, we know the ptr has been aligned by the loop above
                // so end[0] and end[1] must be in the same page, so they're either both accessible, or both not.

                while ((end[0] & end[1]) != 0 || (end[0] != 0 && end[1] != 0))
                {
                    end += 2;
                }
            }
            // finish up with the naive loop
            for (; *end != 0; end++)
                ;

            int count = (int)(end - ptr);

            return count;
        }
    }
}


