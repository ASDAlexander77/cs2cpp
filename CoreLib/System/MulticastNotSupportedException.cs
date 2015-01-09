namespace System
{
    using System;
    [Serializable()]
    public sealed class MulticastNotSupportedException : SystemException
    {

        public MulticastNotSupportedException()
        {
        }

        public MulticastNotSupportedException(String message)
            : base(message)
        {
        }

        public MulticastNotSupportedException(String message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
