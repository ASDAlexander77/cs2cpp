// Licensed under the MIT license.

namespace Microsoft.Win32
{
    using System;
    using System.Runtime.InteropServices;

    internal static class Win32Native
    {
        [StructLayout(LayoutKind.Sequential)]
        internal class SECURITY_ATTRIBUTES
        {
            internal int nLength = 0;
            // don't remove null, or this field will disappear in bcl.small
            internal unsafe byte* pSecurityDescriptor = null;
            internal int bInheritHandle = 0;
        }

        internal static readonly IntPtr INVALID_HANDLE_VALUE = new IntPtr(-1);  // WinBase.h

        public static bool CloseHandle(IntPtr handle)
        {
            ////throw new NotImplementedException();
            return true;
        }
    }
}
