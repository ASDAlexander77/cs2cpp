// Licensed under the MIT license.

namespace System.Runtime.InteropServices
{
    using Microsoft.Win32;

    public static partial class Marshal
    {
        //====================================================================
        // GetLastWin32Error
        //====================================================================
        public static int GetLastWin32Error()
        {
            return 0;
        }

        //====================================================================
        // SetLastWin32Error
        //====================================================================
        internal static void SetLastWin32Error(int error)
        {
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
    }
}

