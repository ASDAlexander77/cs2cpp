namespace System
{
    using Runtime.CompilerServices;

    [MergeCode]
    public static partial class Environment
    {
        [MergeCode]
        private const int CLOCK_MONOTONIC = 1;

        [MethodImplAttribute(MethodImplOptions.Unmanaged)]
        [MergeCode]
        public static extern unsafe int clock_gettime(int type, int* time);

        [MethodImpl(MethodImplOptions.Unmanaged)]
        [MergeCode]
        public static extern void exit(int exitCode);

        [MergeCode]
        internal static String GetResourceFromDefault(String key)
        {
            return key;
        }

        public static int TickCount
        {
            [MergeCode]
            get
            {
                unsafe
                {
                    long time;
                    if (clock_gettime(CLOCK_MONOTONIC, (int*)&time) == 0)
                    {
                        return (int)time >> 32;
                    }

                    throw new Exception();
                }
            }
        }

        public static int ExitCode
        {
            [MergeCode]
            get;
            [MergeCode]
            set;
        }

        // Terminates this process with the given exit code.
        [MergeCode]
        internal static void _Exit(int exitCode)
        {
            exit(exitCode);
        }
    }
}
