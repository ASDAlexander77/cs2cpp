using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Threading;

/*
 * FileStream supports different modes of accessing the disk - async mode
 * and [....] mode.  They are two completely different codepaths in the
 * [....] & async methods (ie, Read/Write vs. BeginRead/BeginWrite).  File
 * handles in NT can be opened in only [....] or overlapped (async) mode,
 * and we have to deal with this pain.  Stream has implementations of
 * the [....] methods in terms of the async ones, so we'll
 * call through to our base class to get those methods when necessary.
 *
 * Also buffering is added into FileStream as well. Folded in the
 * code from BufferedStream, so all the comments about it being mostly
 * aggressive (and the possible perf improvement) apply to FileStream as 
 * well.  Also added some buffering to the async code paths.
 *
 * Class Invariants:
 * The class has one buffer, shared for reading & writing.  It can only be
 * used for one or the other at any point in time - not both.  The following
 * should be true:
 *   0 <= _readPos <= _readLen < _bufferSize
 *   0 <= _writePos < _bufferSize
 *   _readPos == _readLen && _readPos > 0 implies the read buffer is valid, 
 *     but we're at the end of the buffer.
 *   _readPos == _readLen == 0 means the read buffer contains garbage.
 *   Either _writePos can be greater than 0, or _readLen & _readPos can be
 *     greater than zero, but neither can be greater than zero at the same time.
 *
 */

namespace System.IO
{
    // This is an internal object implementing IAsyncResult with fields
    // for all of the relevant data necessary to complete the IO operation.
    // This is used by AsyncFSCallback and all of the async methods.
    // We should probably make this a nested type of FileStream. But 
    // I don't know how to define a nested class in mscorlib.h

    // Ideally we shoult make this type windows only (!FEATURE_PAL). But to make that happen
    // we need to do a lot of untangling in the VM code.
    unsafe internal sealed class FileStreamAsyncResult : IAsyncResult
    {
        // README:
        // If you modify the order of these fields, make sure to update 
        // the native VM definition of this class as well!!! 
        private Object _userStateObject;
        private ManualResetEvent _waitHandle;
        private SafeFileHandle _handle;      // For cancellation support.

        internal int _EndXxxCalled;   // Whether we've called EndXxx already.
        private int _numBytes;     // number of bytes read OR written
        internal int NumBytes { get { return _numBytes; } }

        private int _errorCode;
        internal int ErrorCode { get { return _errorCode; } }

        private int _numBufferedBytes;
        internal int NumBufferedBytes { get { return _numBufferedBytes; } }

        internal int NumBytesRead { get { return _numBytes + _numBufferedBytes; } }

        private bool _isWrite;     // Whether this is a read or a write
        internal bool IsWrite { get { return _isWrite; } }

        private bool _isComplete;  // Value for IsCompleted property        
        private bool _completedSynchronously;  // Which thread called callback

        // The NativeOverlapped struct keeps a GCHandle to this IAsyncResult object.
        // So if the user doesn't call EndRead/EndWrite, a finalizer won't help because
        // it'll never get called. 

        // Overlapped class will take care of the async IO operations in progress 
        // when an appdomain unload occurs.

        public Object AsyncState
        {
            get { return _userStateObject; }
        }

        public bool IsCompleted
        {
            get { return _isComplete; }
        }

        public WaitHandle AsyncWaitHandle
        {
            get
            {
                return null;
            }
        }

        // Returns true iff the user callback was called by the thread that 
        // called BeginRead or BeginWrite.  If we use an async delegate or
        // threadpool thread internally, this will be false.  This is used
        // by code to determine whether a successive call to BeginRead needs 
        // to be done on their main thread or in their callback to avoid a
        // stack overflow on many reads or writes.
        public bool CompletedSynchronously
        {
            get { return _completedSynchronously; }
        }
    }

    [ComVisible(true)]
    public class FileStream : Stream
    {
        internal const int DefaultBufferSize = 4096;


        // @todo: This #ifdef factoring fixes the public api async behavior for M6 with minimal risk. When more
        // time is available, we should more rigorously scrub out all references for FileStreamAsyncResult and AsyncFSCallback
        // (this requires corresponding changes in the src\vm tree as well.)
        private static readonly bool _canUseAsync = false;

