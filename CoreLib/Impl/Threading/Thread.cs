namespace System.Threading
{
    using System.Security.Principal;

    /// <summary>
    /// Partial implementation of Thread
    /// </summary>
    public partial class Thread
    {
        /// <summary>
        /// </summary>
        private static Thread currentThread;

        /// <summary>
        /// </summary>
        private object abortReason;

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        private static Thread GetCurrentThreadNative()
        {
            return currentThread;
        }

        /// <summary>
        /// </summary>
        public int ManagedThreadId
        {
            get
            {
                return this.m_ManagedThreadId;
            }
        }

        /// <summary>
        /// </summary>
        private void InternalFinalize()
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="principal">
        /// </param>
        /// <param name="stackMark">
        /// </param>
        private void StartInternal(IPrincipal principal, ref StackCrawlMark stackMark)
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="millisecondsTimeout">
        /// </param>
        /// <returns>
        /// </returns>
        private bool JoinInternal(int millisecondsTimeout)
        {
            return false;
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        private static AppDomain GetDomainInternal()
        {
            return AppDomain.CurrentDomain;
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        private static AppDomain GetFastDomainInternal()
        {
            return AppDomain.CurrentDomain;
        }

        // Helper function to set the AbortReason for a thread abort.
        // Checks that they're not alredy set, and then atomically updates
        // the reason info (object + ADID).
        /// <summary>
        /// </summary>
        /// <param name="o">
        /// </param>
        internal void SetAbortReason(object o)
        {
            this.abortReason = o;
        }

        // Helper function to retrieve the AbortReason from a thread
        // abort.  Will perform cross-AppDomain marshalling if the object
        // lives in a different AppDomain from the requester.
        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        internal object GetAbortReason()
        {
            return this.abortReason;
        }

        // Helper function to clear the AbortReason.  Takes care of
        // AppDomain related cleanup if required.
        /// <summary>
        /// </summary>
        internal void ClearAbortReason()
        {
            this.abortReason = null;
        }

        /// <summary>
        /// </summary>
        /// <param name="millisecondsTimeout">
        /// </param>
        private static void SleepInternal(int millisecondsTimeout)
        {
        }

        // Helper method to get a logical thread ID for StringBuilder (for
        // correctness) and for FileStream's async code path (for perf, to
        // avoid creating a Thread instance).
        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        internal static IntPtr InternalGetCurrentThread()
        {
            return new IntPtr(0);
        }

        /// <summary>
        /// </summary>
        /// <param name="t">
        /// </param>
        /// <param name="name">
        /// </param>
        /// <param name="len">
        /// </param>
        private static void InformThreadNameChange(ThreadHandle t, string name, int len)
        {
        }

#if! FEATURE_LEAK_CULTURE_INFO

        /// <summary>
        /// </summary>
        private static void nativeInitCultureAccessors()
        {
        }

#endif

        /* wait for a length of time proportial to 'iterations'.  Each iteration is should
           only take a few machine instructions.  Calling this API is preferable to coding
           a explict busy loop because the hardware can be informed that it is busy waiting. */

        /// <summary>
        /// </summary>
        /// <param name="iterations">
        /// </param>
        private static void SpinWaitInternal(int iterations)
        {
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        private static ulong GetProcessDefaultStackSize()
        {
            return 16 * 1024;
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        private static bool YieldInternal()
        {
            return false;
        }

        /// <summary>
        /// </summary>
        private void AbortInternal()
        {
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        private int GetPriorityNative()
        {
            return 0;
        }

        /// <summary>
        /// </summary>
        /// <param name="priority">
        /// </param>
        private void SetPriorityNative(int priority)
        {
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        private int GetThreadStateNative()
        {
            return (int)ThreadState.Unstarted;
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        private bool IsBackgroundNative()
        {
            return false;
        }

        /// <summary>
        /// </summary>
        /// <param name="isBackground">
        /// </param>
        private void SetBackgroundNative(bool isBackground)
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="start">
        /// </param>
        /// <param name="maxStackSize">
        /// </param>
        private void SetStart(Delegate start, int maxStackSize)
        {
        }
    }
}