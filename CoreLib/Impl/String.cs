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
    }
}