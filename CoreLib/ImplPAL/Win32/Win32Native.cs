namespace Microsoft.Win32
{
    using System;
    using System.Security;
    using System.Text;
    using System.Threading;
    using Microsoft.Win32.SafeHandles;
    using System.Runtime.CompilerServices;
    using System.Runtime.ConstrainedExecution;

    using BOOL = System.Int32;
    using DWORD = System.UInt32;
    using ULONG = System.UInt32;

    /**
     * Win32 encapsulation for MSCORLIB.
     */
    // Remove the default demands for all P/Invoke methods with this
    // global declaration on the class.

    [SuppressUnmanagedCodeSecurityAttribute()]
    internal static partial class Win32Native
    {
        private const int STDIN_FILENO = 0;

        private const int STDOUT_FILENO = 1;

        private const int STDERR_FILENO = 2;

        private const int O_RDONLY = 0x0000;	/* open for reading only */

        private const int O_WRONLY = 0x0001;	/* open for writing only */

        private const int O_RDWR = 0x0002;	/* open for reading and writing */

        private const int O_CREAT = 0x0100;		/* create if nonexistant */

        private const int O_TRUNC = 0x0200;		/* truncate to zero length */

        private const int O_EXCL = 0x0040;		/* error if already exists */

        private const int F_SETFD = 2;		/* set file descriptor flags */

        private const int LOCK_SH = 0x01;		/* shared file lock */

        private const int LOCK_EX = 0x02;		/* exclusive file lock */

        private const int LOCK_NB = 0x04;		/* don't block when locking */

        private const int LOCK_UN = 0x08;		/* unlock file */

        /* access function */
        private const int F_OK = 0;	/* test for existence of file */

        private const int O_DIRECT = 00040000;

        private const int O_BINARY = 0x8000;

        private const int S_IRUSR = 0000400;			/* R for owner */

        private const int S_IWUSR = 0000200;			/* W for owner */

        private const int S_IROTH = 0000004;			/* R for other */

        private const int S_IRGRP = 0000040;			/* R for group */

        private const uint GENERIC_READ = 0x80000000;

        private const uint GENERIC_WRITE = 0x40000000;

        public const uint FILE_ATTRIBUTE_HIDDEN = 0x00000002;

        public const uint FILE_ATTRIBUTE_SYSTEM = 0x00000004;

        //public const uint FILE_ATTRIBUTE_DIRECTORY = 0x00000010;

        public const uint FILE_ATTRIBUTE_ARCHIVE = 0x00000020;

        public const uint FILE_ATTRIBUTE_DEVICE = 0x00000040;

        public const uint FILE_ATTRIBUTE_NORMAL = 0x00000080;

        private const uint S_IFMT = 0xF000;

        private const uint S_IFDIR = 0x4000;

        private const int FILE_FLAG_NO_BUFFERING = 0x20000000;

        [MethodImpl(MethodImplOptions.Unmanaged)]
        public unsafe static extern byte* __get_full_path(byte* file_name, byte* resolved_name);

        [MethodImplAttribute(MethodImplOptions.Unmanaged)]
        private extern unsafe static int open(byte* fileName, int flags, int mode);

        [MethodImplAttribute(MethodImplOptions.Unmanaged)]
        private extern static int close(int fd);

        [MethodImpl(MethodImplOptions.Unmanaged)]
        public unsafe static extern int write(int fd, void* buf, int count);

        [MethodImpl(MethodImplOptions.Unmanaged)]
        public unsafe static extern int read(int fd, void* buf, int count);

        [MethodImpl(MethodImplOptions.Unmanaged)]
        public static extern int fcntl(int fd, int cmd, __arglist);

        [MethodImpl(MethodImplOptions.Unmanaged)]
        public static extern int flock(int fd, int operation);

        [MethodImpl(MethodImplOptions.Unmanaged)]
        public static unsafe extern int access(byte* pathname, int mode);

        [MethodImpl(MethodImplOptions.Unmanaged)]
        public static unsafe extern int stat(byte* path, int* buf);

        [MethodImpl(MethodImplOptions.Unmanaged)]
        public static unsafe extern int fstat(int fd, int* buf);

        private static int _errorMode;

        struct stat_data
        {
            public int st_dev;     /* ID of device containing file */
            public int st_ino;     /* inode number */
            public ushort st_mode;    /* protection */
            public short st_nlink;   /* number of hard links */
            public short st_uid;     /* user ID of owner */
            public short st_gid;     /* group ID of owner */
            public int st_rdev;    /* device ID (if special file) */
            public int st_size;    /* total size, in bytes */
            public int st_atime;   /* time of last access */
            public int st_mtime;   /* time of last modification */
            public int st_ctime;   /* time of last status change */
            public int reserved0;
            public int reserved1;
            public int reserved2;
            public int reserved3;
            public int reserved4;
            public int reserved5;
            public int reserved6;
        };

        public static byte[] ToAsciiString(string s)
        {
            if (s == null)
            {
                throw new ArgumentNullException("s", "String");
            }

            var byteCount = Encoding.ASCII.GetByteCount(s);
            // +1 needed for ending \0
            var bytes = new byte[byteCount + 1];
            var bytesReceived = Encoding.ASCII.GetBytes(s, 0, s.Length, bytes, 0);
            return bytes;
        }

        public static byte[] ToAsciiString(char[] chars)
        {
            if (chars == null)
            {
                throw new ArgumentNullException("chars", "Chars");
            }

            var byteCount = Encoding.ASCII.GetByteCount(chars);
            // +1 needed for ending \0
            var bytes = new byte[byteCount + 1];
            var bytesReceived = Encoding.ASCII.GetBytes(chars, 0, chars.Length, bytes, 0);
            return bytes;
        }

        internal static bool SetEvent(SafeWaitHandle handle)
        {
            var acquiredLock = false;

            try
            {
                Monitor.Enter(handle, ref acquiredLock);

                unsafe
                {
                    var waitHandleData = (int*)handle.DangerousGetHandle().ToPointer();
                    if (waitHandleData == null)
                    {
                        throw new InvalidOperationException();
                    }

                    // state = true
                    waitHandleData[0] = 1;

                    // type = AutoReset
                    if (waitHandleData[1] > 0)
                    {
                        Monitor.Pulse(handle);
                    }
                    else
                    {
                        Monitor.PulseAll(handle);
                    }
                }
            }
            finally
            {
                if (acquiredLock)
                {
                    Monitor.Exit(handle);
                }
            }

            return true;
        }

        internal static bool ResetEvent(SafeWaitHandle handle)
        {
            var acquiredLock = false;

            try
            {
                Monitor.Enter(handle, ref acquiredLock);

                unsafe
                {
                    var waitHandleData = (int*)handle.DangerousGetHandle().ToPointer();
                    if (waitHandleData == null)
                    {
                        throw new InvalidOperationException();
                    }

                    // state = false
                    waitHandleData[0] = 0;
                }
            }
            finally
            {
                if (acquiredLock)
                {
                    Monitor.Exit(handle);
                }
            }

            return true;
        }

        internal static SafeWaitHandle CreateEvent(SECURITY_ATTRIBUTES lpSecurityAttributes, bool isManualReset, bool initialState, String name)
        {
            // state, type
            var waitHandleData = new int[2];

            waitHandleData[0] = initialState ? 1 : 0;
            waitHandleData[1] = isManualReset ? 0 : 1;

            // we use this SafeWaitHandle as SyncBlock object
            unsafe
            {
                fixed (int* ptr = waitHandleData)
                {
                    return new SafeWaitHandle(new IntPtr(ptr), true);
                }
            }
        }

        private static unsafe int wcslen(char* ptr)
        {
            char* end = ptr;

            // The following code is (somewhat surprisingly!) significantly faster than a naive loop,
            // at least on x86 and the current jit.

            // First make sure our pointer is aligned on a dword boundary
            while (((uint)end & 3) != 0 && *end != 0)
                end++;
            if (*end != 0)
            {
                // The loop condition below works because if "end[0] & end[1]" is non-zero, that means
                // neither operand can have been zero. If is zero, we have to look at the operands individually,
                // but we hope this going to fairly rare.

                // In general, it would be incorrect to access end[1] if we haven't made sure
                // end[0] is non-zero. However, we know the ptr has been aligned by the loop above
                // so end[0] and end[1] must be in the same page, so they're either both accessible, or both not.

                while ((end[0] & end[1]) != 0 || (end[0] != 0 && end[1] != 0))
                {
                    end += 2;
                }
            }
            // finish up with the naive loop
            for (; *end != 0; end++)
                ;

            int count = (int)(end - ptr);

            return count;
        }

        internal unsafe static int GetFullPathName(char* path, int numBufferChars, char* buffer, IntPtr mustBeZero)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path", "path");
            }

            var byteCount = Encoding.ASCII.GetByteCount(path, wcslen(path));
            // +1 needed for ending \0
            var relative_path_ascii = stackalloc byte[byteCount + 1];

            var bytesReceived = Encoding.ASCII.GetBytes(path, wcslen(path), relative_path_ascii, byteCount);

            var resolved_path_ascii = stackalloc byte[numBufferChars];

            var result = (int)__get_full_path(relative_path_ascii, resolved_path_ascii);

            if (result != 0)
            {
                Encoding.ASCII.GetChars(resolved_path_ascii, numBufferChars, buffer, numBufferChars);
                return wcslen(buffer);
            }

            return result;
        }

        internal static int GetFileType(SafeFileHandle handle)
        {
            var stdId = handle.DangerousGetHandle().ToInt32();
            if (stdId == STD_OUTPUT_HANDLE || stdId == STD_ERROR_HANDLE)
            {
                return FILE_TYPE_CHAR;
            }

            return FILE_TYPE_DISK;
        }

        // Do not use these directly, use the safe or unsafe versions above.
        // The safe version does not support devices (aka if will only open
        // files on disk), while the unsafe version give you the full semantic
        // of the native version.

        private static SafeFileHandle CreateFile(String lpFileName,
                    int dwDesiredAccess, System.IO.FileShare dwShareMode,
                    SECURITY_ATTRIBUTES securityAttrs, System.IO.FileMode dwCreationDisposition,
                    int dwFlagsAndAttributes, IntPtr hTemplateFile)
        {
            int filed = -1;
            int create_flags = (S_IRUSR | S_IWUSR | S_IRGRP | S_IROTH);
            int open_flags = 0;
            bool fFileExists = false;

            if (lpFileName == null)
            {
                return new SafeFileHandle(INVALID_HANDLE_VALUE, false);
            }

            unsafe
            {
                fixed (byte* filename_ascii = ToAsciiString(lpFileName))
                {
                    switch ((uint)dwDesiredAccess)
                    {
                        case GENERIC_READ:
                            open_flags |= O_RDONLY;
                            break;
                        case GENERIC_WRITE:
                            open_flags |= O_WRONLY;
                            break;
                        case GENERIC_READ | GENERIC_WRITE:
                            open_flags |= O_RDWR;
                            break;
                        default:
                            return new SafeFileHandle(INVALID_HANDLE_VALUE, false);
                    }

                    switch (dwCreationDisposition)
                    {
                        case System.IO.FileMode.Create:
                            // check whether the file exists
                            if (access(filename_ascii, F_OK) == 0)
                            {
                                fFileExists = true;
                            }

                            open_flags |= O_CREAT | O_TRUNC;
                            break;
                        case System.IO.FileMode.CreateNew:
                            open_flags |= O_CREAT | O_EXCL;
                            break;
                        case System.IO.FileMode.Open:
                            /* don't need to do anything here */
                            break;
                        case System.IO.FileMode.OpenOrCreate:
                            if (access(filename_ascii, F_OK) == 0)
                            {
                                fFileExists = true;
                            }

                            open_flags |= O_CREAT;
                            break;
                        case System.IO.FileMode.Truncate:
                            open_flags |= O_TRUNC;
                            break;
                        default:
                            return new SafeFileHandle(INVALID_HANDLE_VALUE, false);

                    }

                    if ((dwFlagsAndAttributes & FILE_FLAG_NO_BUFFERING) > 0)
                    {
                        open_flags |= O_DIRECT;
                    }

                    open_flags |= O_BINARY;

                    filed = open(filename_ascii, open_flags, (open_flags & O_CREAT) > 0 ? create_flags : 0);
                    if (filed < 0)
                    {
                        return new SafeFileHandle(new IntPtr(0), false);
                    }
                }
            }

#if flock
            var lock_mode = (dwShareMode == 0 /* FILE_SHARE_NONE */) ? LOCK_EX : LOCK_SH;

            if (flock(filed, lock_mode | LOCK_NB) != 0)
            {
                return new SafeFileHandle(INVALID_HANDLE_VALUE, false);
            }
#endif

#if O_DIRECT
            if ((dwFlagsAndAttributes & FILE_FLAG_NO_BUFFERING) > 0)
            {
#if F_NOCACHE
                if (-1 == fcntl(filed, F_NOCACHE, 1))
                {
                    return new SafeFileHandle(INVALID_HANDLE_VALUE, false);
                }
#else
#error Insufficient support for uncached I/O on this platform
#endif
            }
#endif

#if fcntl
            /* make file descriptor close-on-exec; inheritable handles will get
              "uncloseonexeced" in CreateProcess if they are actually being inherited*/
            var ret = fcntl(filed, F_SETFD, __arglist(1));
            if (-1 == ret)
            {
                return new SafeFileHandle(INVALID_HANDLE_VALUE, false);
            }
#endif

            return new SafeFileHandle(new IntPtr(filed), false);
        }

        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        internal static bool CloseHandle(IntPtr handle)
        {
            close(handle.ToInt32());
            return true;
        }

        unsafe internal static int ReadFile(SafeFileHandle handle, byte* bytes, int numBytesToRead, out int numBytesRead, IntPtr mustBeZero)
        {
            var r = read(handle.DangerousGetHandle().ToInt32(), bytes, numBytesToRead);
            if (r == -1)
            {
                numBytesRead = 0;
                return 0;
            }

            numBytesRead = r;
            return 1;
        }

        internal static unsafe int WriteFile(SafeFileHandle handle, byte* bytes, int numBytesToWrite, out int numBytesWritten, IntPtr mustBeZero)
        {
            var fd = handle.DangerousGetHandle().ToInt32();
            if (fd == STD_OUTPUT_HANDLE)
            {
                numBytesWritten = write(STDOUT_FILENO, bytes, numBytesToWrite);
                return numBytesWritten < numBytesToWrite ? 0 : 1;
            }
            else if (fd == STD_ERROR_HANDLE)
            {
                numBytesWritten = write(STDERR_FILENO, bytes, numBytesToWrite);
                return numBytesWritten < numBytesToWrite ? 0 : 1;
            }
            else
            {
                var r = write(fd, bytes, numBytesToWrite);
                if (r != -1)
                {
                    numBytesWritten = r;
                    return 1;
                }
            }

            numBytesWritten = 0;
            return 0;
        }

        internal static int GetFileSize(SafeFileHandle hFile, out int highSize)
        {
            highSize = 0;
            unsafe
            {
                var data = new stat_data();
                var returnCode = fstat(hFile.DangerousGetHandle().ToInt32(), &data.st_dev);
                if (returnCode != 0)
                {
                    return 0;
                }

                return data.st_size;
            }
        }

        internal static IntPtr GetStdHandle(int nStdHandle)  // param is NOT a handle, but it returns one!
        {
            return new IntPtr(nStdHandle);
        }

        internal static bool GetFileAttributesEx(String name, int fileInfoLevel, ref WIN32_FILE_ATTRIBUTE_DATA lpFileInformation)
        {
            if (name == null)
            {
                return false;
            }

            unsafe
            {
                var data = new stat_data();
                fixed (byte* filename_ascii = ToAsciiString(name))
                {
                    var returnCode = stat(filename_ascii, &data.st_dev);
                    if (returnCode != 0)
                    {
                        return false;
                    }

                    var fileAttributes = FILE_ATTRIBUTE_NORMAL;

                    // if this is folder, return false
                    if ((data.st_mode & S_IFMT) == S_IFDIR)
                    {
                        fileAttributes = FILE_ATTRIBUTE_DIRECTORY;
                    }

                    lpFileInformation.fileAttributes = (int)fileAttributes;
                    lpFileInformation.fileSizeLow = data.st_size;
                    lpFileInformation.fileSizeHigh = 0;

                    return true;
                }
            }
        }
    }
}
