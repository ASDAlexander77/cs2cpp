namespace System.IO
{

    using System;
    using System.Text;

    [System.Runtime.InteropServices.ComVisible(true)]
    public class BinaryReader : IDisposable
    {
        private const int MaxCharBytesSize = 128;

        private Stream m_stream;
        private byte[] m_buffer;
        private Decoder m_decoder;
        private byte[] m_charBytes;
        private char[] m_singleChar;
        private char[] m_charBuffer;
        private int m_maxCharsSize;  // From MaxCharBytesSize & Encoding

        // Performance optimization for Read() w/ Unicode.  Speeds us up by ~40% 
        private bool m_2BytesPerChar;
        private bool m_isMemoryStream; // "do we sit on MemoryStream?" for Read/ReadInt32 perf
        private bool m_leaveOpen;

        public BinaryReader(Stream input)
            : this(input, new UTF8Encoding(), false)
        {
        }

        public BinaryReader(Stream input, Encoding encoding)
            : this(input, encoding, false)
        {
        }

        public BinaryReader(Stream input, Encoding encoding, bool leaveOpen)
        {
            if (input == null)
            {
                throw new ArgumentNullException("input");
            }
            if (encoding == null)
            {
                throw new ArgumentNullException("encoding");
            }
            if (!input.CanRead)
                throw new ArgumentException("StreamNotReadable");
            m_stream = input;
            m_decoder = encoding.GetDecoder();
            m_maxCharsSize = encoding.GetMaxCharCount(MaxCharBytesSize);
            int minBufferSize = encoding.GetMaxByteCount(1);  // max bytes per one char
            if (minBufferSize < 16)
                minBufferSize = 16;
            m_buffer = new byte[minBufferSize];
            // m_charBuffer and m_charBytes will be left null.

            // For Encodings that always use 2 bytes per char (or more), 
            // special case them here to make Read() & Peek() faster.
            m_2BytesPerChar = encoding is UnicodeEncoding;
            // check if BinaryReader is based on MemoryStream, and keep this for it's life
            // we cannot use "as" operator, since derived classes are not allowed
            m_isMemoryStream = (m_stream.GetType() == typeof(MemoryStream));
            m_leaveOpen = leaveOpen;
        }

        public virtual Stream BaseStream
        {
            get
            {
                return m_stream;
            }
        }

        public virtual void Close()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Stream copyOfStream = m_stream;
                m_stream = null;
                if (copyOfStream != null && !m_leaveOpen)
                    copyOfStream.Close();
            }
            m_stream = null;
            m_buffer = null;
            m_decoder = null;
            m_charBytes = null;
            m_singleChar = null;
            m_charBuffer = null;
        }

        public void Dispose()
        {
            Dispose(true);
        }

        public virtual int PeekChar()
        {
            if (m_stream == null) __Error.FileNotOpen();

            if (!m_stream.CanSeek)
                return -1;
            long origPos = m_stream.Position;
            int ch = Read();
            m_stream.Position = origPos;
            return ch;
        }

        public virtual int Read()
        {
            if (m_stream == null)
            {
                __Error.FileNotOpen();
            }

            return InternalReadOneChar();
        }

        public virtual bool ReadBoolean()
        {
            FillBuffer(1);
            return (m_buffer[0] != 0);
        }

        public virtual byte ReadByte()
        {
            // Inlined to avoid some method call overhead with FillBuffer.
            if (m_stream == null) __Error.FileNotOpen();

            int b = m_stream.ReadByte();
            if (b == -1)
                __Error.EndOfFile();
            return (byte)b;
        }

        [CLSCompliant(false)]
        public virtual sbyte ReadSByte()
        {
            FillBuffer(1);
            return (sbyte)(m_buffer[0]);
        }

        public virtual char ReadChar()
        {
            int value = Read();
            if (value == -1)
            {
                __Error.EndOfFile();
            }
            return (char)value;
        }

        public virtual short ReadInt16()
        {
            FillBuffer(2);
            return (short)(m_buffer[0] | m_buffer[1] << 8);
        }

        [CLSCompliant(false)]
        public virtual ushort ReadUInt16()
        {
            FillBuffer(2);
            return (ushort)(m_buffer[0] | m_buffer[1] << 8);
        }

        public virtual int ReadInt32()
        {
            if (m_isMemoryStream)
            {
                if (m_stream == null) __Error.FileNotOpen();
                // read directly from MemoryStream buffer
                MemoryStream mStream = m_stream as MemoryStream;

                return mStream.InternalReadInt32();
            }
            else
            {
                FillBuffer(4);
                return (int)(m_buffer[0] | m_buffer[1] << 8 | m_buffer[2] << 16 | m_buffer[3] << 24);
            }
        }

        [CLSCompliant(false)]
        public virtual uint ReadUInt32()
        {
            FillBuffer(4);
            return (uint)(m_buffer[0] | m_buffer[1] << 8 | m_buffer[2] << 16 | m_buffer[3] << 24);
        }

        public virtual long ReadInt64()
        {
            FillBuffer(8);
            uint lo = (uint)(m_buffer[0] | m_buffer[1] << 8 |
                             m_buffer[2] << 16 | m_buffer[3] << 24);
            uint hi = (uint)(m_buffer[4] | m_buffer[5] << 8 |
                             m_buffer[6] << 16 | m_buffer[7] << 24);
            return (long)((ulong)hi) << 32 | lo;
        }

        [CLSCompliant(false)]
        public virtual ulong ReadUInt64()
        {
            FillBuffer(8);
            uint lo = (uint)(m_buffer[0] | m_buffer[1] << 8 |
                             m_buffer[2] << 16 | m_buffer[3] << 24);
            uint hi = (uint)(m_buffer[4] | m_buffer[5] << 8 |
                             m_buffer[6] << 16 | m_buffer[7] << 24);
            return ((ulong)hi) << 32 | lo;
        }

        public virtual unsafe float ReadSingle()
        {
            FillBuffer(4);
            uint tmpBuffer = (uint)(m_buffer[0] | m_buffer[1] << 8 | m_buffer[2] << 16 | m_buffer[3] << 24);
            return *((float*)&tmpBuffer);
        }

        public virtual unsafe double ReadDouble()
        {
            FillBuffer(8);
            uint lo = (uint)(m_buffer[0] | m_buffer[1] << 8 |
                m_buffer[2] << 16 | m_buffer[3] << 24);
            uint hi = (uint)(m_buffer[4] | m_buffer[5] << 8 |
                m_buffer[6] << 16 | m_buffer[7] << 24);

            ulong tmpBuffer = ((ulong)hi) << 32 | lo;
            return *((double*)&tmpBuffer);
        }

        public virtual String ReadString()
        {
            if (m_stream == null)
                __Error.FileNotOpen();

            int currPos = 0;
            int n;
            int stringLength;
            int readLength;
            int charsRead;

            // Length of the string in bytes, not chars
            stringLength = Read7BitEncodedInt();
            if (stringLength < 0)
            {
                throw new IOException("InvalidStringLen_Len");
            }

            if (stringLength == 0)
            {
                return String.Empty;
            }

            if (m_charBytes == null)
            {
                m_charBytes = new byte[MaxCharBytesSize];
            }

            if (m_charBuffer == null)
            {
                m_charBuffer = new char[m_maxCharsSize];
            }

            StringBuilder sb = null;
            do
            {
                readLength = ((stringLength - currPos) > MaxCharBytesSize) ? MaxCharBytesSize : (stringLength - currPos);

                n = m_stream.Read(m_charBytes, 0, readLength);
                if (n == 0)
                {
                    __Error.EndOfFile();
                }

                charsRead = m_decoder.GetChars(m_charBytes, 0, n, m_charBuffer, 0);

                if (currPos == 0 && n == stringLength)
                    return new String(m_charBuffer, 0, charsRead);

                if (sb == null)
                    sb = new StringBuilder(stringLength); // Actual string length in chars may be smaller.
                sb.Append(m_charBuffer, 0, charsRead);
                currPos += n;

            } while (currPos < stringLength);

            return sb.ToString();
        }

        public virtual int Read(char[] buffer, int index, int count)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException("buffer", "Buffer");
            }
            if (index < 0)
            {
                throw new ArgumentOutOfRangeException("index", "NeedNonNegNum");
            }
            if (count < 0)
            {
                throw new ArgumentOutOfRangeException("count", "NeedNonNegNum");
            }
            if (buffer.Length - index < count)
            {
                throw new ArgumentException("InvalidOffLen");
            }

            if (m_stream == null)
                __Error.FileNotOpen();

            // SafeCritical: index and count have already been verified to be a valid range for the buffer
            return InternalReadChars(buffer, index, count);
        }

        private int InternalReadChars(char[] buffer, int index, int count)
        {
            int numBytes = 0;
            int charsRemaining = count;

            if (m_charBytes == null)
            {
                m_charBytes = new byte[MaxCharBytesSize];
            }

            while (charsRemaining > 0)
            {
                int charsRead = 0;
                // We really want to know what the minimum number of bytes per char
                // is for our encoding.  Otherwise for UnicodeEncoding we'd have to
                // do ~1+log(n) reads to read n characters.
                numBytes = charsRemaining;

                // special case for DecoderNLS subclasses when there is a hanging byte from the previous loop
                DecoderNLS decoder = m_decoder as DecoderNLS;
                if (decoder != null && decoder.HasState && numBytes > 1)
                {
                    numBytes -= 1;
                }

                if (m_2BytesPerChar)
                    numBytes <<= 1;
                if (numBytes > MaxCharBytesSize)
                    numBytes = MaxCharBytesSize;

                int position = 0;
                byte[] byteBuffer = null;
                if (m_isMemoryStream)
                {
                    MemoryStream mStream = m_stream as MemoryStream;

                    position = mStream.InternalGetPosition();
                    numBytes = mStream.InternalEmulateRead(numBytes);
                    byteBuffer = mStream.InternalGetBuffer();
                }
                else
                {
                    numBytes = m_stream.Read(m_charBytes, 0, numBytes);
                    byteBuffer = m_charBytes;
                }

                if (numBytes == 0)
                {
                    return (count - charsRemaining);
                }

                unsafe
                {
                    fixed (byte* pBytes = byteBuffer)
                    fixed (char* pChars = buffer)
                    {
                        charsRead = m_decoder.GetChars(pBytes + position, numBytes, pChars + index, charsRemaining, false);
                    }
                }

                charsRemaining -= charsRead;
                index += charsRead;
            }

            // we may have read fewer than the number of characters requested if end of stream reached 
            // or if the encoding makes the char count too big for the buffer (e.g. fallback sequence)
            return (count - charsRemaining);
        }

        private int InternalReadOneChar()
        {
            // I know having a separate InternalReadOneChar method seems a little 
            // redundant, but this makes a scenario like the security parser code
            // 20% faster, in addition to the optimizations for UnicodeEncoding I
            // put in InternalReadChars.   
            int charsRead = 0;
            int numBytes = 0;
            long posSav = posSav = 0;

            if (m_stream.CanSeek)
                posSav = m_stream.Position;

            if (m_charBytes == null)
            {
                m_charBytes = new byte[MaxCharBytesSize]; //
            }
            if (m_singleChar == null)
            {
                m_singleChar = new char[1];
            }

            while (charsRead == 0)
            {
                // We really want to know what the minimum number of bytes per char
                // is for our encoding.  Otherwise for UnicodeEncoding we'd have to
                // do ~1+log(n) reads to read n characters.
                // Assume 1 byte can be 1 char unless m_2BytesPerChar is true.
                numBytes = m_2BytesPerChar ? 2 : 1;

                int r = m_stream.ReadByte();
                m_charBytes[0] = (byte)r;
                if (r == -1)
                    numBytes = 0;
                if (numBytes == 2)
                {
                    r = m_stream.ReadByte();
                    m_charBytes[1] = (byte)r;
                    if (r == -1)
                        numBytes = 1;
                }

                if (numBytes == 0)
                {
                    // Console.WriteLine("Found no bytes.  We're outta here.");
                    return -1;
                }

                try
                {

                    charsRead = m_decoder.GetChars(m_charBytes, 0, numBytes, m_singleChar, 0);
                }
                catch
                {
                    // Handle surrogate char 

                    if (m_stream.CanSeek)
                        m_stream.Seek((posSav - m_stream.Position), SeekOrigin.Current);
                    // else - we can't do much here

                    throw;
                }
            }
            if (charsRead == 0)
                return -1;
            return m_singleChar[0];
        }

        public virtual char[] ReadChars(int count)
        {
            if (count < 0)
            {
                throw new ArgumentOutOfRangeException("count", "NeedNonNegNum");
            }

            if (m_stream == null)
            {
                __Error.FileNotOpen();
            }

            if (count == 0)
            {
                return EmptyArray<Char>.Value;
            }

            // SafeCritical: we own the chars buffer, and therefore can guarantee that the index and count are valid
            char[] chars = new char[count];
            int n = InternalReadChars(chars, 0, count);
            if (n != count)
            {
                char[] copy = new char[n];
                Buffer.InternalBlockCopy(chars, 0, copy, 0, 2 * n); // sizeof(char)
                chars = copy;
            }

            return chars;
        }

        public virtual int Read(byte[] buffer, int index, int count)
        {
            if (buffer == null)
                throw new ArgumentNullException("buffer", "Buffer");
            if (index < 0)
                throw new ArgumentOutOfRangeException("index", "NeedNonNegNum");
            if (count < 0)
                throw new ArgumentOutOfRangeException("count", "NeedNonNegNum");
            if (buffer.Length - index < count)
                throw new ArgumentException("InvalidOffLen");

            if (m_stream == null) __Error.FileNotOpen();
            return m_stream.Read(buffer, index, count);
        }

        public virtual byte[] ReadBytes(int count)
        {
            if (count < 0) throw new ArgumentOutOfRangeException("count", "NeedNonNegNum");

            if (m_stream == null) __Error.FileNotOpen();

            if (count == 0)
            {
                return EmptyArray<Byte>.Value;
            }

            byte[] result = new byte[count];

            int numRead = 0;
            do
            {
                int n = m_stream.Read(result, numRead, count);
                if (n == 0)
                    break;
                numRead += n;
                count -= n;
            } while (count > 0);

            if (numRead != result.Length)
            {
                // Trim array.  This should happen on EOF & possibly net streams.
                byte[] copy = new byte[numRead];
                Buffer.InternalBlockCopy(result, 0, copy, 0, numRead);
                result = copy;
            }

            return result;
        }

        protected virtual void FillBuffer(int numBytes)
        {
            if (m_buffer != null && (numBytes < 0 || numBytes > m_buffer.Length))
            {
                throw new ArgumentOutOfRangeException("numBytes", "BinaryReaderFillBuffer");
            }
            int bytesRead = 0;
            int n = 0;

            if (m_stream == null) __Error.FileNotOpen();

            // Need to find a good threshold for calling ReadByte() repeatedly
            // vs. calling Read(byte[], int, int) for both buffered & unbuffered
            // streams.
            if (numBytes == 1)
            {
                n = m_stream.ReadByte();
                if (n == -1)
                    __Error.EndOfFile();
                m_buffer[0] = (byte)n;
                return;
            }

            do
            {
                n = m_stream.Read(m_buffer, bytesRead, numBytes - bytesRead);
                if (n == 0)
                {
                    __Error.EndOfFile();
                }
                bytesRead += n;
            } while (bytesRead < numBytes);
        }

        internal protected int Read7BitEncodedInt()
        {
            // Read out an Int32 7 bits at a time.  The high bit
            // of the byte when on means to continue reading more bytes.
            int count = 0;
            int shift = 0;
            byte b;
            do
            {
                // Check for a corrupted stream.  Read a max of 5 bytes.
                // In a future version, add a DataFormatException.
                if (shift == 5 * 7)  // 5 bytes max per Int32, shift += 7
                    throw new FormatException("Bad7BitInt32");

                // ReadByte handles end of stream cases for us.
                b = ReadByte();
                count |= (b & 0x7F) << shift;
                shift += 7;
            } while ((b & 0x80) != 0);
            return count;
        }
    }
}
