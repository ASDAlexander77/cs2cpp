////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Apache License 2.0 (Apache)
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////namespace System.Threading
namespace System.Threading
{
    using System;
    using System.Runtime.CompilerServices;
    // After much discussion, we decided the Interlocked class doesn't need
    // any HPA's for synchronization or external threading.  They hurt C#'s
    // codegen for the yield keyword, and arguably they didn't protect much.
    // Instead, they penalized people (and compilers) for writing threadsafe
    // code.
    public static class Interlocked
    {
        /******************************
         * Increment
         *   Implemented: int
         *                        long
         *****************************/

        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern int Increment(ref int location);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern int Increment(ref long location);
        /******************************
         * Decrement
         *   Implemented: int
         *                        long
         *****************************/

        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern int Decrement(ref int location);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern long Decrement(ref long location);

        /******************************
         * Exchange
         *   Implemented: int
         *                        long
         *                        float
         *                        double
         *                        Object
         *                        IntPtr
         *****************************/

        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern int Exchange(ref int location1, int value);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern long Exchange(ref long location1, long value);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern object Exchange(ref object location1, object value);

        //[MethodImpl(MethodImplOptions.InternalCall)]
        //public static extern int Exchange(ref IntPtr location1, IntPtr value);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern T Exchange<T>(ref T location1, T value) where T : class;
        /******************************
         * CompareExchange
         *    Implemented: int
         *                         long
         *                         float
         *                         double
         *                         Object
         *                         IntPtr
         *****************************/

        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern int CompareExchange(ref int location1, int value, int comparand);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern long CompareExchange(ref long location1, long value, long comparand);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern object CompareExchange(ref object location1, object value, object comparand);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern IntPtr CompareExchange(ref IntPtr location1, IntPtr value, IntPtr comparand);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern T CompareExchange<T>(ref T location1, T value, T comparand) where T : class;

        // BCL-internal overload that returns success via a ref bool param, useful for reliable spin locks.
        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        internal static extern int CompareExchange(ref int location1, int value, int comparand, ref bool succeeded);

        /******************************
         * Add
         *    Implemented: int
         *                         long
         *****************************/

        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        internal static extern int ExchangeAdd(ref int location1, int value);

        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        internal static extern long ExchangeAdd(ref long location1, long value);

        public static int Add(ref int location1, int value)
        {
            return ExchangeAdd(ref location1, value) + value;
        }

        public static long Add(ref long location1, long value)
        {
            return ExchangeAdd(ref location1, value) + value;
        }

        /******************************
         * Read
         *****************************/
        public static long Read(ref long location)
        {
            return Interlocked.CompareExchange(ref location, 0, 0);
        }

        public static void MemoryBarrier()
        {
            Thread.MemoryBarrier();
        }
    }
}