        private byte[] _buffer;   // Shared read/write buffer.  Alloc on first use.
        private String _fileName; // Fully qualified file name.
        private bool _isAsync;    // Whether we opened the handle for overlapped IO
        private bool _canRead;
        private bool _canWrite;
        private bool _canSeek;
        private bool _exposedHandle; // Could other code be using this handle?
        private bool _isPipe;     // Whether to disable async buffering code.
        private int _readPos;     // Read pointer within shared buffer.
        private int _readLen;     // Number of bytes read in buffer from file.
        private int _writePos;    // Write pointer within shared buffer.
        private int _bufferSize;  // Length of internal buffer, if it's allocated.
        private SafeFileHandle _handle;
        private long _pos;        // Cache current location in the file.
        private long _appendStart;// When appending, prevent overwriting file.

        //This exists only to support IsolatedStorageFileStream.
        //Any changes to FileStream must include the corresponding changes in IsolatedStorage.
        internal FileStream()
        {
        }

        public FileStream(String path, FileMode mode)
            : this(path, mode, (mode == FileMode.Append ? FileAccess.Write : FileAccess.ReadWrite), FileShare.Read, DefaultBufferSize, FileOptions.None, path, false)
        {
        }

        public FileStream(String path, FileMode mode, FileAccess access)
            : this(path, mode, access, FileShare.Read, DefaultBufferSize, FileOptions.None, path, false)
        {
        }

        public FileStream(String path, FileMode mode, FileAccess access, FileShare share)
            : this(path, mode, access, share, DefaultBufferSize, FileOptions.None, path, false)
        {
        }

        public FileStream(String path, FileMode mode, FileAccess access, FileShare share, int bufferSize)
            : this(path, mode, access, share, bufferSize, FileOptions.None, path, false)
        {
        }

        public FileStream(String path, FileMode mode, FileAccess access, FileShare share, int bufferSize, FileOptions options)
            : this(path, mode, access, share, bufferSize, options, path, false)
        {
        }

        public FileStream(String path, FileMode mode, FileAccess access, FileShare share, int bufferSize, bool useAsync)
            : this(path, mode, access, share, bufferSize, (useAsync ? FileOptions.Asynchronous : FileOptions.None), path, false)
        {
        }

        internal FileStream(String path, FileMode mode, FileAccess access, FileShare share, int bufferSize, FileOptions options, String msgPath, bool bFromProxy)
        {
            Init(path, mode, access, 0, false, share, bufferSize, options, msgPath, bFromProxy, false, false);
        }

        internal FileStream(String path, FileMode mode, FileAccess access, FileShare share, int bufferSize, FileOptions options, String msgPath, bool bFromProxy, bool useLongPath)
        {
            Init(path, mode, access, 0, false, share, bufferSize, options, msgPath, bFromProxy, useLongPath, false);
        }

        internal FileStream(String path, FileMode mode, FileAccess access, FileShare share, int bufferSize, FileOptions options, String msgPath, bool bFromProxy, bool useLongPath, bool checkHost)
        {
            Init(path, mode, access, 0, false, share, bufferSize, options, msgPath, bFromProxy, useLongPath, checkHost);
        }

