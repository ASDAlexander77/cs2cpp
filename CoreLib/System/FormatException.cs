namespace System
{
    [Serializable()]
    public class FormatException : SystemException
    {
        public FormatException()
            : base()
        {
        }

        public FormatException(String message)
            : base(message)
        {
        }

        public FormatException(String message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}


