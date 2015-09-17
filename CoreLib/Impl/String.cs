namespace System
{
    using Runtime.CompilerServices;

    public partial class String
    {
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