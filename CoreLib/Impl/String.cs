namespace System
{
    using Runtime.CompilerServices;

    [MergeCode]
    public partial class String
    {
        [MergeCode]
        public int Length
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return m_stringLength;
            }
        }
    }
}