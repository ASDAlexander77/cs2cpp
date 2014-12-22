////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Apache License 2.0 (Apache)
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
namespace System
{
    public delegate void Action();
    public delegate void Action<T1>(T1 arg1);
    public delegate void Action<T1, T2>(T1 arg1, T2 arg2);
    public delegate void Action<T1, T2, T3>(T1 arg1, T2 arg2, T3 arg3);
    public delegate void Action<T1, T2, T3, T4>(T1 arg1, T2 arg2, T3 arg3, T4 arg4);

    public delegate TResult Func<TResult>();
    public delegate TResult Func<T1, TResult>(T1 arg1);
    public delegate TResult Func<T1, T2, TResult>(T1 arg1, T2 arg2);
    public delegate TResult Func<T1, T2, T3, TResult>(T1 arg1, T2 arg2, T3 arg3);
    public delegate TResult Func<T1, T2, T3, T4, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4);

    public delegate int Comparison<T>(T x, T y);

    public delegate TOutput Converter<TInput, TOutput>(TInput input);

    public delegate bool Predicate<T>(T obj); 
}


