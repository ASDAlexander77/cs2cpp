// Licensed under the MIT license.

namespace System.Threading
{
    using System.Runtime.ConstrainedExecution;
    using System.Runtime.InteropServices;

    partial class ThreadPool
    {
        private static bool SetMinThreadsNative(int workerThreads, int completionPortThreads)
        {
            throw new NotImplementedException();
        }

        private static bool SetMaxThreadsNative(int workerThreads, int completionPortThreads)
        {
            throw new NotImplementedException();
        }

        private static void GetMinThreadsNative(out int workerThreads, out int completionPortThreads)
        {
            throw new NotImplementedException();
        }

        private static void GetMaxThreadsNative(out int workerThreads, out int completionPortThreads)
        {
            throw new NotImplementedException();
        }

        private static void GetAvailableThreadsNative(out int workerThreads, out int completionPortThreads)
        {
            throw new NotImplementedException();
        }

        internal static bool NotifyWorkItemComplete()
        {
            throw new NotImplementedException();
        }

        internal static void ReportThreadStatus(bool isWorking)
        {
            throw new NotImplementedException();
        }

        internal static void NotifyWorkItemProgressNative()
        {
            throw new NotImplementedException();
        }

        internal static bool IsThreadPoolHosted()
        {
            throw new NotImplementedException();
        }

        private static void InitializeVMTp(ref bool enableWorkerTracking)
        {
            throw new NotImplementedException();
        }

        private static IntPtr RegisterWaitForSingleObjectNative(
             WaitHandle waitHandle,
             Object state,
             uint timeOutInterval,
             bool executeOnlyOnce,
             RegisteredWaitHandle registeredWaitHandle,
             ref StackCrawlMark stackMark,
             bool compressStack
             )
        {
            throw new NotImplementedException();
        }

        private static bool BindIOCompletionCallbackNative(IntPtr fileHandle)
        {
            throw new NotImplementedException();
        }

        internal static bool RequestWorkerThread()
        {
            throw new NotImplementedException();
        }

        unsafe private static bool PostQueuedCompletionStatus(NativeOverlapped* overlapped)
        {
            throw new NotImplementedException();
        }
    }

    internal partial class RegisteredWaitHandleSafe : CriticalFinalizerObject
    {
        private static void WaitHandleCleanupNative(IntPtr handle)
        {
            throw new NotImplementedException();
        }

        private static bool UnregisterWaitNative(IntPtr handle, SafeHandle waitObject)
        {
            throw new NotImplementedException();
        }
    }
}
