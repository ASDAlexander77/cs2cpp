// Licensed under the MIT license.

namespace System.Runtime.InteropServices
{
    using System.Runtime.CompilerServices;
    using ConstrainedExecution;
    using Microsoft.Win32;

    public static partial class Marshal
    {
        //====================================================================
        // GetLastWin32Error
        //====================================================================
        [System.Security.SecurityCritical]  // auto-generated_required
        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        public static extern int GetLastWin32Error();


        //====================================================================
        // SetLastWin32Error
        //====================================================================
        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        internal static extern void SetLastWin32Error(int error);


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

        // hack
        public static void Copy(float[] source, int startIndex, IntPtr destination, int length)
        {
        }
    }
}

