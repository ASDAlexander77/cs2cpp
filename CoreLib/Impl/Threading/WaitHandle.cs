namespace System.Threading
{
    using System.Runtime.InteropServices;

    using Microsoft.Win32.SafeHandles;

    [MergeCode]
    partial class WaitHandle
    {
        [MergeCode]
        private static int WaitOneNative(SafeHandle waitableSafeHandle, uint millisecondsTimeout, bool hasThreadAffinity, bool exitContext)
        {
            var acquiredLock = false;

            try
            {
                if ((int)millisecondsTimeout != Timeout.Infinite)
                {
                    Monitor.TryEnter(waitableSafeHandle, (int)millisecondsTimeout, ref acquiredLock);
                }
                else
                {
                    Monitor.Enter(waitableSafeHandle, ref acquiredLock);
                }

                unsafe
                {
                    var waitHandleData = (int*)waitableSafeHandle.DangerousGetHandle().ToPointer();
                    if (waitHandleData == null)
                    {
                        throw new InvalidOperationException();
                    }

                    var @continue = waitHandleData[0] == 1; 
                    // state = true
                    while (waitHandleData[0] == 0 && (@continue = Monitor.Wait(waitableSafeHandle, (int)millisecondsTimeout, exitContext)))
                    {
                        // empty
                    }

                    // if AutoReset
                    if (@continue && waitHandleData[1] > 0)
                    {
                        waitHandleData[0] = 0;
                    }

                    return @continue && acquiredLock ? 0 : WaitTimeout;
                }

            }
            finally
            {
                if (acquiredLock)
                {
                    Monitor.Exit(waitableSafeHandle);
                }
            }
        }

        [MergeCode]
        private static int SignalAndWaitOne(
            SafeWaitHandle waitHandleToSignal, SafeWaitHandle waitHandleToWaitOn, int millisecondsTimeout, bool hasThreadAffinity, bool exitContext)
        {
            var acquiredLock = false;

            try
            {
                Monitor.Enter(waitHandleToSignal, ref acquiredLock);

                // Code that accesses resources that are protected by the lock.
                Monitor.Pulse(waitHandleToSignal);
            }
            finally
            {
                if (acquiredLock)
                {
                    Monitor.Exit(waitHandleToSignal);
                }
            }

            acquiredLock = false;

            try
            {
                Monitor.Enter(waitHandleToWaitOn, ref acquiredLock);

                // Code that accesses resources that are protected by the lock.
                return Monitor.Wait(waitHandleToWaitOn, (int)millisecondsTimeout, exitContext) ? 0 : WaitTimeout;
            }
            finally
            {
                if (acquiredLock)
                {
                    Monitor.Exit(waitHandleToWaitOn);
                }
            }
        }

        /*========================================================================
        ** Waits for signal from all the objects. 
        ** timeout indicates how long to wait before the method returns.
        ** This method will return either when all the object have been pulsed
        ** or timeout milliseonds have elapsed.
        ** If exitContext is true then the synchronization domain for the context 
        ** (if in a synchronized context) is exited before the wait and reacquired 
        ========================================================================*/

        [MergeCode]
        private static int WaitMultiple(WaitHandle[] waitHandles, int millisecondsTimeout, bool exitContext, bool WaitAll)
        {
            // TODO: import code from https://github.com/neosmart/pevents/blob/master/pevents.cpp
            throw new NotImplementedException();
        }
    }
}
