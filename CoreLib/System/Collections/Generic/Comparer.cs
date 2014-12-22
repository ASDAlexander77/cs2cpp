namespace System.Collections.Generic
{
    [Serializable]
    public abstract class Comparer<T> : IComparer, IComparer<T>
    {
        static volatile Comparer<T> defaultComparer;

        public static Comparer<T> Default
        {
            get
            {
                Comparer<T> comparer = defaultComparer;
                if (comparer == null)
                {
                    comparer = CreateComparer();
                    defaultComparer = comparer;
                }
                return comparer;
            }
        }

        public static Comparer<T> Create(Comparison<T> comparison)
        {
            if (comparison == null)
                throw new ArgumentNullException("comparison");

            return new ComparisonComparer<T>(comparison);
        }

        private static Comparer<T> CreateComparer()
        {
            var t = typeof(T);
            if (t == typeof(byte))
            {
                return (Comparer<T>)(object)(new ByteComparer());
            }
            if (t == typeof(int))
            {
                return (Comparer<T>)(object)(new IntComparer());
            }
            if (t == typeof(long))
            {
                return (Comparer<T>)(object)(new LongComparer());
            }

            return new ObjectComparer<T>();
        }

        public abstract int Compare(T x, T y);

        int IComparer.Compare(object x, object y)
        {
            if (x == null) return y == null ? 0 : -1;
            if (y == null) return 1;
            if (x is T && y is T) return Compare((T)x, (T)y);
            throw new ArgumentException("InvalidArgumentForComparison");
        }
    }

    [Serializable]
    internal class GenericComparer<T> : Comparer<T> where T : IComparable<T>
    {
        public override int Compare(T x, T y)
        {
            if (x != null)
            {
                if (y != null) return x.CompareTo(y);
                return 1;
            }
            if (y != null) return -1;
            return 0;
        }

        public override bool Equals(Object obj)
        {
            GenericComparer<T> comparer = obj as GenericComparer<T>;
            return comparer != null;
        }

        public override int GetHashCode()
        {
            return this.GetType().Name.GetHashCode();
        }
    }

    [Serializable]
    internal class ByteComparer : Comparer<byte>
    {
        public override int Compare(byte x, byte y)
        {
            if (x == y)
            {
                return 0;
            }

            return x < y ? -1 : 1;
        }

        // Equals method for the comparer itself. 
        public override bool Equals(Object obj)
        {
            ObjectComparer<byte> comparer = obj as ObjectComparer<byte>;
            return comparer != null;
        }

        public override int GetHashCode()
        {
            return this.GetType().Name.GetHashCode();
        }
    }

    [Serializable]
    internal class IntComparer : Comparer<int>
    {
        public override int Compare(int x, int y)
        {
            if (x == y)
            {
                return 0;
            }

            return x < y ? -1 : 1;
        }

        // Equals method for the comparer itself. 
        public override bool Equals(Object obj)
        {
            ObjectComparer<int> comparer = obj as ObjectComparer<int>;
            return comparer != null;
        }

        public override int GetHashCode()
        {
            return this.GetType().Name.GetHashCode();
        }
    }

    [Serializable]
    internal class LongComparer: Comparer<long>
    {
        public override int Compare(long x, long y)
        {
            if (x == y)
            {
                return 0;
            }

            return x < y ? -1 : 1;
        }

        // Equals method for the comparer itself. 
        public override bool Equals(Object obj)
        {
            ObjectComparer<long> comparer = obj as ObjectComparer<long>;
            return comparer != null;
        }

        public override int GetHashCode()
        {
            return this.GetType().Name.GetHashCode();
        }
    }

    [Serializable]
    internal class ObjectComparer<T> : Comparer<T>
    {
        public override int Compare(T x, T y)
        {
            return System.Collections.Comparer.Default.Compare(x, y);
        }

        // Equals method for the comparer itself. 
        public override bool Equals(Object obj)
        {
            ObjectComparer<T> comparer = obj as ObjectComparer<T>;
            return comparer != null;
        }

        public override int GetHashCode()
        {
            return this.GetType().Name.GetHashCode();
        }
    }

    [Serializable]
    internal class ComparisonComparer<T> : Comparer<T>
    {
        private readonly Comparison<T> _comparison;

        public ComparisonComparer(Comparison<T> comparison)
        {
            _comparison = comparison;
        }

        public override int Compare(T x, T y)
        {
            return _comparison(x, y);
        }
    }
}
