namespace System.Text
{
    using System;

    [Serializable]
    public sealed class EncoderExceptionFallback : EncoderFallback
    {
        // Construction
        public EncoderExceptionFallback()
        {
        }

        public override EncoderFallbackBuffer CreateFallbackBuffer()
        {
            return new EncoderExceptionFallbackBuffer();
        }

        // Maximum number of characters that this instance of this fallback could return
        public override int MaxCharCount
        {
            get
            {
                return 0;
            }
        }

        public override bool Equals(Object value)
        {
            EncoderExceptionFallback that = value as EncoderExceptionFallback;
            if (that != null)
            {
                return (true);
            }
            return (false);
        }

        public override int GetHashCode()
        {
            return 654;
        }
    }


    public sealed class EncoderExceptionFallbackBuffer : EncoderFallbackBuffer
    {
        public EncoderExceptionFallbackBuffer() { }
        public override bool Fallback(char charUnknown, int index)
        {
            // Fall back our char
            throw new EncoderFallbackException("InvalidCodePageConversionIndex");
        }

        public override bool Fallback(char charUnknownHigh, char charUnknownLow, int index)
        {
            if (!Char.IsHighSurrogate(charUnknownHigh))
            {
                throw new ArgumentOutOfRangeException("Range");
            }
            if (!Char.IsLowSurrogate(charUnknownLow))
            {
                throw new ArgumentOutOfRangeException("Range");
            }

            int iTemp = Char.ConvertToUtf32(charUnknownHigh, charUnknownLow);

            // Fall back our char
            throw new EncoderFallbackException("InvalidCodePageConversionIndex");
        }

        public override char GetNextChar()
        {
            return (char)0;
        }

        public override bool MovePrevious()
        {
            // Exception fallback doesn't have anywhere to back up to.
            return false;
        }

        // Exceptions are always empty
        public override int Remaining
        {
            get
            {
                return 0;
            }
        }
    }

    [Serializable]
    public sealed class EncoderFallbackException : ArgumentException
    {
        char charUnknown;
        char charUnknownHigh;
        char charUnknownLow;
        int index;

        public EncoderFallbackException()
            : base("ArgumentException")
        {
        }

        public EncoderFallbackException(String message)
            : base(message)
        {
        }

        public EncoderFallbackException(String message, Exception innerException)
            : base(message, innerException)
        {
        }

        internal EncoderFallbackException(
            String message, char charUnknown, int index)
            : base(message)
        {
            this.charUnknown = charUnknown;
            this.index = index;
        }

        internal EncoderFallbackException(
            String message, char charUnknownHigh, char charUnknownLow, int index)
            : base(message)
        {
            if (!Char.IsHighSurrogate(charUnknownHigh))
            {
                throw new ArgumentOutOfRangeException("Range");
            }
            if (!Char.IsLowSurrogate(charUnknownLow))
            {
                throw new ArgumentOutOfRangeException("Range");
            }

            this.charUnknownHigh = charUnknownHigh;
            this.charUnknownLow = charUnknownLow;
            this.index = index;
        }

        public char CharUnknown
        {
            get
            {
                return (charUnknown);
            }
        }

        public char CharUnknownHigh
        {
            get
            {
                return (charUnknownHigh);
            }
        }

        public char CharUnknownLow
        {
            get
            {
                return (charUnknownLow);
            }
        }

        public int Index
        {
            get
            {
                return index;
            }
        }

        // Return true if the unknown character is a surrogate pair.
        public bool IsUnknownSurrogate()
        {
            return (this.charUnknownHigh != '\0');
        }
    }
}