        private void Init(String path, FileMode mode, FileAccess access, int rights, bool useRights, FileShare share, int bufferSize, FileOptions options, String msgPath, bool bFromProxy, bool useLongPath, bool checkHost)
        {
            if (path == null)
                throw new ArgumentNullException("path", "Path");
            if (path.Length == 0)
                throw new ArgumentException("EmptyPath");

            // msgPath must be safe to hand back to untrusted code.

            _fileName = msgPath;  // To handle odd cases of finalizing partially constructed objects.
            _exposedHandle = false;

            // don't include inheritable in our bounds check for share
            FileShare tempshare = share & ~FileShare.Inheritable;
            String badArg = null;

            if (mode < FileMode.CreateNew || mode > FileMode.Append)
                badArg = "mode";
            else if (!useRights && (access < FileAccess.Read || access > FileAccess.ReadWrite))
                badArg = "access";
            else if (tempshare < FileShare.None || tempshare > (FileShare.ReadWrite | FileShare.Delete))
                badArg = "share";

            if (badArg != null)
                throw new ArgumentOutOfRangeException(badArg, "Enum");

            // NOTE: any change to FileOptions enum needs to be matched here in the error validation
            if (options != FileOptions.None && (options & ~(FileOptions.WriteThrough | FileOptions.Asynchronous | FileOptions.RandomAccess | FileOptions.DeleteOnClose | FileOptions.SequentialScan | FileOptions.Encrypted | (FileOptions)0x20000000 /* NoBuffering */)) != 0)
                throw new ArgumentOutOfRangeException("options", "Enum");

            if (bufferSize <= 0)
                throw new ArgumentOutOfRangeException("bufferSize", "NeedPosNum");

            if (!useRights && (access & FileAccess.Write) == 0)
            {
                if (mode == FileMode.Truncate || mode == FileMode.CreateNew || mode == FileMode.Create || mode == FileMode.Append)
                {
                    // No write access
                    if (!useRights)
                        throw new ArgumentException("InvalidFileMode&AccessCombo");
                }
            }

            int fAccess;
            if (!useRights)
            {
                fAccess = access == FileAccess.Read ? GENERIC_READ :
                access == FileAccess.Write ? GENERIC_WRITE :
                GENERIC_READ | GENERIC_WRITE;
            }
            else
            {
                fAccess = rights;
            }

            // Get absolute path - Security needs this to prevent something
            // like trying to create a file in c:\tmp with the name 
            // "..\WinNT\System32\ntoskrnl.exe".  Store it for user convenience.
            String filePath = path;

            _fileName = filePath;

            // In 4.0, we always construct a FileIOPermission object below. 
            // If filePath contained a ':', we would throw a NotSupportedException in 
            // System.Security.Util.StringExpressionSet.CanonicalizePath. 
            // If filePath contained other illegal characters, we would throw an ArgumentException in 
            // FileIOPermission.CheckIllegalCharacters.
            // In 4.5 we on longer construct the FileIOPermission object in full trust.
            // To preserve the 4.0 behavior we do an explicit check for ':' here and also call Path.CheckInvalidPathChars.
            // Note that we need to call CheckInvalidPathChars before checking for ':' because that is what FileIOPermission does.

            bool read = false;

            if (!useRights && (access & FileAccess.Read) != 0)
            {
                if (mode == FileMode.Append)
                    throw new ArgumentException("InvalidAppendMode");
                else
                    read = true;
            }

            // Our Inheritable bit was stolen from Windows, but should be set in
            // the security attributes class.  Don't leave this bit set.
            share &= ~FileShare.Inheritable;

            bool seekToEnd = (mode == FileMode.Append);
            // Must use a valid Win32 constant here...
            if (mode == FileMode.Append)
                mode = FileMode.OpenOrCreate;

            // WRT async IO, do the right thing for whatever platform we're on.
            // This way, someone can easily write code that opens a file 
            // asynchronously no matter what their platform is.  
            if (_canUseAsync && (options & FileOptions.Asynchronous) != 0)
                _isAsync = true;
            else
                options &= ~FileOptions.Asynchronous;

            int flagsAndAttributes = (int)options;

            if (!useRights)
            {
                _canRead = (access & FileAccess.Read) != 0;
                _canWrite = (access & FileAccess.Write) != 0;
            }

            // create safehandle
            _handle = new IO.SafeFileHandle();
            _handle.OpenFile(filePath, mode, access);

            _canSeek = true;
            _isPipe = false;
            _pos = 0;
            _bufferSize = bufferSize;
            _readPos = 0;
            _readLen = 0;
            _writePos = 0;

            // For Append mode...
            if (seekToEnd)
            {
                _appendStart = SeekCore(0, SeekOrigin.End);
            }
            else
            {
                _appendStart = -1;
            }
        }

        public FileStream(SafeFileHandle handle, FileAccess access)
            : this(handle, access, DefaultBufferSize, false)
        {
        }

        public FileStream(SafeFileHandle handle, FileAccess access, int bufferSize)
            : this(handle, access, bufferSize, false)
        {
        }

        public FileStream(SafeFileHandle handle, FileAccess access, int bufferSize, bool isAsync)
        {
            // To ensure we don't leak a handle, put it in a SafeFileHandle first
            if (handle.IsInvalid)
                throw new ArgumentException("InvalidHandle", "handle");

            _handle = handle;
            _exposedHandle = true;

            // Now validate arguments.
            if (access < FileAccess.Read || access > FileAccess.ReadWrite)
                throw new ArgumentOutOfRangeException("access", "Enum");
            if (bufferSize <= 0)
                throw new ArgumentOutOfRangeException("bufferSize", "NeedPosNum");

            _isAsync = isAsync && _canUseAsync;  // On Win9x, just do the right thing.
            _canRead = 0 != (access & FileAccess.Read);
            _canWrite = 0 != (access & FileAccess.Write);
            _canSeek = true;
            _bufferSize = bufferSize;
            _readPos = 0;
            _readLen = 0;
            _writePos = 0;
            _fileName = null;
            _isPipe = false;

            // This is necessary for async IO using IO Completion ports via our 
            // managed Threadpool API's.  This calls the OS's 
            // BindIoCompletionCallback method, and passes in a stub for the 
            // LPOVERLAPPED_COMPLETION_ROUTINE.  This stub looks at the Overlapped
            // struct for this request and gets a delegate to a managed callback 
            // from there, which it then calls on a threadpool thread.  (We allocate
            // our native OVERLAPPED structs 2 pointers too large and store EE 
            // state & a handle to a delegate there.)
            if (_canSeek)
                SeekCore(0, SeekOrigin.Current);
            else
                _pos = 0;
        }

