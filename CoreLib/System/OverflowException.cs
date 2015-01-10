namespace System
{
    [Serializable]
    public class OverflowException : ArithmeticException
    {
        public OverflowException()
            : base()
        {
        }

        public OverflowException(string message)
            : base(message)
        {
        }

        public OverflowException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}


