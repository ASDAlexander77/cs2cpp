namespace System.Collections.Generic
{
    using System;

    public interface IComparer<T>
    {
        int Compare(T x, T y);
    }
}
