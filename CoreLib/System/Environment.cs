namespace System
{
    using Runtime.CompilerServices;

    public static class Environment
    {
        [MethodImplAttribute(MethodImplOptions.Unmanaged)]
        public static extern unsafe int clock_gettime(int type, long* time);

        [MethodImpl(MethodImplOptions.Unmanaged)]
        public static extern int exit(int exitCode);

        public static string NewLine = "\r\n";

        public static string Space = " ";

        private const long TicksPerMillisecond = 10000;

        private const long TicksPerSecond = TicksPerMillisecond * 1000;

        private const int CLOCK_MONOTONIC = 1;

        public static string CurrentDirectory { get; set; }

        public static int ExitCode { get; set; }

        public static int TickCount
        {
            get
            {
                unsafe
                {
                    long time;
                    if (clock_gettime(CLOCK_MONOTONIC, &time) == 0)
                    {
                        return (int) time >> 32;
                    }

                    throw new Exception();
                }
            }
        }

        public static string GetResourceString(string name)
        {
            return name;
        }

        public static string GetResourceString(string name, string value)
        {
            return name + Space + value;
        }

        public static string GetResourceString(string name, int value)
        {
            return name + Space + value;
        }

        public static string GetResourceString(string name, string value, string value2)
        {
            return name + Space + value + Space + value2;
        }

        public static void Exit(int exitCode)
        {
            exit(exitCode);
        }
    }
}
