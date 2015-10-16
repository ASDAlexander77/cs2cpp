namespace System
{
    using Runtime.CompilerServices;

    [MergeCode]
    public partial class String
    {
        public int Length
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            [MergeCode]
            get
            {
                return m_stringLength;
            }
        }

        [System.Runtime.CompilerServices.IndexerName("Chars")]
        public char this[int index]
        {
            [MergeCode]
            get
            {
                if (index < 0 || index >= m_stringLength)
                {
                    throw new ArgumentOutOfRangeException("index");
                }

                unsafe
                {
                    fixed (char* p = this)
                    {
                        return *(p + index);
                    }
                }
            }
        }
    }
}