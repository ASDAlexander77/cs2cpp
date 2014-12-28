using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

namespace System.IO
{
    internal sealed class __ConsoleStream : Stream
    {
        private const int ERROR_SUCCESS = 0;

        private SafeFileHandle _handle;
        private bool _canRead;
        private bool _canWrite;

        internal __ConsoleStream(SafeFileHandle handle, FileAccess access)
        {
            _handle = handle;
            _canRead = ((access & FileAccess.Read) == FileAccess.Read);
            _canWrite = ((access & FileAccess.Write) == FileAccess.Write);
        }

        public override bool CanRead
        {
            get { return _canRead; }
        }

        public override bool CanWrite
        {
            get { return _canWrite; }
        }

        public override bool CanSeek
        {
            get { return false; }
        }

        public override long Length
        {
            get
            {
                __Error.SeekNotSupported();
                return 0; // compiler appeasement
            }
        }

        public override long Position
        {
            get
            {
                __Error.SeekNotSupported();
                return 0; // compiler appeasement
            }
            set
            {
                __Error.SeekNotSupported();
            }
        }

        protected override void Dispose(bool disposing)
        {
            // We're probably better off not closing the OS handle here.  First,
            // we allow a program to get multiple instances of __ConsoleStreams
            // around the same OS handle, so closing one handle would invalidate
            // them all.  Additionally, we want a second AppDomain to be able to 
            // write to stdout if a second AppDomain quits.
            if (_handle != null)
            {
                _handle = null;
            }
            _canRead = false;
            _canWrite = false;
            base.Dispose(disposing);
        }

        public override void Flush()
        {
            if (_handle == null) __Error.FileNotOpen();
            if (!CanWrite) __Error.WriteNotSupported();
        }

        public override void SetLength(long value)
        {
            __Error.SeekNotSupported();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (buffer == null)
                throw new ArgumentNullException("buffer");
            if (offset < 0 || count < 0)
                throw new ArgumentOutOfRangeException((offset < 0 ? "offset" : "count"), "NeedNonNegNum");
            if (buffer.Length - offset < count)
                throw new ArgumentException("InvalidOffLen");

            if (!_canRead) __Error.ReadNotSupported();

            int bytesRead;
            int errCode = ReadFileNative(_handle, buffer, offset, count, out bytesRead);

            if (ERROR_SUCCESS != errCode)
                __Error.WinIOError(errCode, String.Empty);

            return bytesRead;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            __Error.SeekNotSupported();
            return 0; // compiler appeasement
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            if (buffer == null)
                throw new ArgumentNullException("buffer");
            if (offset < 0 || count < 0)
                throw new ArgumentOutOfRangeException((offset < 0 ? "offset" : "count"), "NeedNonNegNum");
            if (buffer.Length - offset < count)
                throw new ArgumentException("InvalidOffLen");
            if (!_canWrite) __Error.WriteNotSupported();

            int errCode = WriteFileNative(_handle, buffer, offset, count);

            if (ERROR_SUCCESS != errCode)
                __Error.WinIOError(errCode, String.Empty);

            return;
        }

        private static int ReadFileNative(SafeFileHandle hFile, byte[] bytes, int offset, int count, out int bytesRead)
        {
            if (bytes.Length - offset < count)
                throw new IndexOutOfRangeException("IORaceCondition");

            // You can't use the fixed statement on an array of length 0.
            if (bytes.Length == 0)
            {
                bytesRead = 0;
                return ERROR_SUCCESS;
            }

            bool readSuccess;

            bytesRead = hFile.ReadFile(bytes, offset, count);
            readSuccess = (0 != bytesRead);

            if (readSuccess)
                return ERROR_SUCCESS;

            int errorCode = -1;
            return errorCode;
        }

        private static int WriteFileNative(SafeFileHandle hFile, byte[] bytes, int offset, int count)
        {
            // You can't use the fixed statement on an array of length 0.
            if (bytes.Length == 0)
                return ERROR_SUCCESS;

            bool writeSuccess;

            int numBytesWritten = hFile.WriteFile(bytes, offset, count);
            writeSuccess = (0 != numBytesWritten);

            if (writeSuccess)
                return ERROR_SUCCESS;

            int errorCode = -1;

            return errorCode;
        }
    }
}
