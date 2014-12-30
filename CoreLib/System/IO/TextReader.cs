using System;
using System.Text;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using System.Reflection;

namespace System.IO
{
    // This abstract base class represents a reader that can read a sequential
    // stream of characters.  This is not intended for reading bytes -
    // there are methods on the Stream class to read bytes.
    // A subclass must minimally implement the Peek() and Read() methods.
    //
    // This class is intended for character input, not bytes.  
    // There are methods on the Stream class for reading bytes. 
    [Serializable]
    [ComVisible(true)]
    public abstract class TextReader : IDisposable
    {
        public static readonly TextReader Null = new NullTextReader();

        protected TextReader() { }

        // Closes this TextReader and releases any system resources associated with the
        // TextReader. Following a call to Close, any operations on the TextReader
        // may raise exceptions.
        // 
        // This default method is empty, but descendant classes can override the
        // method to provide the appropriate functionality.
        public virtual void Close()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
        }

        // Returns the next available character without actually reading it from
        // the input stream. The current position of the TextReader is not changed by
        // this operation. The returned value is -1 if no further characters are
        // available.
        // 
        // This default method simply returns -1.
        //
        public virtual int Peek()
        {
            return -1;
        }

        // Reads the next character from the input stream. The returned value is
        // -1 if no further characters are available.
        // 
        // This default method simply returns -1.
        //
        public virtual int Read()
        {
            return -1;
        }

        // Reads a block of characters. This method will read up to
        // count characters from this TextReader into the
        // buffer character array starting at position
        // index. Returns the actual number of characters read.
        //
        public virtual int Read(char[] buffer, int index, int count)
        {
            if (buffer == null)
                throw new ArgumentNullException("buffer", "Buffer");
            if (index < 0)
                throw new ArgumentOutOfRangeException("index", "NeedNonNegNum");
            if (count < 0)
                throw new ArgumentOutOfRangeException("count", "NeedNonNegNum");
            if (buffer.Length - index < count)
                throw new ArgumentException("InvalidOffLen");

            int n = 0;
            do
            {
                int ch = Read();
                if (ch == -1) break;
                buffer[index + n++] = (char)ch;
            } while (n < count);
            return n;
        }

        // Reads all characters from the current position to the end of the 
        // TextReader, and returns them as one string.
        public virtual String ReadToEnd()
        {
            char[] chars = new char[4096];
            int len;
            StringBuilder sb = new StringBuilder(4096);
            while ((len = Read(chars, 0, chars.Length)) != 0)
            {
                sb.Append(chars, 0, len);
            }
            return sb.ToString();
        }

        // Blocking version of read.  Returns only when count
        // characters have been read or the end of the file was reached.
        // 
        public virtual int ReadBlock(char[] buffer, int index, int count)
        {
            int i, n = 0;
            do
            {
                n += (i = Read(buffer, index + n, count - n));
            } while (i > 0 && n < count);
            return n;
        }

        // Reads a line. A line is defined as a sequence of characters followed by
        // a carriage return ('\r'), a line feed ('\n'), or a carriage return
        // immediately followed by a line feed. The resulting string does not
        // contain the terminating carriage return and/or line feed. The returned
        // value is null if the end of the input stream has been reached.
        //
        public virtual String ReadLine()
        {
            StringBuilder sb = new StringBuilder();
            while (true)
            {
                int ch = Read();
                if (ch == -1) break;
                if (ch == '\r' || ch == '\n')
                {
                    if (ch == '\r' && Peek() == '\n') Read();
                    return sb.ToString();
                }
                sb.Append((char)ch);
            }
            if (sb.Length > 0) return sb.ToString();
            return null;
        }

        public static TextReader Synchronized(TextReader reader)
        {
            if (reader == null)
                throw new ArgumentNullException("reader");

            if (reader is SyncTextReader)
                return reader;

            return new SyncTextReader(reader);
        }

        [Serializable]
        private sealed class NullTextReader : TextReader
        {
            public NullTextReader() { }

            public override int Read(char[] buffer, int index, int count)
            {
                return 0;
            }

            public override String ReadLine()
            {
                return null;
            }
        }

        [Serializable]
        internal sealed class SyncTextReader : TextReader
        {
            internal TextReader _in;

            internal SyncTextReader(TextReader t)
            {
                _in = t;
            }

            [MethodImplAttribute(MethodImplOptions.Synchronized)]
            public override void Close()
            {
                // So that any overriden Close() gets run
                _in.Close();
            }

            [MethodImplAttribute(MethodImplOptions.Synchronized)]
            protected override void Dispose(bool disposing)
            {
                // Explicitly pick up a potentially methodimpl'ed Dispose
                if (disposing)
                    ((IDisposable)_in).Dispose();
            }

            [MethodImplAttribute(MethodImplOptions.Synchronized)]
            public override int Peek()
            {
                return _in.Peek();
            }

            [MethodImplAttribute(MethodImplOptions.Synchronized)]
            public override int Read()
            {
                return _in.Read();
            }

            [MethodImplAttribute(MethodImplOptions.Synchronized)]
            public override int Read(char[] buffer, int index, int count)
            {
                return _in.Read(buffer, index, count);
            }

            [MethodImplAttribute(MethodImplOptions.Synchronized)]
            public override int ReadBlock(char[] buffer, int index, int count)
            {
                return _in.ReadBlock(buffer, index, count);
            }

            [MethodImplAttribute(MethodImplOptions.Synchronized)]
            public override String ReadLine()
            {
                return _in.ReadLine();
            }

            [MethodImplAttribute(MethodImplOptions.Synchronized)]
            public override String ReadToEnd()
            {
                return _in.ReadToEnd();
            }
        }
    }
}
