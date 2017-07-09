////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Apache License 2.0 (Apache)
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
namespace System
{

    using System.Globalization;

    [Serializable]
    [System.Runtime.InteropServices.ComVisible(true)]
    public struct IntPtr
    {
        unsafe private void* _value; 

        public static readonly IntPtr Zero;

        internal unsafe bool IsNull()
        {
            return (this._value == null);
        }

        public unsafe IntPtr(int value)
        {
            _value = (void *)value;
        }

        public unsafe IntPtr(long value)
        {
            _value = (void*)value;
        }

        public unsafe IntPtr(void* value)
        {
            _value = value;
        }

        public unsafe override bool Equals(Object obj)
        {
            if (obj is IntPtr)
            {
                return (_value == ((IntPtr)obj)._value);
            }

            return false;
        }

        public unsafe override int GetHashCode()
        {
            return unchecked((int)((long)_value));
        }

        public unsafe int ToInt32()
        {
            return (int)_value;
        }

        public unsafe long ToInt64()
        {
            return (long)(int)_value;
        }

        public unsafe override String ToString()
        {
            return ((int)_value).ToString();
        }

        public unsafe String ToString(String format)
        {
            return ((int)_value).ToString(format);
        }

        public static explicit operator IntPtr(int value)
        {
            return new IntPtr(value);
        }

        public static explicit operator IntPtr(long value)
        {
            return new IntPtr(value);
        }

        public static unsafe explicit operator IntPtr(void* value)
        {
            return new IntPtr(value);
        }

        public static unsafe explicit operator void*(IntPtr value)
        {
            return value.ToPointer();
        }

        public unsafe static explicit operator int(IntPtr value)
        {
            return (int)value._value;
        }

        public unsafe static explicit operator long(IntPtr value)
        {
            return (long)(int)value._value;
        }

        public unsafe static bool operator ==(IntPtr value1, IntPtr value2)
        {
            return value1._value == value2._value;
        }

        public unsafe static bool operator !=(IntPtr value1, IntPtr value2)
        {
            return value1._value != value2._value;
        }

        public static IntPtr Add(IntPtr pointer, int offset)
        {
            return pointer + offset;
        }

        public static IntPtr operator +(IntPtr pointer, int offset)
        {
            return new IntPtr(pointer.ToInt32() + offset);
        }

        public static IntPtr Subtract(IntPtr pointer, int offset)
        {
            return pointer - offset;
        }

        public static IntPtr operator -(IntPtr pointer, int offset)
        {
            return new IntPtr(pointer.ToInt32() - offset);
        }

        public static int Size
        {
            get
            {
                unsafe
                {
                    return sizeof(void*);
                }
            }
        }

        public unsafe void* ToPointer()
        {
            return _value;
        }
    }
}




