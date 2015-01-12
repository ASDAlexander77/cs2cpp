namespace System
{
    using System;

    [Serializable]
    internal sealed class Empty
    {
        private Empty()
        {
        }

        public static readonly Empty Value = new Empty();

        public override String ToString()
        {
            return String.Empty;
        }
    }
}
