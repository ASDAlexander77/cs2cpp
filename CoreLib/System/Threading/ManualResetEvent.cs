// Licensed under the MIT license.

namespace System.Threading
{
    [System.Runtime.InteropServices.ComVisible(true)]
    public sealed class ManualResetEvent : EventWaitHandle
    {
        public ManualResetEvent(bool initialState) : base(initialState, EventResetMode.ManualReset) { }
    }
}