        // Verifies that this handle supports synchronous IO operations (unless you
        // didn't open it for either reading or writing).
        private void VerifyHandleIsSync()
        {
            // Do NOT use this method on pipes.  Reading or writing to a pipe may
            // cause an app to block incorrectly, introducing a deadlock (depending
            // on whether a write will wake up an already-blocked thread or this
            // FileStream's thread).

            // Do NOT change this to use a byte[] of length 0, or test test won't
            // work.  Our ReadFile & WriteFile methods are special cased to return
            // for arrays of length 0, since we'd get an IndexOutOfRangeException 
            // while using C#'s fixed syntax.
            byte[] bytes = new byte[1];
            int hr = 0;
            int r = 0;

            // If the handle is a pipe, ReadFile will block until there
            // has been a write on the other end.  We'll just have to deal with it,
            // For the read end of a pipe, you can mess up and 
            // accidentally read synchronously from an async pipe.
            if (CanRead)
            {
                r = ReadFileNative(_handle, bytes, 0, 0, out hr);
            }
            else if (CanWrite)
            {
                r = WriteFileNative(_handle, bytes, 0, 0, out hr);
            }

            if (hr == ERROR_INVALID_PARAMETER)
                throw new ArgumentException("HandleNotSync");
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
            get { return _canSeek; }
        }

        public virtual bool IsAsync
        {
            get { return _isAsync; }
        }

        public override long Length
        {
            get
            {
                return 0;
            }
        }

        public String Name
        {
            get
            {
                if (_fileName == null)
                    return "UnknownFileName";
                return _fileName;
            }
        }

        internal String NameInternal
        {
            get
            {
                if (_fileName == null)
                    return "<UnknownFileName>";
                return _fileName;
            }
        }

        public override long Position
        {
            get
            {
                if (_handle.IsClosed) throw new Exception("FileNotOpen");
                if (!CanSeek) throw new Exception("SeekNotSupported");

                

                // Verify that internal position is in [....] with the handle
                if (_exposedHandle)
                    VerifyOSHandlePosition();

                // Compensate for buffer that we read from the handle (_readLen) Vs what the user
                // read so far from the internel buffer (_readPos). Of course add any unwrittern  
                // buffered data
                return _pos + (_readPos - _readLen + _writePos);
            }
            set
            {
                if (value < 0) throw new ArgumentOutOfRangeException("value", "NeedNonNegNum");
                if (_writePos > 0) FlushWrite(false);
                _readPos = 0;
                _readLen = 0;
                Seek(value, SeekOrigin.Begin);
            }
        }

        protected override void Dispose(bool disposing)
        {
            // Nothing will be done differently based on whether we are 
            // disposing vs. finalizing.  This is taking advantage of the
            // weak ordering between normal finalizable objects & critical
            // finalizable objects, which I included in the SafeHandle 
            // design for FileStream, which would often "just work" when 
            // finalized.
            try
            {
                if (_handle != null && !_handle.IsClosed)
                {
                    // Flush data to disk iff we were writing.  After 
                    // thinking about this, we also don't need to flush
                    // our read position, regardless of whether the handle
                    // was exposed to the user.  They probably would NOT 
                    // want us to do this.
                    if (_writePos > 0)
                    {
                        FlushWrite(!disposing);
                    }
                }
            }
            finally
            {
                if (_handle != null && !_handle.IsClosed)
                    _handle.Dispose();

                _canRead = false;
                _canWrite = false;
                _canSeek = false;
                // Don't set the buffer to null, to avoid a NullReferenceException
                // when users have a race condition in their code (ie, they call
                // Close when calling another method on Stream like Read).
                //_buffer = null;
                base.Dispose(disposing);
            }
        }

        ~FileStream()
        {
            if (_handle != null)
            {
                Dispose(false);
            }
        }

        public override void Flush()
        {
            Flush(false);
        }

        public virtual void Flush(Boolean flushToDisk)
        {
            // This code is duplicated in Dispose
            if (_handle.IsClosed) throw new Exception("FileNotOpen");

            FlushInternalBuffer();

            if (flushToDisk && CanWrite)
            {
                FlushOSBuffer();
            }
        }

        private void FlushInternalBuffer()
        {
            if (_writePos > 0)
            {
                FlushWrite(false);
            }
            else if (_readPos < _readLen && CanSeek)
            {
                FlushRead();
            }
        }

