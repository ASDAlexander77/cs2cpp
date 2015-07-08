namespace System.Threading
{
    using System.Runtime.InteropServices;

    using Microsoft.Win32.SafeHandles;

    public partial class WaitHandle
    {
        private static int WaitOneNative(SafeHandle waitableSafeHandle, uint millisecondsTimeout, bool hasThreadAffinity, bool exitContext)
        {
            return Monitor.Wait(waitableSafeHandle, (int)millisecondsTimeout, exitContext) ? 0 : WaitTimeout;
        }

        private static int SignalAndWaitOne(
            SafeWaitHandle waitHandleToSignal, SafeWaitHandle waitHandleToWaitOn, int millisecondsTimeout, bool hasThreadAffinity, bool exitContext)
        {
            Monitor.Pulse(waitHandleToSignal);
            return Monitor.Wait(waitHandleToWaitOn, (int)millisecondsTimeout, exitContext) ? 0 : WaitTimeout;
        }

        /*========================================================================
        ** Waits for signal from all the objects. 
        ** timeout indicates how long to wait before the method returns.
        ** This method will return either when all the object have been pulsed
        ** or timeout milliseonds have elapsed.
        ** If exitContext is true then the synchronization domain for the context 
        ** (if in a synchronized context) is exited before the wait and reacquired 
        ========================================================================*/

        private static int WaitMultiple(WaitHandle[] waitHandles, int millisecondsTimeout, bool exitContext, bool WaitAll)
        {
            // TODO: import code from https://github.com/neosmart/pevents/blob/master/pevents.cpp
            throw new NotImplementedException();
        }
    }
}
