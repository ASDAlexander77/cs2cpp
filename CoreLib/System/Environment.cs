namespace System
{
    using System.Diagnostics.Contracts;

    using Runtime.CompilerServices;
    using Runtime.InteropServices;
    using Security;

    public static partial class Environment
    {
        public static string NewLine = "\r\n";

        public static string Space = " ";

        private const long TicksPerMillisecond = 10000;

        private const long TicksPerSecond = TicksPerMillisecond * 1000;

        public static string CurrentDirectory { get; set; }

        private static OperatingSystem _os;

        [SecurityCritical]
        [DllImport(JitHelpers.QCall, CharSet = CharSet.Unicode)]
        [SuppressUnmanagedCodeSecurity]
        private static extern Int32 GetProcessorCount();

        public static int ProcessorCount
        {
            [System.Security.SecuritySafeCritical]  // auto-generated
            get
            {
                return GetProcessorCount();
            }
        }

        public static OperatingSystem OSVersion
        {
            [System.Security.SecuritySafeCritical]  // auto-generated
            get
            {
                Contract.Ensures(Contract.Result<OperatingSystem>() != null);

                if (_os == null)
                { // We avoid the lock since we don't care if two threads will set this at the same time.
#if PLATFORM_UNIX
                    PlatformID id = PlatformID.Unix;
#else
                    PlatformID id = PlatformID.Win32NT;
#endif // PLATFORM_UNIX

                    var v = new Version(6, 1, 7600);
                    _os = new OperatingSystem(id, v);
                }

                Contract.Assert(_os != null, "m_os != null");
                return _os;
            }
        }
        
        public static bool HasShutdownStarted { get; set; }

        public static bool IsWindows8OrAbove { get; set; }

        /*==================================TickCount===================================
        **Action: Gets the number of ticks since the system was started.
        **Returns: The number of ticks since the system was started.
        **Arguments: None
        **Exceptions: None
        ==============================================================================*/
        public static extern int TickCount
        {
            [System.Security.SecuritySafeCritical]  // auto-generated
            [MethodImplAttribute(MethodImplOptions.InternalCall)]
            get;
        }

        // Terminates this process with the given exit code.
        [System.Security.SecurityCritical]  // auto-generated
        [DllImport(JitHelpers.QCall, CharSet = CharSet.Unicode)]
        [SuppressUnmanagedCodeSecurity]
        internal static extern void _Exit(int exitCode);


        public static void Exit(int exitCode)
        {
            _Exit(exitCode);
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

        public static string GetResourceString(string name, object value)
        {
            return name + Space + value;
        }

        public static string GetResourceString(string name, object value, object value2)
        {
            return name + Space + value + Space + value2;
        }

        public static string GetResourceString(string name, string value, string value2)
        {
            return name + Space + value + Space + value2;
        }

        public static string GetResourceString(string name, string value, string value2, string value3, string value4)
        {
            return name + Space + value + Space + value2 + value3 + value4;
        }

        public static void FailFast(string getResourceString, Exception exception)
        {
            throw new NotImplementedException();
        }

        public static string GetEnvironmentVariable(string envVarName)
        {
            return string.Empty;
        }
    }
}
