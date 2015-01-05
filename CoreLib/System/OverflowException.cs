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
    }
}


