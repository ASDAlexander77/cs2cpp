namespace System
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;


    [Serializable]
    [System.Runtime.InteropServices.ComVisible(true)]
    public abstract class StringComparer : IComparer, IEqualityComparer, IComparer<string>, IEqualityComparer<string>
    {
        private static readonly StringComparer _ordinal = new OrdinalComparer(false);
        private static readonly StringComparer _ordinalIgnoreCase = new OrdinalComparer(true);

        public static StringComparer Ordinal
        {
            get
            {
                return _ordinal;
            }
        }

        public static StringComparer OrdinalIgnoreCase
        {
            get
            {
                return _ordinalIgnoreCase;
            }
        }

        public int Compare(object x, object y)
        {
            if (x == y) return 0;
            if (x == null) return -1;
            if (y == null) return 1;

            String sa = x as String;
            if (sa != null)
            {
                String sb = y as String;
                if (sb != null)
                {
                    return Compare(sa, sb);
                }
            }

            IComparable ia = x as IComparable;
            if (ia != null)
            {
                return ia.CompareTo(y);
            }

            throw new ArgumentException("ImplementIComparable");
        }


        public new bool Equals(Object x, Object y)
        {
            if (x == y) return true;
            if (x == null || y == null) return false;

            String sa = x as String;
            if (sa != null)
            {
                String sb = y as String;
                if (sb != null)
                {
                    return Equals(sa, sb);
                }
            }
            return x.Equals(y);
        }

        public int GetHashCode(object obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("obj");
            }

            string s = obj as string;
            if (s != null)
            {
                return GetHashCode(s);
            }
            return obj.GetHashCode();
        }

        public abstract int Compare(String x, String y);
        public abstract bool Equals(String x, String y);
        public abstract int GetHashCode(string obj);
    }

    // Provide x more optimal implementation of ordinal comparison.
    [Serializable]
    internal sealed class OrdinalComparer : StringComparer
    {
        private bool _ignoreCase;

        internal OrdinalComparer(bool ignoreCase)
        {
            _ignoreCase = ignoreCase;
        }

        public override int Compare(string x, string y)
        {
            if (Object.ReferenceEquals(x, y)) return 0;
            if (x == null) return -1;
            if (y == null) return 1;

            if (_ignoreCase)
            {
                throw new InvalidOperationException();
            }

            return String.Compare(x, y);
        }

        public override bool Equals(string x, string y)
        {
            if (Object.ReferenceEquals(x, y)) return true;
            if (x == null || y == null) return false;

            if (_ignoreCase)
            {
                if (x.Length != y.Length)
                {
                    return false;
                }
                return String.Compare(x, y) == 0;
            }
            return x.Equals(y);
        }

        public override int GetHashCode(string obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("obj");
            }

            if (_ignoreCase)
            {
                throw new InvalidOperationException();
            }

            return obj.GetHashCode();
        }

        // Equals method for the comparer itself. 
        public override bool Equals(Object obj)
        {
            OrdinalComparer comparer = obj as OrdinalComparer;
            if (comparer == null)
            {
                return false;
            }
            return (this._ignoreCase == comparer._ignoreCase);
        }

        public override int GetHashCode()
        {
            string name = "OrdinalComparer";
            int hashCode = name.GetHashCode();
            return _ignoreCase ? (~hashCode) : hashCode;
        }
    }
}
