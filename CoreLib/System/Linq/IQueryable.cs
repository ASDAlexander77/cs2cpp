using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace System.Linq
{
    public interface IQueryable : IEnumerable
    {
        Expression Expression { get; }

        Type ElementType { get; }

        IQueryProvider Provider { get; }
    }

    public interface IQueryable<T> : IEnumerable<T>, IQueryable
    {
    }

    public interface IQueryProvider
    {
        IQueryable CreateQuery(Expression expression);

        IQueryable<TElement> CreateQuery<TElement>(Expression expression);

        object Execute(Expression expression);

        TResult Execute<TResult>(Expression expression);
    }

    public interface IOrderedQueryable : IQueryable
    {
    }

    public interface IOrderedQueryable<T> : IQueryable<T>, IOrderedQueryable
    {
    }
}
