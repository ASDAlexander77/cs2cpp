// Licensed under the MIT license.

namespace System.Threading
{
    [System.Runtime.InteropServices.ComVisible(true)]
    public sealed class AutoResetEvent : EventWaitHandle
    {
        public AutoResetEvent(bool initialState) : base(initialState, EventResetMode.AutoReset) { }
    }
}

