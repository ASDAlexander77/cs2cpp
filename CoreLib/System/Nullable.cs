////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Apache License 2.0 (Apache)
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
using System;

namespace System
{
    public struct Nullable<T> where T : struct
    {
        private bool hasValue;
        internal T value;

        public Nullable(T value)
        {
            this.value = value;
            this.hasValue = true;
        }

        public bool HasValue
        {
            get
            {
                return hasValue;
            }
        }

        public T Value
        {
            get
            {
                if (!HasValue)
                {
                    throw new InvalidOperationException();
                }
                return value;
            }
        }

        public T GetValueOrDefault()
        {
            return value;
        }

        public T GetValueOrDefault(T defaultValue)
        {
            return HasValue ? value : defaultValue;
        }

        public override bool Equals(object other)
        {
            if (!HasValue) return other == null;
            if (other == null) return false;
            return value.Equals(other);
        }

        public override int GetHashCode()
        {
            return HasValue ? value.GetHashCode() : 0;
        }

        public override string ToString()
        {
            return HasValue ? value.ToString() : "";
        }

        public static implicit operator Nullable<T>(T value)
        {
            return new Nullable<T>(value);
        }

        public static explicit operator T(Nullable<T> value)
        {
            return value.Value;
        }
    }
}
