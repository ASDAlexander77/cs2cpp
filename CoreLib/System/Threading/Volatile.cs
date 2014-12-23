namespace System.Threading
{
    using System;
    using System.Runtime.InteropServices;
    using System.Runtime.Versioning;
    using System.Runtime.CompilerServices;
    using System.Runtime;

    public static class Volatile
    {
        public static int Read(ref int location)
        {
            return Interlocked.CompareExchange(ref location, 0, 0);
        }

        public static long Read(ref long location)
        {
            return Interlocked.CompareExchange(ref location, 0, 0);
        }

        public static ulong Read(ref ulong location)
        {
            unsafe
            {
                fixed (ulong* pLocation = &location)
                {
                    return (ulong)Interlocked.CompareExchange(ref *(long*)pLocation, 0, 0);
                }
            }
        }

        public static float Read(ref float location)
        {
            return Interlocked.CompareExchange(ref location, 0, 0);
        }

        public static double Read(ref double location)
        {
            return Interlocked.CompareExchange(ref location, 0, 0);
        }

        public static T Read<T>(ref T location) where T : class
        {
            return Interlocked.CompareExchange(ref location, default(T), default(T));
        }

        public static void Write(ref int location, int value)
        {
            Interlocked.Exchange(ref location, value);
        }

        public static void Write(ref long location, long value)
        {
            Interlocked.Exchange(ref location, value);
        }

        public static void Write(ref ulong location, ulong value)
        {
            unsafe
            {
                fixed (ulong* pLocation = &location)
                {
                    Interlocked.Exchange(ref *(long*)pLocation, (long)value);
                }
            }
        }

        public static void Write(ref float location, float value)
        {
            Interlocked.Exchange(ref location, value);
        }

        public static void Write(ref double location, double value)
        {
            Interlocked.Exchange(ref location, value);
        }

        public static void Write<T>(ref T location, T value) where T : class
        {
            Interlocked.Exchange<T>(ref location, value);
        }
    }
}
