////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Apache License 2.0 (Apache)
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
namespace System
{

    using System;
    using System.Runtime.CompilerServices;
    using Globalization;

    [Serializable]
    public struct Char
    {
        internal const int UNICODE_PLANE00_END = 0x00ffff;
        // The starting codepoint for Unicode plane 1.  Plane 1 contains 0x010000 ~ 0x01ffff.
        internal const int UNICODE_PLANE01_START = 0x10000;
        // The end codepoint for Unicode plane 16.  This is the maximum code point value allowed for Unicode.
        // Plane 16 contains 0x100000 ~ 0x10ffff.
        internal const int UNICODE_PLANE16_END = 0x10ffff;

        internal const int HIGH_SURROGATE_START = 0x00d800;
        internal const int HIGH_SURROGATE_END = 0x00dbff;
        internal const int LOW_SURROGATE_START = 0x00dc00;
        internal const int LOW_SURROGATE_END = 0x00dfff;

        //
        // Member Variables
        //
        internal char m_value;

        //
        // Public Constants
        //
        /**
         * The maximum character value.
         */
        public const char MaxValue = (char)0xFFFF;
        /**
         * The minimum character value.
         */
        public const char MinValue = (char)0x00;

        public override int GetHashCode()
        {
            return (int)m_value | ((int)m_value << 16);
        }

        public override String ToString()
        {
            return new String(m_value, 1);
        }

        public char ToLower()
        {
            if('A' <= m_value && m_value <= 'Z')
            {
                return (char)(m_value - ('A' - 'a'));
            }

            return m_value;
        }

        public char ToUpper()
        {
            if('a' <= m_value && m_value <= 'z')
            {
                return (char)(m_value + ('A' - 'a'));
            }

            return m_value;
        }

        public static bool IsDigit(char c)
        {
            return (c >= '0' && c <= '9');
        }

        private static bool IsWhiteSpaceLatin1(char c)
        {

            // There are characters which belong to UnicodeCategory.Control but are considered as white spaces.
            // We use code point comparisons for these characters here as a temporary fix.

            // U+0009 = <control> HORIZONTAL TAB
            // U+000a = <control> LINE FEED
            // U+000b = <control> VERTICAL TAB
            // U+000c = <contorl> FORM FEED
            // U+000d = <control> CARRIAGE RETURN
            // U+0085 = <control> NEXT LINE
            // U+00a0 = NO-BREAK SPACE
            if ((c == ' ') || (c >= '\x0009' && c <= '\x000d') || c == '\x00a0' || c == '\x0085')
            {
                return (true);
            }

            return (false);
        }

        // Return true for all characters below or equal U+00ff, which is ASCII + Latin-1 Supplement.
        private static bool IsLatin1(char ch)
        {
            return (ch <= '\x00ff');
        }

        // Return true for all characters below or equal U+007f, which is ASCII.
        private static bool IsAscii(char ch)
        {
            return (ch <= '\x007f');
        }

        public static bool IsWhiteSpace(char c)
        {
            if (IsLatin1(c))
            {
                return (IsWhiteSpaceLatin1(c));
            }

            throw new NotImplementedException();
        }


        public static bool IsSurrogate(char c)
        {
            return (c >= HIGH_SURROGATE_START && c <= LOW_SURROGATE_END);
        }

        public static bool IsSurrogate(String s, int index)
        {
            if (s == null)
            {
                throw new ArgumentNullException("s");
            }
            if (((uint)index) >= ((uint)s.Length))
            {
                throw new ArgumentOutOfRangeException("index");
            }

            return (IsSurrogate(s[index]));
        }

        public static bool IsHighSurrogate(char c)
        {
            return ((c >= HIGH_SURROGATE_START) && (c <= HIGH_SURROGATE_END));
        }

        public static bool IsHighSurrogate(String s, int index)
        {
            if (s == null)
            {
                throw new ArgumentNullException("s");
            }
            if (index < 0 || index >= s.Length)
            {
                throw new ArgumentOutOfRangeException("index");
            }

            return (IsHighSurrogate(s[index]));
        } 

        public static bool IsLowSurrogate(char c)
        {
            return ((c >= LOW_SURROGATE_START) && (c <= LOW_SURROGATE_END));
        }

        public static bool IsLowSurrogate(String s, int index)
        {
            if (s == null)
            {
                throw new ArgumentNullException("s");
            }
            if (index < 0 || index >= s.Length)
            {
                throw new ArgumentOutOfRangeException("index");
            }

            return (IsLowSurrogate(s[index]));
        } 

        public static int ConvertToUtf32(char highSurrogate, char lowSurrogate)
        {
            if (!IsHighSurrogate(highSurrogate))
            {
                throw new ArgumentOutOfRangeException("highSurrogate", "InvalidHighSurrogate");
            }
            if (!IsLowSurrogate(lowSurrogate))
            {
                throw new ArgumentOutOfRangeException("lowSurrogate", "InvalidLowSurrogate");
            }

            return (((highSurrogate - HIGH_SURROGATE_START) * 0x400) + (lowSurrogate - LOW_SURROGATE_START) + UNICODE_PLANE01_START);
        }

        public static bool IsSurrogatePair(String s, int index)
        {
            if (s == null)
            {
                throw new ArgumentNullException("s");
            }
            if (index < 0 || index >= s.Length)
            {
                throw new ArgumentOutOfRangeException("index");
            }
            if (index + 1 < s.Length)
            {
                return (IsSurrogatePair(s[index], s[index + 1]));
            }
            return (false);
        }

        public static bool IsSurrogatePair(char highSurrogate, char lowSurrogate)
        {
            return ((highSurrogate >= HIGH_SURROGATE_START && highSurrogate <= HIGH_SURROGATE_END) &&
                    (lowSurrogate >= LOW_SURROGATE_START && lowSurrogate <= LOW_SURROGATE_END));
        }
    }
}


