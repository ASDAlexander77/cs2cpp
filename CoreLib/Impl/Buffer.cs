namespace System
{
    using Runtime.CompilerServices;

    [MergeCode]
    public static partial class Buffer
    {
        [MergeCode]
        public static void BlockCopy(Array src, int srcOffset,
            Array dst, int dstOffset, int count)
        {
            throw new NotImplementedException();
        }

        [MergeCode]
        internal static void InternalBlockCopy(Array src, int srcOffsetBytes,
            Array dst, int dstOffsetBytes, int byteCount)
        {
            byte[] srcBytes = (byte[])src;
            byte[] dstBytes = (byte[])dst;
            Memcpy(dstBytes, dstOffsetBytes, srcBytes, srcOffsetBytes, byteCount);
        }

        [MethodImpl(MethodImplOptions.Unmanaged)]
        [MergeCode]
#if WIN64
        extern private unsafe static void __Memmove(byte* dest, byte* src, ulong len);
#else
        extern private static unsafe void __Memmove(byte* dest, byte* src, uint len);
#endif

        [MergeCode]
        internal unsafe static void Memcpy(byte[] dest, int destIndex, byte[] src, int srcIndex, int len)
        {
            // If dest has 0 elements, the fixed statement will throw an 
            // IndexOutOfRangeException.  Special-case 0-byte copies.
            if (len == 0)
                return;
            fixed (byte* pSrc = &src[0])
            fixed (byte* pDest = &dest[0])
            {
                Memcpy(pDest + destIndex, pSrc + srcIndex, len);
            }
        }
    }
}
