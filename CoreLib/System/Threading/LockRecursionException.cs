// Licensed under the MIT license.

/*============================================================
//
//
//
// Purpose: 
// This exception represents a failed attempt to recursively
// acquire a lock, because the particular lock kind doesn't
// support it in its current state.
============================================================*/

namespace System.Threading
{
    using System;

    [Serializable]
    public class LockRecursionException : System.Exception
    {
        public LockRecursionException() { }
        public LockRecursionException(string message) : base(message) { }
        public LockRecursionException(string message, Exception innerException) : base(message, innerException) { }
    }
}
