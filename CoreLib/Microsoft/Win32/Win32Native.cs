// Licensed under the MIT license.

namespace Microsoft.Win32
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Threading;

    using Microsoft.Win32.SafeHandles;

    internal static class Win32Native
    {
        // Win32 ACL-related constants:
        internal const int READ_CONTROL = 0x00020000;
        internal const int SYNCHRONIZE = 0x00100000;

        internal const int STANDARD_RIGHTS_READ = READ_CONTROL;
        internal const int STANDARD_RIGHTS_WRITE = READ_CONTROL;

        // STANDARD_RIGHTS_REQUIRED  (0x000F0000L)
        // SEMAPHORE_ALL_ACCESS          (STANDARD_RIGHTS_REQUIRED|SYNCHRONIZE|0x3) 

        // SEMAPHORE and Event both use 0x0002
        // MUTEX uses 0x001 (MUTANT_QUERY_STATE)

        // Note that you may need to specify the SYNCHRONIZE bit as well
        // to be able to open a synchronization primitive.
        internal const int SEMAPHORE_MODIFY_STATE = 0x00000002;
        internal const int EVENT_MODIFY_STATE = 0x00000002;
        internal const int MUTEX_MODIFY_STATE = 0x00000001;
        internal const int MUTEX_ALL_ACCESS = 0x001F0001;

        // Error codes from WinError.h
        internal const int ERROR_SUCCESS = 0x0;
        internal const int ERROR_INVALID_FUNCTION = 0x1;
        internal const int ERROR_FILE_NOT_FOUND = 0x2;
        internal const int ERROR_PATH_NOT_FOUND = 0x3;
        internal const int ERROR_ACCESS_DENIED = 0x5;
        internal const int ERROR_INVALID_HANDLE = 0x6;
        internal const int ERROR_NOT_ENOUGH_MEMORY = 0x8;
        internal const int ERROR_INVALID_DATA = 0xd;
        internal const int ERROR_INVALID_DRIVE = 0xf;
        internal const int ERROR_NO_MORE_FILES = 0x12;
        internal const int ERROR_NOT_READY = 0x15;
        internal const int ERROR_BAD_LENGTH = 0x18;
        internal const int ERROR_SHARING_VIOLATION = 0x20;
        internal const int ERROR_NOT_SUPPORTED = 0x32;
        internal const int ERROR_FILE_EXISTS = 0x50;
        internal const int ERROR_INVALID_PARAMETER = 0x57;
        internal const int ERROR_BROKEN_PIPE = 0x6D;
        internal const int ERROR_CALL_NOT_IMPLEMENTED = 0x78;
        internal const int ERROR_INSUFFICIENT_BUFFER = 0x7A;
        internal const int ERROR_INVALID_NAME = 0x7B;
        internal const int ERROR_BAD_PATHNAME = 0xA1;
        internal const int ERROR_ALREADY_EXISTS = 0xB7;
        internal const int ERROR_ENVVAR_NOT_FOUND = 0xCB;
        internal const int ERROR_FILENAME_EXCED_RANGE = 0xCE;  // filename too long.
        internal const int ERROR_NO_DATA = 0xE8;
        internal const int ERROR_PIPE_NOT_CONNECTED = 0xE9;
        internal const int ERROR_MORE_DATA = 0xEA;
        internal const int ERROR_DIRECTORY = 0x10B;
        internal const int ERROR_OPERATION_ABORTED = 0x3E3;  // 995; For IO Cancellation
        internal const int ERROR_NOT_FOUND = 0x490;          // 1168; For IO Cancellation
        internal const int ERROR_NO_TOKEN = 0x3f0;
        internal const int ERROR_DLL_INIT_FAILED = 0x45A;
        internal const int ERROR_NON_ACCOUNT_SID = 0x4E9;
        internal const int ERROR_NOT_ALL_ASSIGNED = 0x514;
        internal const int ERROR_UNKNOWN_REVISION = 0x519;
        internal const int ERROR_INVALID_OWNER = 0x51B;
        internal const int ERROR_INVALID_PRIMARY_GROUP = 0x51C;
        internal const int ERROR_NO_SUCH_PRIVILEGE = 0x521;
        internal const int ERROR_PRIVILEGE_NOT_HELD = 0x522;
        internal const int ERROR_NONE_MAPPED = 0x534;
        internal const int ERROR_INVALID_ACL = 0x538;
        internal const int ERROR_INVALID_SID = 0x539;
        internal const int ERROR_INVALID_SECURITY_DESCR = 0x53A;
        internal const int ERROR_BAD_IMPERSONATION_LEVEL = 0x542;
        internal const int ERROR_CANT_OPEN_ANONYMOUS = 0x543;
        internal const int ERROR_NO_SECURITY_ON_OBJECT = 0x546;
        internal const int ERROR_TRUSTED_RELATIONSHIP_FAILURE = 0x6FD;

        // Error codes from ntstatus.h
        internal const uint STATUS_SUCCESS = 0x00000000;
        internal const uint STATUS_SOME_NOT_MAPPED = 0x00000107;
        internal const uint STATUS_NO_MEMORY = 0xC0000017;
        internal const uint STATUS_OBJECT_NAME_NOT_FOUND = 0xC0000034;
        internal const uint STATUS_NONE_MAPPED = 0xC0000073;
        internal const uint STATUS_INSUFFICIENT_RESOURCES = 0xC000009A;
        internal const uint STATUS_ACCESS_DENIED = 0xC0000022;

        internal const int INVALID_FILE_SIZE = -1;

        // From WinStatus.h
        internal const int STATUS_ACCOUNT_RESTRICTION = unchecked((int)0xC000006E);

        [StructLayout(LayoutKind.Sequential)]
        internal class SECURITY_ATTRIBUTES
        {
            internal int nLength = 0;
            // don't remove null, or this field will disappear in bcl.small
            internal unsafe byte* pSecurityDescriptor = null;
            internal int bInheritHandle = 0;
        }

        internal static readonly IntPtr INVALID_HANDLE_VALUE = new IntPtr(-1);  // WinBase.h

        internal static bool SetEvent(SafeWaitHandle handle)
        {
            throw new NotImplementedException();
        }

        internal static bool ResetEvent(SafeWaitHandle handle)
        {
            throw new NotImplementedException();
        }

        internal static SafeWaitHandle CreateEvent(SECURITY_ATTRIBUTES lpSecurityAttributes, bool isManualReset, bool initialState, String name)
        {
            var handler = new object();
            JitHelpers.UnsafeCastToStackPointer(ref handler);
            var safeHandler = new SafeWaitHandle(JitHelpers.UnsafeCastToStackPointer(ref handler), true);
            return safeHandler;
        }

        internal static SafeWaitHandle OpenEvent( /* DWORD */ int desiredAccess, bool inheritHandle, String name)
        {
            throw new NotImplementedException();
        }

        internal static int GetFullPathName(String path, int numBufferChars, [Out]StringBuilder buffer, IntPtr mustBeZero)
        {
            throw new NotImplementedException();
        }

        internal unsafe static int GetFullPathName(char* path, int numBufferChars, char* buffer, IntPtr mustBeZero)
        {
            throw new NotImplementedException();
        }

        internal static uint GetTempPath(int bufferLen, [Out]StringBuilder buffer)
        {
            throw new NotImplementedException();
        }

        internal static uint GetTempFileName(String tmpPath, String prefix, uint uniqueIdOrZero, [Out]StringBuilder tmpFileName)
        {
            throw new NotImplementedException();
        }

        internal unsafe static int GetLongPathName(char* path, char* longPathBuffer, int bufferLength)
        {
            throw new NotImplementedException();
        }

        internal static int GetLongPathName(String path, [Out]StringBuilder longPathBuffer, int bufferLength)
        {
            throw new NotImplementedException();
        }

        public static bool CloseHandle(IntPtr handle)
        {
            throw new NotImplementedException();
        }
    }
}
