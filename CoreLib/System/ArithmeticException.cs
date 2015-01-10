namespace System
{
    [Serializable]
    public class ArithmeticException : SystemException
    {
        public ArithmeticException()
            : base()
        {
        }

        public ArithmeticException(string message)
            : base(message)
        {
        }

        public ArithmeticException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}


