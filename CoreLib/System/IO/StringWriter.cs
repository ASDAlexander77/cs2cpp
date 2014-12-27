using System;
using System.Runtime;
using System.Text;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System.IO
{
    // This class implements a text writer that writes to a string buffer and allows
    // the resulting sequence of characters to be presented as a string.
    //
    [Serializable]
    [ComVisible(true)]
    public class StringWriter : TextWriter
    {
        private static volatile UnicodeEncoding m_encoding = null;

        private StringBuilder _sb;
        private bool _isOpen;

        // Constructs a new StringWriter. A new StringBuilder is automatically
        public StringWriter()
            : this(new StringBuilder())
        {
        }

        public StringWriter(StringBuilder sb)
            : base()
        {
            if (sb == null)
                throw new ArgumentNullException("sb", "Buffer");
            _sb = sb;
            _isOpen = true;
        }

        public override void Close()
        {
            Dispose(true);
        }

        protected override void Dispose(bool disposing)
        {
            // Do not destroy _sb, so that we can extract this after we are
            // done writing (similar to MemoryStream's GetBuffer & ToArray methods)
            _isOpen = false;
            base.Dispose(disposing);
        }


        public override Encoding Encoding
        {
            get
            {
                if (m_encoding == null)
                {
                    m_encoding = new UnicodeEncoding(false, false);
                }
                return m_encoding;
            }
        }

        // Returns the underlying StringBuilder. This is either the StringBuilder
        // that was passed to the constructor, or the StringBuilder that was
        // automatically created.
        //
        public virtual StringBuilder GetStringBuilder()
        {
            return _sb;
        }

        // Writes a character to the underlying string buffer.
        //
        public override void Write(char value)
        {
            if (!_isOpen)
                __Error.WriterClosed();
            _sb.Append(value);
        }

        // Writes a range of a character array to the underlying string buffer.
        // This method will write count characters of data into this
        // StringWriter from the buffer character array starting at position
        // index.
        //
        public override void Write(char[] buffer, int index, int count)
        {
            if (buffer == null)
                throw new ArgumentNullException("buffer", "Buffer");
            if (index < 0)
                throw new ArgumentOutOfRangeException("index", "NeedNonNegNum");
            if (count < 0)
                throw new ArgumentOutOfRangeException("count", "NeedNonNegNum");
            if (buffer.Length - index < count)
                throw new ArgumentException("InvalidOffLen");

            if (!_isOpen)
                __Error.WriterClosed();

            _sb.Append(buffer, index, count);
        }

        // Writes a string to the underlying string buffer. If the given string is
        // null, nothing is written.
        //
        public override void Write(String value)
        {
            if (!_isOpen)
                __Error.WriterClosed();
            if (value != null) _sb.Append(value);
        }

        // Returns a string containing the characters written to this TextWriter
        // so far.
        //
        public override String ToString()
        {
            return _sb.ToString();
        }
    }
}