        private void FlushOSBuffer()
        {
            _handle.Flush();
        }

        // Reading is done by blocks from the file, but someone could read
        // 1 byte from the buffer then write.  At that point, the OS's file
        // pointer is out of [....] with the stream's position.  All write 
        // functions should call this function to preserve the position in the file.
        private void FlushRead()
        {
            
            if (_readPos - _readLen != 0)
            {
                
                SeekCore(_readPos - _readLen, SeekOrigin.Current);
            }
            _readPos = 0;
            _readLen = 0;
        }

        // Writes are buffered.  Anytime the buffer fills up 
        // (_writePos + delta > _bufferSize) or the buffer switches to reading
        // and there is left over data (_writePos > 0), this function must be called.
        private void FlushWrite(bool calledFromFinalizer)
        {
            WriteCore(_buffer, 0, _writePos);
            _writePos = 0;
        }

        public virtual SafeFileHandle SafeFileHandle
        {
            get
            {
                Flush();
                // Explicitly dump any buffered data, since the user could move our
                // position or write to the file.
                _readPos = 0;
                _readLen = 0;
                _writePos = 0;
                _exposedHandle = true;

                return _handle;
            }
        }

        public override void SetLength(long value)
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException("value", "NeedNonNegNum");

            if (_handle.IsClosed) throw new Exception("FileNotOpen");
            if (!CanSeek) throw new Exception("SeekNotSupported");
            if (!CanWrite) throw new Exception("WriteNotSupported");

            // Handle buffering updates.
            if (_writePos > 0)
            {
                FlushWrite(false);
            }
            else if (_readPos < _readLen)
            {
                FlushRead();
            }
            _readPos = 0;
            _readLen = 0;

            if (_appendStart != -1 && value < _appendStart)
                throw new IOException("SetLengthAppendTruncate");
            SetLengthCore(value);
        }

        // We absolutely need this method broken out so that BeginWriteCore can call
        // a method without having to go through buffering code that might call
        // FlushWrite.
        private void SetLengthCore(long value)
        {
            
            long origPos = _pos;

            if (_exposedHandle)
                VerifyOSHandlePosition();
            if (_pos != value)
                SeekCore(value, SeekOrigin.Begin);
            // Return file pointer to where it was before setting length
            if (origPos != value)
            {
                if (origPos < value)
                    SeekCore(origPos, SeekOrigin.Begin);
                else
                    SeekCore(0, SeekOrigin.End);
            }
        }

        public override int Read(byte[] array, int offset, int count)
        {
            if (array == null)
                throw new ArgumentNullException("array", "Buffer");
            if (offset < 0)
                throw new ArgumentOutOfRangeException("offset", "NeedNonNegNum");
            if (count < 0)
                throw new ArgumentOutOfRangeException("count", "NeedNonNegNum");
            if (array.Length - offset < count)
                throw new ArgumentException("InvalidOffLen");

            if (_handle.IsClosed) throw new Exception("FileNotOpen");

            

            bool isBlocked = false;
            int n = _readLen - _readPos;
            // if the read buffer is empty, read into either user's array or our
            // buffer, depending on number of bytes user asked for and buffer size.
            if (n == 0)
            {
                if (!CanRead) throw new Exception("ReadNotSupported");
                if (_writePos > 0) FlushWrite(false);
                if (!CanSeek || (count >= _bufferSize))
                {
                    n = ReadCore(array, offset, count);
                    // Throw away read buffer.
                    _readPos = 0;
                    _readLen = 0;
                    return n;
                }
                if (_buffer == null) _buffer = new byte[_bufferSize];
                n = ReadCore(_buffer, 0, _bufferSize);
                if (n == 0) return 0;
                isBlocked = n < _bufferSize;
                _readPos = 0;
                _readLen = n;
            }
            // Now copy min of count or numBytesAvailable (ie, near EOF) to array.
            if (n > count) n = count;
            Buffer.InternalBlockCopy(_buffer, _readPos, array, offset, n);
            _readPos += n;

            // We may have read less than the number of bytes the user asked 
            // for, but that is part of the Stream contract.  Reading again for
            // more data may cause us to block if we're using a device with 
            // no clear end of file, such as a serial port or pipe.  If we
            // blocked here & this code was used with redirected pipes for a
            // process's standard output, this can lead to deadlocks involving
            // two processes. But leave this here for files to avoid what would
            // probably be a breaking change.         -- 

            // If we are reading from a device with no clear EOF like a 
            // serial port or a pipe, this will cause us to block incorrectly.
            if (!_isPipe)
            {
                // If we hit the end of the buffer and didn't have enough bytes, we must
                // read some more from the underlying stream.  However, if we got
                // fewer bytes from the underlying stream than we asked for (ie, we're 
                // probably blocked), don't ask for more bytes.
                if (n < count && !isBlocked)
                {
                    
                    int moreBytesRead = ReadCore(array, offset + n, count - n);
                    n += moreBytesRead;
                    // We've just made our buffer inconsistent with our position 
                    // pointer.  We must throw away the read buffer.
                    _readPos = 0;
                    _readLen = 0;
                }
            }

            return n;
        }

