namespace System.Collections
{

    using System;
    using System.Globalization;

    [Serializable]
    public sealed class Comparer : IComparer
    {
        public static readonly Comparer Default = new Comparer();

        private Comparer()
        {
        }

        public int Compare(Object a, Object b)
        {
            if (a == b) return 0;
            if (a == null) return -1;
            if (b == null) return 1;

            IComparable ia = a as IComparable;
            if (ia != null)
                return ia.CompareTo(b);

            IComparable ib = b as IComparable;
            if (ib != null)
                return -ib.CompareTo(a);

            throw new ArgumentException("ImplementIComparable");
        }
    }
}
