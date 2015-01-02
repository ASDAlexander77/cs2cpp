////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Apache License 2.0 (Apache)
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
namespace System
{
    using System;
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

        private char[] chars;

        public override int GetHashCode()
        {
            int h = 0;
            int len = this.Length;

            if (h == 0 && len > 0)
            {
                for (int i = 0; i < len; i++)
                {
                    h = 31 * h + chars[i];
                }
            }

            return h;
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
            if (a == null && b == null)
            {
                return true;
            }

            if (a == null || b == null)
            {
                return false;
            }

            var charsA = a.chars;
            var charsB = b.chars;

            if (charsA == null && charsB == null)
            {
                return true;
            }

            if (charsA == null || charsB == null)
            {
                return false;
            }

            if (charsA.Length != charsB.Length)
            {
                return false;
            }

            var len = charsA.Length;
            for (var index = 0; index < len; index++)
            {
                var cA = charsA[index];
                var cB = charsB[index];

                if (cA != cB)
                {
                    return false;
                }
            }

            return true;
        }

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
                return this.chars[index];
            }
        }

        public char[] ToCharArray()
        {
            return this.chars;
        }

        public char[] ToCharArray(int startIndex, int length)
        {
            var newChars = new char[length];
            var index = startIndex;
            for (var dst = 0; dst < length; dst++)
            {
                newChars[dst] = this.chars[index++];
            }

            return newChars;
        }

        public int Length
        {
            get
            {
                if (chars == null)
                {
                    return 0;
                }

                return chars.Length;
            }
        }

        public String[] Split(params char[] separator)
        {
            //throw new NotImplementedException();
            // TODO:
            return null;
        }

        public String[] Split(char[] separator, int count)
        {
            //throw new NotImplementedException();
            // TODO:
            return null;
        }


        public String Substring(int startIndex)
        {
            return new String(this.chars, startIndex, this.chars.Length - startIndex);
        }


        public String Substring(int startIndex, int length)
        {
            return new String(this.chars, startIndex, length);
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

        private String TrimHelper(char[] trimChars, int trimType)
        {
            //end will point to the first non-trimmed character on the right
            //start will point to the first non-trimmed character on the Left
            int end = this.Length - 1;
            int start = 0;

            //Trim specified characters.
            if (trimType != TrimTail)
            {
                for (start = 0; start < this.Length; start++)
                {
                    int i = 0;
                    char ch = this.chars[start];
                    for (i = 0; i < trimChars.Length; i++)
                    {
                        if (trimChars[i] == ch) break;
                    }
                    if (i == trimChars.Length)
                    { // the character is not white space
                        break;
                    }
                }
            }

            if (trimType != TrimHead)
            {
                for (end = Length - 1; end >= start; end--)
                {
                    int i = 0;
                    char ch = this.chars[end];
                    for (i = 0; i < trimChars.Length; i++)
                    {
                        if (trimChars[i] == ch) break;
                    }
                    if (i == trimChars.Length)
                    { // the character is not white space
                        break;
                    }
                }
            }

            //Create a new STRINGREF and initialize it from the range determined above.
            int len = end - start + 1;
            if (len == this.Length)
            {
                // Don't allocate a new string is the trimmed string has not changed.
                return this;
            }
            else
            {
                if (len == 0)
                {
                    return String.Empty;
                }
                return new String(this.chars, start, len);
            }
        }

        public String(char[] value, int startIndex, int length)
        {
            this.chars = new char[length];
            Array.Copy(value, startIndex, this.chars, 0, length);
        }

        public String(char[] value)
        {
            this.chars = value;
        }

        public String(char c, int count)
        {
            this.chars = new char[count];
            for (var index = 0; index < count; index++)
            {
                chars[index] = c;
            }
        }

        public static int Compare(String a, String b)
        {
            if (a == null && b == null)
            {
                return 0;
            }

            if (a == null)
            {
                return 1;
            }

            if (b == null)
            {
                return -1;
            }

            var charsA = a.chars;
            var charsB = b.chars;

            if (charsA == null && charsB == null)
            {
                return 0;
            }

            if (charsA == null)
            {
                return 1;
            }

            if (charsB == null)
            {
                return -1;
            }

            var len = Math.Min(charsA.Length, charsB.Length);
            for (var index = 0; index < len; index++)
            {
                var cA = charsA[index];
                var cB = charsB[index];

                if (cA < cB)
                {
                    return 1;
                }

                if (cA > cB)
                {
                    return -1;
                }
            }

            if (charsA.Length < charsB.Length)
            {
                return 1;
            }

            if (charsA.Length > charsB.Length)
            {
                return -1;
            }

            return 0;
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
            for (var index = 0; index < this.chars.Length; index++)
            {
                if (this.chars[index] == value)
                {
                    return index;
                }
            }

            return -1;
        }


        public int IndexOf(char value, int startIndex)
        {
            for (var index = startIndex; index < this.chars.Length; index++)
            {
                if (this.chars[index] == value)
                {
                    return index;
                }
            }

            return -1;
        }

        public int IndexOf(char value, int startIndex, int count)
        {
            for (var index = startIndex; index < Math.Min(count, this.chars.Length); index++)
            {
                if (this.chars[index] == value)
                {
                    return index;
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

        public extern int IndexOf(String value, int startIndex);

        public extern int IndexOf(String value, int startIndex, int count);

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

        public extern int LastIndexOf(String value, int startIndex);


        public extern int LastIndexOf(String value, int startIndex, int count);


        public String ToLower()
        {
            var newChars = new char[this.chars.Length];

            for (var i = 0; i < this.chars.Length; i++)
            {
                var c = this.chars[i];
                if ('A' <= c && c <= 'Z')
                {
                    newChars[i] = (char)(c | 0x20);
                }
                else
                {
                    newChars[i] = c;
                }
            }

            return new string(newChars);
        }

        public String ToUpper()
        {
            var newChars = new char[this.chars.Length];

            for (var i = 0; i < this.chars.Length; i++)
            {
                var c = this.chars[i];
                if ('a' <= c && c <= 'z')
                {
                    newChars[i] = (char)(c & ~0x20);
                }
                else
                {
                    newChars[i] = c;
                }
            }

            return new string(newChars);
        }

        public override String ToString()
        {
            return this;
        }


        public String Trim()
        {
            return this.TrimHelper(this.chars, TrimBoth);
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

        unsafe static internal String CreateStringFromEncoding(
            byte* bytes, int byteLength, Encoding encoding)
        {
            // Get our string length
            int stringLength = encoding.GetCharCount(bytes, byteLength, null);

            // They gave us an empty string if they needed one
            // 0 bytelength might be possible if there's something in an encoder
            if (stringLength == 0)
                return String.Empty;

            String s = new String('\x0', stringLength);
            fixed (char* pTempChars = s)
            {
                int doubleCheck = encoding.GetChars(bytes, byteLength, pTempChars, stringLength, null);
            }

            return s;
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
                fixed (char* src = this.chars)
                fixed (char* dest = destination)
                    wstrcpy(dest + destinationIndex, src + sourceIndex, count);
            }
        }

        internal static unsafe void wstrcpy(char* dmem, char* smem, int charCount)
        {
            Buffer.Memcpy((byte*)dmem, (byte*)smem, charCount * 2); // 2 used everywhere instead of sizeof(char)
        }
    }
}


