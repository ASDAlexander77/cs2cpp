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

        public static extern int Increment(ref int location);

        public static extern int Increment(ref long location);
        /******************************
         * Decrement
         *   Implemented: int
         *                        long
         *****************************/

        
        public static extern int Decrement(ref int location);
        
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

        public static extern int Exchange(ref int location1, int value);

        public static extern int Exchange(ref long location1, long value);

        public static extern int Exchange(ref float location1, float value);

        public static extern int Exchange(ref double location1, double value);

        public static extern int Exchange(ref object location1, object value);

        //public static extern int Exchange(ref IntPtr location1, IntPtr value);

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

        
        public static extern int CompareExchange(ref int location1, int value, int comparand);

        public static extern long CompareExchange(ref long location1, long value, long comparand);

        public static extern float CompareExchange(ref float location1, float value, float comparand);

        public static extern double CompareExchange(ref double location1, double value, double comparand);

        public static extern object CompareExchange(ref object location1, object value, object comparand);

        //public static extern IntPtr CompareExchange(ref IntPtr location1, IntPtr value, IntPtr comparand);

        public static extern T CompareExchange<T>(ref T location1, T value, T comparand) where T : class;
    }
}


