// Licensed under the MIT license.

namespace System.Runtime.InteropServices
{
    using System.Runtime.CompilerServices;

    using Microsoft.Win32;

    public static partial class Marshal
    {
        [MethodImplAttribute(MethodImplOptions.Unmanaged)]
        private extern static int _set_errno(int _Value);

        [MethodImplAttribute(MethodImplOptions.Unmanaged)]
        private extern unsafe static int _get_errno(int* _Value);

        //====================================================================
        // GetLastWin32Error
        //====================================================================
        public static int GetLastWin32Error()
        {
            unsafe
            {
                return _get_errno(null);
            }
        }

        //====================================================================
        // SetLastWin32Error
        //====================================================================
        internal static void SetLastWin32Error(int error)
        {
            _set_errno(error);
        }

        public static int SizeOf<T>(T obj)
        {
            return 0;
        }

        public static uint AlignedSizeOf<T>()
        {
            return 0;
        }

        public static uint SizeOfType(Type type)
        {
            return 0;
        }

        public static Exception GetExceptionForHR(int getLastWin32Error)
        {
            return new Exception();
        }

        public static int ReadInt32(IntPtr arModifiers, int p1)
        {
            throw new NotImplementedException();
        }

        public static int ReadInt16(IntPtr blobStart)
        {
            throw new NotImplementedException();
        }
    }
}

