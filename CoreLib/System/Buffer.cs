namespace System
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Runtime.Versioning;

    public static partial class Buffer
    {
        internal unsafe static int IndexOfByte(byte* src, byte value, int index, int count)
        {
            byte* pByte = src + index;

            // Align up the pointer to sizeof(int).
            while (((int)pByte & 3) != 0)
            {
                if (count == 0)
                    return -1;
                else if (*pByte == value)
                    return (int)(pByte - src);

                count--;
                pByte++;
            }

            // Fill comparer with value byte for comparisons
            //
            // comparer = 0/0/value/value
            uint comparer = (((uint)value << 8) + (uint)value);
            // comparer = value/value/value/value
            comparer = (comparer << 16) + comparer;

            // Run through buffer until we hit a 4-byte section which contains
            // the byte we're looking for or until we exhaust the buffer.
            while (count > 3)
            {
                // Test the buffer for presence of value. comparer contains the byte
                // replicated 4 times.
                uint t1 = *(uint*)pByte;
                t1 = t1 ^ comparer;
                uint t2 = 0x7efefeff + t1;
                t1 = t1 ^ 0xffffffff;
                t1 = t1 ^ t2;
                t1 = t1 & 0x81010100;

                // if t1 is zero then these 4-bytes don't contain a match
                if (t1 != 0)
                {
                    // We've found a match for value, figure out which position it's in.
                    int foundIndex = (int)(pByte - src);
                    if (pByte[0] == value)
                        return foundIndex;
                    else if (pByte[1] == value)
                        return foundIndex + 1;
                    else if (pByte[2] == value)
                        return foundIndex + 2;
                    else if (pByte[3] == value)
                        return foundIndex + 3;
                }

                count -= 4;
                pByte += 4;

            }

            // Catch any bytes that might be left at the tail of the buffer
            while (count > 0)
            {
                if (*pByte == value)
                    return (int)(pByte - src);

                count--;
                pByte++;
            }

            // If we don't have a match return -1;
            return -1;
        }

        private static bool IsPrimitiveTypeArray(Array array)
        {
            throw new NotImplementedException();
        }

        private static byte _GetByte(Array array, int index)
        {
            throw new NotImplementedException();
        }

        public static byte GetByte(Array array, int index)
        {
            // Is the array present?
            if (array == null)
                throw new ArgumentNullException("array");

            // Is it of primitive types?
            if (!IsPrimitiveTypeArray(array))
                throw new ArgumentException("MustBePrimArray");

            // Is the index in valid range of the array?
            if (index < 0 || index >= _ByteLength(array))
                throw new ArgumentOutOfRangeException("index");

            return _GetByte(array, index);
        }

        private static void _SetByte(Array array, int index, byte value)
        {
            throw new NotImplementedException();
        }

        public static void SetByte(Array array, int index, byte value)
        {
            // Is the array present?
            if (array == null)
                throw new ArgumentNullException("array");

            // Is it of primitive types?
            if (!IsPrimitiveTypeArray(array))
                throw new ArgumentException("MustBePrimArray");

            // Is the index in valid range of the array?
            if (index < 0 || index >= _ByteLength(array))
                throw new ArgumentOutOfRangeException("index");

            // Make the FCall to do the work
            _SetByte(array, index, value);
        }

        private static int _ByteLength(Array array)
        {
            throw new NotImplementedException();
        }

        public static int ByteLength(Array array)
        {
            // Is the array present?
            if (array == null)
                throw new ArgumentNullException("array");

            // Is it of primitive types?
            if (!IsPrimitiveTypeArray(array))
                throw new ArgumentException("MustBePrimArray");

            return _ByteLength(array);
        }

        internal unsafe static void ZeroMemory(byte* src, long len)
        {
            while (len-- > 0)
                *(src + len) = 0;
        }
    }
}
