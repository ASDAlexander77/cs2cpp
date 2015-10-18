namespace System.Runtime.CompilerServices {

    public static partial class RuntimeHelpers
    {
        public static void ProbeForSufficientStack()
        {
            // TODO: finish it
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

