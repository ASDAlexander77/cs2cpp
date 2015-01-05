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
    }
}


