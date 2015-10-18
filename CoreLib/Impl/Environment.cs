namespace System
{
    using Runtime.CompilerServices;

    public static partial class Environment
    {
        private const int CLOCK_MONOTONIC = 1;

        private static int _exitCode;

        [MethodImplAttribute(MethodImplOptions.Unmanaged)]
        public static extern unsafe int clock_gettime(int type, int* time);

        [MethodImpl(MethodImplOptions.Unmanaged)]
        public static extern void exit(int exitCode);

        internal static String GetResourceFromDefault(String key)
        {
            return key;
        }

        public static int TickCount
        {
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
            get { return _exitCode; }
            set { _exitCode = value; }
        }

        // Terminates this process with the given exit code.
        internal static void _Exit(int exitCode)
        {
            exit(exitCode);
        }
    }
}
