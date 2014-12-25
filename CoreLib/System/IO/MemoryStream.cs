using System.Runtime.InteropServices;

namespace System.IO
{
    // A MemoryStream represents a Stream in memory (ie, it has no backing store).
    // This stream may reduce the need for temporary buffers and files in 
    // an application.  
    // 
    // There are two ways to create a MemoryStream.  You can initialize one
    // from an unsigned byte array, or you can create an empty one.  Empty 
    // memory streams are resizable, while ones created with a byte array provide
    // a stream "view" of the data.
    [Serializable]
    [ComVisible(true)]
    public class MemoryStream : Stream
    {
        private byte[] _buffer;    // Either allocated internally or externally.
        private int _origin;       // For user-provided arrays, start at this origin
        private int _position;     // read/write head.
        private int _length;       // Number of bytes within the memory stream
        private int _capacity;     // length of usable portion of buffer for stream
        // Note that _capacity == _buffer.Length for non-user-provided byte[]'s

        private bool _expandable;  // User-provided buffers aren't expandable.
        private bool _writable;    // Can user write to this stream?
        private bool _exposable;   // Whether the array can be returned to the user.
        private bool _isOpen;      // Is this stream open or closed?

        private const int MemStreamMaxLength = Int32.MaxValue;

        public MemoryStream()
            : this(0)
        {
        }

        public MemoryStream(int capacity)
        {
            if (capacity < 0)
            {
                throw new ArgumentOutOfRangeException("capacity", "NegativeCapacity");
            }

            _buffer = new byte[capacity];
            _capacity = capacity;
            _expandable = true;
            _writable = true;
            _exposable = true;
            _origin = 0;      // Must be 0 for byte[]'s created by MemoryStream
            _isOpen = true;
        }

        public MemoryStream(byte[] buffer)
            : this(buffer, true)
        {
        }

        public MemoryStream(byte[] buffer, bool writable)
        {
            if (buffer == null) throw new ArgumentNullException("buffer", "Buffer");
            _buffer = buffer;
            _length = _capacity = buffer.Length;
            _writable = writable;
            _exposable = false;
            _origin = 0;
            _isOpen = true;
        }

        public MemoryStream(byte[] buffer, int index, int count)
            : this(buffer, index, count, true, false)
        {
        }

        public MemoryStream(byte[] buffer, int index, int count, bool writable)
            : this(buffer, index, count, writable, false)
        {
        }

        public MemoryStream(byte[] buffer, int index, int count, bool writable, bool publiclyVisible)
        {
            if (buffer == null)
                throw new ArgumentNullException("buffer", "Buffer");
            if (index < 0)
                throw new ArgumentOutOfRangeException("index", "NeedNonNegNum");
            if (count < 0)
                throw new ArgumentOutOfRangeException("count", "NeedNonNegNum");
            if (buffer.Length - index < count)
                throw new ArgumentException("InvalidOffLen");

            _buffer = buffer;
            _origin = _position = index;
            _length = _capacity = index + count;
            _writable = writable;
            _exposable = publiclyVisible;  // Can GetBuffer return the array?
            _expandable = false;
            _isOpen = true;
        }

        public override bool CanRead
        {
            get { return _isOpen; }
        }

        public override bool CanSeek
        {
            get { return _isOpen; }
        }

        public override bool CanWrite
        {
            get { return _writable; }
        }

