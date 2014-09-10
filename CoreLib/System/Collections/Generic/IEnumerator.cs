namespace System.Collections.Generic
{
    using System;

    public interface IEnumerator<T> : IDisposable, IEnumerator
    {
        new T Current
        {
            get;
        }
    }
}
