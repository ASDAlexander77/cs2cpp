// Licensed under the MIT license.

//
/*=============================================================================
**
**
**
** Purpose: Wait(), Notify() or NotifyAll() was called from an unsynchronized
**          block of code.
**
**
=============================================================================*/

namespace System.Threading {

    using System;
[System.Runtime.InteropServices.ComVisible(true)]
    [Serializable]
    public class SynchronizationLockException : SystemException {
        public SynchronizationLockException() 
            : base(Environment.GetResourceString("Arg_SynchronizationLockException")) {
        }
    
        public SynchronizationLockException(String message) 
            : base(message) {
        }
    
        public SynchronizationLockException(String message, Exception innerException) 
            : base(message, innerException) {
        }
    }

}