        private int ReadCore(byte[] buffer, int offset, int count)
        {
            // Make sure we are reading from the right spot
            if (_exposedHandle)
                VerifyOSHandlePosition();

            int hr = 0;
            int r = ReadFileNative(_handle, buffer, offset, count, out hr);
            if (r == -1)
            {
                // For pipes, ERROR_BROKEN_PIPE is the normal end of the pipe.
                if (hr == ERROR_BROKEN_PIPE)
                {
                    r = 0;
                }
                else
                {
                    if (hr == ERROR_INVALID_PARAMETER)
                        throw new ArgumentException("HandleNotSync");
                }
            }
            
            _pos += r;

            return r;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            if (origin < SeekOrigin.Begin || origin > SeekOrigin.End)
                throw new ArgumentException("InvalidSeekOrigin");
            if (_handle.IsClosed) throw new Exception("FileNotOpen");
            if (!CanSeek) throw new Exception("SeekNotSupported");

            

            // If we've got bytes in our buffer to write, write them out.
            // If we've read in and consumed some bytes, we'll have to adjust
            // our seek positions ONLY IF we're seeking relative to the current
            // position in the stream.  This simulates doing a seek to the new
            // position, then a read for the number of bytes we have in our buffer.
            if (_writePos > 0)
            {
                FlushWrite(false);
            }
            else if (origin == SeekOrigin.Current)
            {
                // Don't call FlushRead here, which would have caused an infinite
                // loop.  Simply adjust the seek origin.  This isn't necessary
                // if we're seeking relative to the beginning or end of the stream.
                offset -= (_readLen - _readPos);
            }

            // Verify that internal position is in [....] with the handle
            if (_exposedHandle)
                VerifyOSHandlePosition();

            long oldPos = _pos + (_readPos - _readLen);
            long pos = SeekCore(offset, origin);

            // Prevent users from overwriting data in a file that was opened in
            // append mode.
            if (_appendStart != -1 && pos < _appendStart)
            {
                SeekCore(oldPos, SeekOrigin.Begin);
                throw new IOException("SeekAppendOverwrite");
            }

            // We now must update the read buffer.  We can in some cases simply
            // update _readPos within the buffer, copy around the buffer so our 
            // Position property is still correct, and avoid having to do more 
            // reads from the disk.  Otherwise, discard the buffer's contents.
            if (_readLen > 0)
            {
                // We can optimize the following condition:
                // oldPos - _readPos <= pos < oldPos + _readLen - _readPos
                if (oldPos == pos)
                {
                    if (_readPos > 0)
                    {
                        //Console.WriteLine("Seek: seeked for 0, adjusting buffer back by: "+_readPos+"  _readLen: "+_readLen);
                        Buffer.InternalBlockCopy(_buffer, _readPos, _buffer, 0, _readLen - _readPos);
                        _readLen -= _readPos;
                        _readPos = 0;
                    }
                    // If we still have buffered data, we must update the stream's 
                    // position so our Position property is correct.
                    if (_readLen > 0)
                        SeekCore(_readLen, SeekOrigin.Current);
                }
                else if (oldPos - _readPos < pos && pos < oldPos + _readLen - _readPos)
                {
                    int diff = (int)(pos - oldPos);
                    //Console.WriteLine("Seek: diff was "+diff+", readpos was "+_readPos+"  adjusting buffer - shrinking by "+ (_readPos + diff));
                    Buffer.InternalBlockCopy(_buffer, _readPos + diff, _buffer, 0, _readLen - (_readPos + diff));
                    _readLen -= (_readPos + diff);
                    _readPos = 0;
                    if (_readLen > 0)
                        SeekCore(_readLen, SeekOrigin.Current);
                }
                else
                {
                    // Lose the read buffer.
                    _readPos = 0;
                    _readLen = 0;
                }
                
                
            }
            return pos;
        }

        private long SeekCore(long offset, SeekOrigin origin)
        {
            long ret = 0;
            _pos = ret;
            return ret;
        }

