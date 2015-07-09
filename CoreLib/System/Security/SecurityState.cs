// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Security;
using System.Security.Permissions;

namespace System.Security
{
    [System.Security.SecurityCritical]  // auto-generated_required
#pragma warning disable 618
    #if PROTECTION
[PermissionSet(SecurityAction.InheritanceDemand, Unrestricted = true)]
#endif
#pragma warning restore 618
    public abstract class SecurityState
    {
        protected SecurityState(){}
        
        [System.Security.SecurityCritical]  // auto-generated
        public bool IsStateAvailable()
        {
            return true;
        }
        // override this function and throw the appropriate 
        public abstract void EnsureState();
    }

}
