// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

//
/*=============================================================================
**
**
**
** Purpose: An example of a WaitHandle class
**
**
=============================================================================*/
namespace System.Threading {
    
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
        public AutoResetEvent(bool initialState) : base(initialState,EventResetMode.AutoReset){ }
    }
}
    
