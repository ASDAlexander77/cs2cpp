////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Apache License 2.0 (Apache)
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
namespace System.Collections.Generic
{
    using System;
    using System.Collections;

    [Serializable]
    public abstract class EqualityComparer<T> : IEqualityComparer, IEqualityComparer<T>
    {
        static volatile EqualityComparer<T> defaultComparer;

        public static EqualityComparer<T> Default
        {
            get
            {
                EqualityComparer<T> comparer = defaultComparer;
                if (comparer == null)
                {
                    comparer = CreateComparer();
                    defaultComparer = comparer;
                }
                return comparer;
            }
        }

        private static EqualityComparer<T> CreateComparer()
        {
            var t = typeof(T);
            if (t == typeof(byte))
            {
                return (EqualityComparer<T>)(object)(new ByteEqualityComparer());
            }
            if (t == typeof(int))
            {
                return (EqualityComparer<T>)(object)(new IntEqualityComparer());
            }
            if (t == typeof(long))
            {
                return (EqualityComparer<T>)(object)(new LongEqualityComparer());
            }

            return new ObjectEqualityComparer<T>();
        }

        public abstract bool Equals(T x, T y);

        public abstract int GetHashCode(T obj);

        internal virtual int IndexOf(T[] array, T value, int startIndex, int count)
        {
            int endIndex = startIndex + count;
            for (int i = startIndex; i < endIndex; i++)
            {
                if (Equals(array[i], value)) return i;
            }
            return -1;
        }

        internal virtual int LastIndexOf(T[] array, T value, int startIndex, int count)
        {
            int endIndex = startIndex - count + 1;
            for (int i = startIndex; i >= endIndex; i--)
            {
                if (Equals(array[i], value)) return i;
            }
            return -1;
        }

        int IEqualityComparer.GetHashCode(object obj)
        {
            if (obj == null) return 0;
            if (obj is T) return GetHashCode((T)obj);
            throw new ArgumentException("InvalidArgumentForComparison");
        }

        bool IEqualityComparer.Equals(object x, object y)
        {
            if (x == y) return true;
            if (x == null || y == null) return false;
            if ((x is T) && (y is T)) return Equals((T)x, (T)y);
            throw new ArgumentException("InvalidArgumentForComparison");
        }
    }

    [Serializable]
    internal class ObjectEqualityComparer<T> : EqualityComparer<T>
    {
        public override bool Equals(T x, T y)
        {
            if (x != null)
            {
                if (y != null) return x.Equals(y);
                return false;
            }
            if (y != null) return false;
            return true;
        }

        public override int GetHashCode(T obj)
        {
            if (obj == null) return 0;
            return obj.GetHashCode();
        }

        internal override int IndexOf(T[] array, T value, int startIndex, int count)
        {
            int endIndex = startIndex + count;
            if (value == null)
            {
                for (int i = startIndex; i < endIndex; i++)
                {
                    if (array[i] == null) return i;
                }
            }
            else
            {
                for (int i = startIndex; i < endIndex; i++)
                {
                    if (array[i] != null && array[i].Equals(value)) return i;
                }
            }
            return -1;
        }

        internal override int LastIndexOf(T[] array, T value, int startIndex, int count)
        {
            int endIndex = startIndex - count + 1;
            if (value == null)
            {
                for (int i = startIndex; i >= endIndex; i--)
                {
                    if (array[i] == null) return i;
                }
            }
            else
            {
                for (int i = startIndex; i >= endIndex; i--)
                {
                    if (array[i] != null && array[i].Equals(value)) return i;
                }
            }
            return -1;
        }

        // Equals method for the comparer itself. 
        public override bool Equals(Object obj)
        {
            ObjectEqualityComparer<T> comparer = obj as ObjectEqualityComparer<T>;
            return comparer != null;
        }

        public override int GetHashCode()
        {
            return this.GetType().Name.GetHashCode();
        }
    }

    [Serializable]
    internal sealed class ByteEqualityComparer : EqualityComparer<byte>
    {
        public override bool Equals(byte x, byte y)
        {
            return x == y;
        }

        public override int GetHashCode(byte obj)
        {
            return obj.GetHashCode();
        }

        // Equals method for the comparer itself. 
        public override bool Equals(Object obj)
        {
            ByteEqualityComparer comparer = obj as ByteEqualityComparer;
            return comparer != null;
        }

        public override int GetHashCode()
        {
            return this.GetType().Name.GetHashCode();
        }
    }

    [Serializable]
    internal sealed class IntEqualityComparer : EqualityComparer<int>
    {
        public override bool Equals(int x, int y)
        {
            return x == y;
        }

        public override int GetHashCode(int obj)
        {
            return obj.GetHashCode();
        }

        // Equals method for the comparer itself. 
        public override bool Equals(Object obj)
        {
            IntEqualityComparer comparer = obj as IntEqualityComparer;
            return comparer != null;
        }

        public override int GetHashCode()
        {
            return this.GetType().Name.GetHashCode();
        }
    }

    [Serializable]
    internal sealed class LongEqualityComparer : EqualityComparer<long>
    {
        public override bool Equals(long x, long y)
        {
            return x == y;
        }

        public override int GetHashCode(long obj)
        {
            return obj.GetHashCode();
        }

        // Equals method for the comparer itself. 
        public override bool Equals(Object obj)
        {
            LongEqualityComparer comparer = obj as LongEqualityComparer;
            return comparer != null;
        }

        public override int GetHashCode()
        {
            return this.GetType().Name.GetHashCode();
        }
    }
}

