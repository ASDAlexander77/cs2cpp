// Licensed under the MIT license.

//
namespace System.Threading 
{
    using System;
    using System.Runtime.InteropServices;

    [Serializable]
    [ComVisibleAttribute(false)]

#if FEATURE_CORECLR
    public class WaitHandleCannotBeOpenedException : Exception {
#else
    public class WaitHandleCannotBeOpenedException : ApplicationException { 
#endif // FEATURE_CORECLR
        public WaitHandleCannotBeOpenedException() : base(Environment.GetResourceString("Threading.WaitHandleCannotBeOpenedException")) 
        {
        }
    
        public WaitHandleCannotBeOpenedException(String message) : base(message)
        {
        }

        public WaitHandleCannotBeOpenedException(String message, Exception innerException) : base(message, innerException)
        {
        }
    }
}

