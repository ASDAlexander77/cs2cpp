namespace System.Runtime.CompilerServices {

    public static partial class RuntimeHelpers
    {
        public static void ProbeForSufficientStack()
        {
        }

        private static void _RunClassConstructor(RuntimeType type)
        {
            // TODO: finish it, should it execute static constructor
        }

        public static void ExecuteCodeWithGuaranteedCleanup(
            TryCode code,
            CleanupCode backoutCode,
            Object userData)
        {
            var exceptionThrown = false;
            try
            {
                code.Invoke(userData);            
            }
            catch
            {
                exceptionThrown = true;
            }

            backoutCode.Invoke(userData, exceptionThrown);
        }
    }
}

