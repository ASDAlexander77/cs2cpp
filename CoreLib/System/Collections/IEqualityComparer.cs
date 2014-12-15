namespace System.Collections
{
    using System;
    public interface IEqualityComparer
    {
        bool Equals(Object x,Object y);
        int GetHashCode(Object obj);
    }

    public interface IEqualityComparer<T>
    {
        bool Equals(T x, T y);
        int GetHashCode(T obj);
    }
}