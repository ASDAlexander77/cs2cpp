// Licensed under the MIT license.

namespace System.Threading
{
    using System.Runtime.ConstrainedExecution;
    using System.Runtime.InteropServices;

    [MergeCode]
    partial class ThreadPool
    {
        [MergeCode]
        private static bool SetMinThreadsNative(int workerThreads, int completionPortThreads)
        {
            throw new NotImplementedException();
        }

        [MergeCode]
        private static bool SetMaxThreadsNative(int workerThreads, int completionPortThreads)
        {
            throw new NotImplementedException();
        }

        [MergeCode]
        private static void GetMinThreadsNative(out int workerThreads, out int completionPortThreads)
        {
            throw new NotImplementedException();
        }

        [MergeCode]
        private static void GetMaxThreadsNative(out int workerThreads, out int completionPortThreads)
        {
            throw new NotImplementedException();
        }

        [MergeCode]
        private static void GetAvailableThreadsNative(out int workerThreads, out int completionPortThreads)
        {
            throw new NotImplementedException();
        }

        [MergeCode]
        internal static bool NotifyWorkItemComplete()
        {
            throw new NotImplementedException();
        }

        [MergeCode]
        internal static void ReportThreadStatus(bool isWorking)
        {
            throw new NotImplementedException();
        }

        [MergeCode]
        internal static void NotifyWorkItemProgressNative()
        {
            throw new NotImplementedException();
        }

        [MergeCode]
        internal static bool IsThreadPoolHosted()
        {
            return false;
        }

        [MergeCode]
        private static void InitializeVMTp(ref bool enableWorkerTracking)
        {
            throw new NotImplementedException();
        }

        [MergeCode]
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

        [MergeCode]
        private static bool BindIOCompletionCallbackNative(IntPtr fileHandle)
        {
            throw new NotImplementedException();
        }

        [MergeCode]
        internal static bool RequestWorkerThread()
        {
            throw new NotImplementedException();
        }

        [MergeCode]
        unsafe private static bool PostQueuedCompletionStatus(NativeOverlapped* overlapped)
        {
            throw new NotImplementedException();
        }
    }

    [MergeCode]
    internal partial class RegisteredWaitHandleSafe : CriticalFinalizerObject
    {
        [MergeCode]
        private static void WaitHandleCleanupNative(IntPtr handle)
        {
            throw new NotImplementedException();
        }

        [MergeCode]
        private static bool UnregisterWaitNative(IntPtr handle, SafeHandle waitObject)
        {
            throw new NotImplementedException();
        }
    }
}