        // Checks the position of the OS's handle equals what we expect it to.
        // This will fail if someone else moved the FileStream's handle or if
        // we've hit a bug in FileStream's position updating code.
        private void VerifyOSHandlePosition()
        {
            if (!CanSeek)
                return;

            // SeekCore will override the current _pos, so save it now
            long oldPos = _pos;
            long curPos = SeekCore(0, SeekOrigin.Current);

            if (curPos != oldPos)
            {
                // For reads, this is non-fatal but we still could have returned corrupted 
                // data in some cases. So discard the internal buffer. Potential MDA 
                _readPos = 0;
                _readLen = 0;
                if (_writePos > 0)
                {
                    // Discard the buffer and let the user know!
                    _writePos = 0;
                    throw new IOException("FileStreamHandlePosition");
                }
            }
        }

        public override void Write(byte[] array, int offset, int count)
        {
            if (array == null)
                throw new ArgumentNullException("array", "Buffer");
            if (offset < 0)
                throw new ArgumentOutOfRangeException("offset", "NeedNonNegNum");
            if (count < 0)
                throw new ArgumentOutOfRangeException("count", "NeedNonNegNum");
            if (array.Length - offset < count)
                throw new ArgumentException("InvalidOffLen");

            if (_handle.IsClosed) throw new Exception("FileNotOpen");

            if (_writePos == 0)
            {
                // Ensure we can write to the stream, and ready buffer for writing.
                if (!CanWrite) throw new Exception("WriteNotSupported");
                if (_readPos < _readLen) FlushRead();
                _readPos = 0;
                _readLen = 0;
            }

            // If our buffer has data in it, copy data from the user's array into
            // the buffer, and if we can fit it all there, return.  Otherwise, write
            // the buffer to disk and copy any remaining data into our buffer.
            // The assumption here is memcpy is cheaper than disk (or net) IO.
            // (10 milliseconds to disk vs. ~20-30 microseconds for a 4K memcpy)
            // So the extra copying will reduce the total number of writes, in 
            // non-pathological cases (ie, write 1 byte, then write for the buffer 
            // size repeatedly)
            if (_writePos > 0)
            {
                int numBytes = _bufferSize - _writePos;   // space left in buffer
                if (numBytes > 0)
                {
                    if (numBytes > count)
                        numBytes = count;
                    Buffer.InternalBlockCopy(array, offset, _buffer, _writePos, numBytes);
                    _writePos += numBytes;
                    if (count == numBytes) return;
                    offset += numBytes;
                    count -= numBytes;
                }
                // Reset our buffer.  We essentially want to call FlushWrite
                // without calling Flush on the underlying Stream.
                WriteCore(_buffer, 0, _writePos);
                _writePos = 0;
            }
            // If the buffer would slow writes down, avoid buffer completely.
            if (count >= _bufferSize)
            {
                
                WriteCore(array, offset, count);
                return;
            }
            else if (count == 0)
                return;  // Don't allocate a buffer then call memcpy for 0 bytes.
            if (_buffer == null) _buffer = new byte[_bufferSize];
            // Copy remaining bytes into buffer, to write at a later date.
            Buffer.InternalBlockCopy(array, offset, _buffer, _writePos, count);
            _writePos = count;
            return;
        }

        private unsafe void WriteCore(byte[] buffer, int offset, int count)
        {
            // Make sure we are writing to the position that we think we are
            if (_exposedHandle)
                VerifyOSHandlePosition();

            int hr = 0;
            int r = WriteFileNative(_handle, buffer, offset, count, out hr);
            if (r == -1)
            {
                throw new IOException();
            }
            
            _pos += r;
            return;
        }

        public override IAsyncResult BeginRead(byte[] array, int offset, int numBytes, AsyncCallback userCallback, Object stateObject)
        {
            if (array == null)
                throw new ArgumentNullException("array");
            if (offset < 0)
                throw new ArgumentOutOfRangeException("offset", "NeedNonNegNum");
            if (numBytes < 0)
                throw new ArgumentOutOfRangeException("numBytes", "NeedNonNegNum");
            if (array.Length - offset < numBytes)
                throw new ArgumentException("InvalidOffLen");

            if (_handle.IsClosed) throw new Exception("FileNotOpen");

            return base.BeginRead(array, offset, numBytes, userCallback, stateObject);
        }

        public unsafe override int EndRead(IAsyncResult asyncResult)
        {
            // There are 3 significantly different IAsyncResults we'll accept
            // here.  One is from Stream::BeginRead.  The other two are variations
            // on our FileStreamAsyncResult.  One is from BeginReadCore,
            // while the other is from the BeginRead buffering wrapper.
            if (asyncResult == null)
                throw new ArgumentNullException("asyncResult");

            return base.EndRead(asyncResult);
        }

