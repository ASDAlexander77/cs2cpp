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
            throw new NotImplementedException();
        }

        public static uint AlignedSizeOf<T>()
        {
            throw new NotImplementedException();
        }

        public static uint SizeOfType(Type type)
        {
            throw new NotImplementedException();
        }
    }
}

