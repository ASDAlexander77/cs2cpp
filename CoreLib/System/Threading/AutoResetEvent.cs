// Licensed under the MIT license.

namespace System.Threading
{

    using System;
    using System.Security.Permissions;
    using System.Runtime.InteropServices;

#if PROTECTION
#if PROTECTION
[HostProtection(Synchronization=true, ExternalThreading=true)]
#endif
#endif
    [System.Runtime.InteropServices.ComVisible(true)]
    public sealed class AutoResetEvent : EventWaitHandle
    {
        public AutoResetEvent(bool initialState) : base(initialState, EventResetMode.AutoReset) { }
    }
}

