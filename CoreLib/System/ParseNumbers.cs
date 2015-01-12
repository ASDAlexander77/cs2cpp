namespace System
{

    //This class contains only static members and does not need to be serializable.
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.Versioning;

    internal static class ParseNumbers
    {
        internal const int PrintAsI1 = 0x40;
        internal const int PrintAsI2 = 0x80;
        internal const int PrintAsI4 = 0x100;
        internal const int TreatAsUnsigned = 0x200;
        internal const int TreatAsI1 = 0x400;
        internal const int TreatAsI2 = 0x800;
        internal const int IsTight = 0x1000;
        internal const int NoSpace = 0x2000;

        //
        //
        // NATIVE METHODS
        // For comments on these methods please see $\src\vm\COMUtilNative.cpp
        //
        public unsafe static long StringToLong(System.String s, int radix, int flags)
        {
            return StringToLong(s, radix, flags, null);
        }

        public unsafe static long StringToLong(System.String s, int radix, int flags, int* currPos)
        {
            throw new NotImplementedException();
        }

        public unsafe static long StringToLong(System.String s, int radix, int flags, ref int currPos)
        {
            fixed (int* ppos = &currPos)
            {
                return StringToLong(s, radix, flags, ppos);
            }
        }


        public unsafe static int StringToInt(System.String s, int radix, int flags)
        {
            return StringToInt(s, radix, flags, null);
        }



        public unsafe static int StringToInt(System.String s, int radix, int flags, int* currPos)
        {
            throw new NotImplementedException();
        }

        public unsafe static int StringToInt(System.String s, int radix, int flags, ref int currPos)
        {
            fixed (int* ppos = &currPos)
            {
                return StringToInt(s, radix, flags, ppos);
            }
        }

        public static String IntToString(int l, int radix, int width, char paddingChar, int flags)
        {
            throw new NotImplementedException();
        }

        public static String LongToString(long l, int radix, int width, char paddingChar, int flags)
        {
            throw new NotImplementedException();
        }
    }
}
