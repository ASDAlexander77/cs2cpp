namespace System
{
    using Runtime.CompilerServices;

    public static partial class Buffer
    {
        public static void BlockCopy(Array src, int srcOffset,
            Array dst, int dstOffset, int count)
        {
            throw new NotImplementedException();
        }

        internal static void InternalBlockCopy(Array src, int srcOffsetBytes,
            Array dst, int dstOffsetBytes, int byteCount)
        {
            byte[] srcBytes = (byte[])src;
            byte[] dstBytes = (byte[])dst;
            Memcpy(dstBytes, dstOffsetBytes, srcBytes, srcOffsetBytes, byteCount);
        }

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