        private void EnsureWriteable()
        {
            if (!CanWrite) __Error.WriteNotSupported();
        }

        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    _isOpen = false;
                    _writable = false;
                    _expandable = false;
                }
            }
            finally
            {
                // Call base.Close() to cleanup async IO resources
                base.Dispose(disposing);
            }
        }

        // returns a bool saying whether we allocated a new array.
        private bool EnsureCapacity(int value)
        {
            // Check for overflow
            if (value < 0)
                throw new IOException("StreamTooLong");
            if (value > _capacity)
            {
                int newCapacity = value;
                if (newCapacity < 256)
                    newCapacity = 256;
                if (newCapacity < _capacity * 2)
                    newCapacity = _capacity * 2;
                Capacity = newCapacity;
                return true;
            }
            return false;
        }

        public override void Flush()
        {
        }

        public virtual byte[] GetBuffer()
        {
            if (!_exposable)
                throw new UnauthorizedAccessException("MemStreamBuffer");
            return _buffer;
        }

        internal byte[] InternalGetBuffer()
        {
            return _buffer;
        }

        internal void InternalGetOriginAndLength(out int origin, out int length)
        {
            if (!_isOpen) __Error.StreamIsClosed();
            origin = _origin;
            length = _length;
        }

        internal int InternalGetPosition()
        {
            if (!_isOpen) __Error.StreamIsClosed();
            return _position;
        }

        // PERF: Takes out Int32 as fast as possible
        internal int InternalReadInt32()
        {
            if (!_isOpen)
                __Error.StreamIsClosed();

            int pos = (_position += 4); // use temp to avoid ----
            if (pos > _length)
            {
                _position = _length;
                __Error.EndOfFile();
            }
            return (int)(_buffer[pos - 4] | _buffer[pos - 3] << 8 | _buffer[pos - 2] << 16 | _buffer[pos - 1] << 24);
        }

        // PERF: Get actual length of bytes available for read; do sanity checks; shift position - i.e. everything except actual copying bytes
        internal int InternalEmulateRead(int count)
        {
            if (!_isOpen) __Error.StreamIsClosed();

            int n = _length - _position;
            if (n > count) n = count;
            if (n < 0) n = 0;

            _position += n;
            return n;
        }

        // Gets & sets the capacity (number of bytes allocated) for this stream.
        // The capacity cannot be set to a value less than the current length
        // of the stream.
        // 
        public virtual int Capacity
        {
            get
            {
                if (!_isOpen) __Error.StreamIsClosed();
                return _capacity - _origin;
            }
            set
            {
                // Only update the capacity if the MS is expandable and the value is different than the current capacity.
                // Special behavior if the MS isn't expandable: we don't throw if value is the same as the current capacity
                if (value < Length) throw new ArgumentOutOfRangeException("value", "SmallCapacity");

                if (!_isOpen) __Error.StreamIsClosed();
                if (!_expandable && (value != Capacity)) __Error.MemoryStreamNotExpandable();

                // MemoryStream has this invariant: _origin > 0 => !expandable (see ctors)
                if (_expandable && value != _capacity)
                {
                    if (value > 0)
                    {
                        byte[] newBuffer = new byte[value];
                        if (_length > 0) Buffer.InternalBlockCopy(_buffer, 0, newBuffer, 0, _length);
                        _buffer = newBuffer;
                    }
                    else
                    {
                        _buffer = null;
                    }
                    _capacity = value;
                }
            }
        }

        public override long Length
        {
            get
            {
                if (!_isOpen) __Error.StreamIsClosed();
                return _length - _origin;
            }
        }

        public override long Position
        {
            get
            {
                if (!_isOpen) __Error.StreamIsClosed();
                return _position - _origin;
            }
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException("value", "NeedNonNegNum");

                if (!_isOpen) __Error.StreamIsClosed();

                if (value > MemStreamMaxLength)
                    throw new ArgumentOutOfRangeException("value", "StreamLength");
                _position = _origin + (int)value;
            }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (buffer == null)
                throw new ArgumentNullException("buffer", "Buffer");
            if (offset < 0)
                throw new ArgumentOutOfRangeException("offset", "NeedNonNegNum");
            if (count < 0)
                throw new ArgumentOutOfRangeException("count", "NeedNonNegNum");
            if (buffer.Length - offset < count)
                throw new ArgumentException("InvalidOffLen");

            if (!_isOpen) __Error.StreamIsClosed();

            int n = _length - _position;
            if (n > count) n = count;
            if (n <= 0)
                return 0;

            if (n <= 8)
            {
                int byteCount = n;
                while (--byteCount >= 0)
                    buffer[offset + byteCount] = _buffer[_position + byteCount];
            }
            else
                Buffer.InternalBlockCopy(_buffer, _position, buffer, offset, n);
            _position += n;

            return n;
        }

        public override int ReadByte()
        {
            if (!_isOpen) __Error.StreamIsClosed();

            if (_position >= _length) return -1;

            return _buffer[_position++];
        }

        public override long Seek(long offset, SeekOrigin loc)
        {
            if (!_isOpen) __Error.StreamIsClosed();

            if (offset > MemStreamMaxLength)
                throw new ArgumentOutOfRangeException("offset", "StreamLength");
            switch (loc)
            {
                case SeekOrigin.Begin:
                    {
                        int tempPosition = unchecked(_origin + (int)offset);
                        if (offset < 0 || tempPosition < _origin)
                            throw new IOException("SeekBeforeBegin");
                        _position = tempPosition;
                        break;
                    }
                case SeekOrigin.Current:
                    {
                        int tempPosition = unchecked(_position + (int)offset);
                        if (unchecked(_position + offset) < _origin || tempPosition < _origin)
                            throw new IOException("SeekBeforeBegin");
                        _position = tempPosition;
                        break;
                    }
                case SeekOrigin.End:
                    {
                        int tempPosition = unchecked(_length + (int)offset);
                        if (unchecked(_length + offset) < _origin || tempPosition < _origin)
                            throw new IOException("SeekBeforeBegin");
                        _position = tempPosition;
                        break;
                    }
                default:
                    throw new ArgumentException("InvalidSeekOrigin");
            }

            return _position;
        }

        // Sets the length of the stream to a given value.  The new
        // value must be nonnegative and less than the space remaining in
        // the array, Int32.MaxValue - origin
        // Origin is 0 in all cases other than a MemoryStream created on
        // top of an existing array and a specific starting offset was passed 
        // into the MemoryStream constructor.  The upper bounds prevents any 
        // situations where a stream may be created on top of an array then 
        // the stream is made longer than the maximum possible length of the 
        // array (Int32.MaxValue).
        // 
        public override void SetLength(long value)
        {
            if (value < 0 || value > Int32.MaxValue)
            {
                throw new ArgumentOutOfRangeException("value", "StreamLength");
            }

            EnsureWriteable();

            if (value > (Int32.MaxValue - _origin))
            {
                throw new ArgumentOutOfRangeException("value", "StreamLength");
            }

            int newLength = _origin + (int)value;
            bool allocatedNewArray = EnsureCapacity(newLength);
            if (!allocatedNewArray && newLength > _length)
                Array.Clear(_buffer, _length, newLength - _length);
            _length = newLength;
            if (_position > newLength) _position = newLength;

        }

        public virtual byte[] ToArray()
        {
            byte[] copy = new byte[_length - _origin];
            Buffer.InternalBlockCopy(_buffer, _origin, copy, 0, _length - _origin);
            return copy;
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            if (buffer == null)
                throw new ArgumentNullException("buffer", "Buffer");
            if (offset < 0)
                throw new ArgumentOutOfRangeException("offset", "NeedNonNegNum");
            if (count < 0)
                throw new ArgumentOutOfRangeException("count", "NeedNonNegNum");
            if (buffer.Length - offset < count)
                throw new ArgumentException("InvalidOffLen");

            if (!_isOpen) __Error.StreamIsClosed();
            EnsureWriteable();

            int i = _position + count;
            // Check for overflow
            if (i < 0)
                throw new IOException("StreamTooLong");

            if (i > _length)
            {
                bool mustZero = _position > _length;
                if (i > _capacity)
                {
                    bool allocatedNewArray = EnsureCapacity(i);
                    if (allocatedNewArray)
                        mustZero = false;
                }
                if (mustZero)
                    Array.Clear(_buffer, _length, i - _length);
                _length = i;
            }
            if ((count <= 8) && (buffer != _buffer))
            {
                int byteCount = count;
                while (--byteCount >= 0)
                    _buffer[_position + byteCount] = buffer[offset + byteCount];
            }
            else
                Buffer.InternalBlockCopy(buffer, offset, _buffer, _position, count);
            _position = i;

        }

        public override void WriteByte(byte value)
        {
            if (!_isOpen) __Error.StreamIsClosed();
            EnsureWriteable();

            if (_position >= _length)
            {
                int newLength = _position + 1;
                bool mustZero = _position > _length;
                if (newLength >= _capacity)
                {
                    bool allocatedNewArray = EnsureCapacity(newLength);
                    if (allocatedNewArray)
                        mustZero = false;
                }
                if (mustZero)
                    Array.Clear(_buffer, _length, _position - _length);
                _length = newLength;
            }
            _buffer[_position++] = value;

        }

        // Writes this MemoryStream to another stream.
        public virtual void WriteTo(Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException("stream", "Stream");

            if (!_isOpen) __Error.StreamIsClosed();
            stream.Write(_buffer, _origin, _length - _origin);
        }
    }
}
