// ==++==
namespace System.IO
{
    using System;
    using Runtime.InteropServices;

    [Serializable]
    [ComVisible(true)]
    public class EndOfStreamException : IOException
    {
        public EndOfStreamException()
            : base("_EndOfStreamException")
        {
        }

        public EndOfStreamException(String message)
            : base(message)
        {
        }

        public EndOfStreamException(String message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
