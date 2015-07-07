namespace System.IO
{
    internal static class __Error
    {
        internal static void EndOfFile()
        {
            throw new EndOfStreamException("ReadBeyondEOF");
        }

        internal static void FileNotOpen()
        {
            throw new ObjectDisposedException("FileClosed");
        }

        internal static void StreamIsClosed()
        {
            throw new ObjectDisposedException("StreamClosed");
        }

        internal static void MemoryStreamNotExpandable()
        {
            throw new NotSupportedException("MemStreamNotExpandable");
        }

        internal static void ReaderClosed()
        {
            throw new ObjectDisposedException("ReaderClosed");
        }

        internal static void ReadNotSupported()
        {
            throw new NotSupportedException("UnreadableStream");
        }

        internal static void SeekNotSupported()
        {
            throw new NotSupportedException("UnseekableStream");
        }

        internal static void WrongAsyncResult()
        {
            throw new ArgumentException("WrongAsyncResult");
        }

        internal static void EndReadCalledTwice()
        {
            // Should ideally be InvalidOperationExc but we can't maitain parity with Stream and FileStream without some work
            throw new ArgumentException("EndReadCalledMultiple");
        }

        internal static void EndWriteCalledTwice()
        {
            // Should ideally be InvalidOperationExc but we can't maintain parity with Stream and FileStream without some work
            throw new ArgumentException("EndWriteCalledMultiple");
        }

        internal static void WriteNotSupported()
        {
            throw new NotSupportedException("UnwritableStream");
        }

        internal static void WriterClosed()
        {
            throw new ObjectDisposedException("WriterClosed");
        }

        internal static void WinIOError(int errCode, string msg)
        {
            throw new IOException(msg);
        }

        internal static void WinIOError()
        {
            throw new IOException();
        }
    }
}