        public override int ReadByte()
        {
            if (_handle.IsClosed) throw new Exception("FileNotOpen");
            if (_readLen == 0 && !CanRead) throw new Exception("ReadNotSupported");
            
            if (_readPos == _readLen)
            {
                if (_writePos > 0) FlushWrite(false);
                
                if (_buffer == null) _buffer = new byte[_bufferSize];
                _readLen = ReadCore(_buffer, 0, _bufferSize);
                _readPos = 0;
            }
            if (_readPos == _readLen)
                return -1;

            int result = _buffer[_readPos];
            _readPos++;
            return result;
        }


        public override IAsyncResult BeginWrite(byte[] array, int offset, int numBytes, AsyncCallback userCallback, Object stateObject)
        {
            if (array == null)
                throw new ArgumentNullException("array");
            if (offset < 0)
                throw new ArgumentOutOfRangeException("offset", "NeedNonNegNum");
            if (numBytes < 0)
                throw new ArgumentOutOfRangeException("numBytes", "NeedNonNegNum");
            if (array.Length - offset < numBytes)
                throw new ArgumentException("InvalidOffLen");
            
            if (_handle.IsClosed) throw new Exception("FileNotOpen");

            return base.BeginWrite(array, offset, numBytes, userCallback, stateObject);
        }

        public unsafe override void EndWrite(IAsyncResult asyncResult)
        {
            if (asyncResult == null)
                throw new ArgumentNullException("asyncResult");

            base.EndWrite(asyncResult);
        }

        public override void WriteByte(byte value)
        {
            if (_handle.IsClosed) throw new Exception("FileNotOpen");
            if (_writePos == 0)
            {
                if (!CanWrite) throw new Exception("WriteNotSupported");
                if (_readPos < _readLen) FlushRead();
                _readPos = 0;
                _readLen = 0;
                
                if (_buffer == null) _buffer = new byte[_bufferSize];
            }
            if (_writePos == _bufferSize)
                FlushWrite(false);

            _buffer[_writePos] = value;
            _writePos++;
        }

        // Windows API definitions, from winbase.h and others

        private const int FILE_ATTRIBUTE_NORMAL = 0x00000080;
        private const int FILE_ATTRIBUTE_ENCRYPTED = 0x00004000;
        private const int FILE_FLAG_OVERLAPPED = 0x40000000;
        internal const int GENERIC_READ = unchecked((int)0x80000000);
        private const int GENERIC_WRITE = 0x40000000;

        private const int FILE_BEGIN = 0;
        private const int FILE_CURRENT = 1;
        private const int FILE_END = 2;

        // Error codes (not HRESULTS), from winerror.h
        internal const int ERROR_BROKEN_PIPE = 109;
        internal const int ERROR_NO_DATA = 232;
        private const int ERROR_HANDLE_EOF = 38;
        private const int ERROR_INVALID_PARAMETER = 87;
        private const int ERROR_IO_PENDING = 997;


        // __ConsoleStream also uses this code. 
        private int ReadFileNative(SafeFileHandle handle, byte[] bytes, int offset, int count, out int hr)
        {
            // Don't corrupt memory when multiple threads are erroneously writing
            // to this stream simultaneously.
            if (bytes.Length - offset < count)
                throw new IndexOutOfRangeException("IORaceCondition");

            // You can't use the fixed statement on an array of length 0.
            if (bytes.Length == 0)
            {
                hr = 0;
                return 0;
            }

            int r = 0;
            int numBytesRead = 0;

            hr = 0;
            numBytesRead = handle.ReadFile(bytes, offset, count);

            return numBytesRead;
        }

        private unsafe int WriteFileNative(SafeFileHandle handle, byte[] bytes, int offset, int count, out int hr)
        {
            // Don't corrupt memory when multiple threads are erroneously writing
            // to this stream simultaneously.  (the OS is reading from
            // the array we pass to WriteFile, but if we read beyond the end and
            // that memory isn't allocated, we could get an AV.)
            if (bytes.Length - offset < count)
                throw new IndexOutOfRangeException("IORaceCondition");

            // You can't use the fixed statement on an array of length 0.
            if (bytes.Length == 0)
            {
                hr = 0;
                return 0;
            }

            int numBytesWritten = 0;
            int r = 0;

            hr = 0;
            numBytesWritten = handle.WriteFile(bytes, offset, count);

            return numBytesWritten;
        }
    }
}
