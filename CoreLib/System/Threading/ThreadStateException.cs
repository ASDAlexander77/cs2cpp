// Licensed under the MIT license.

namespace System.Threading
{
    using System;
    [System.Runtime.InteropServices.ComVisible(true)]
    [Serializable]
    public class ThreadStateException : SystemException
    {
        public ThreadStateException()
            : base(Environment.GetResourceString("Arg_ThreadStateException"))
        {
        }

        public ThreadStateException(String message)
            : base(message)
        {
        }

        public ThreadStateException(String message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
