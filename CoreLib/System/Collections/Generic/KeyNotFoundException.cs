namespace System.Collections.Generic
{
    using System;

    public class KeyNotFoundException : SystemException
    {

        public KeyNotFoundException()
            : base("KeyNotFound")
        {
        }

        public KeyNotFoundException(String message)
            : base(message)
        {
        }

        public KeyNotFoundException(String message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
