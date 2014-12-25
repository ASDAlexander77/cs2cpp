namespace System
{
    [Serializable]
    [System.Runtime.InteropServices.ComVisible(true)]
    public class UnauthorizedAccessException : SystemException
    {
        public UnauthorizedAccessException()
            : base("UnauthorizedAccessException")
        {
        }

        public UnauthorizedAccessException(String message)
            : base(message)
        {
        }

        public UnauthorizedAccessException(String message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
