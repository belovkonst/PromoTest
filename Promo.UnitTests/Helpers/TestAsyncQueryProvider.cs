using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Promo.UnitTests.Helpers;

internal class TestAsyncQueryProvider<T> : IAsyncQueryProvider
{
    private readonly IQueryProvider _inner;

    internal TestAsyncQueryProvider(IQueryProvider inner) => _inner = inner;

    public IQueryable CreateQuery(Expression expression) => new TestAsyncEnumerable<T>(expression);

    public IQueryable<TElement> CreateQuery<TElement>(Expression expression) => new TestAsyncEnumerable<TElement>(expression);

    public object Execute(Expression expression) => _inner.Execute(expression);

    public TResult Execute<TResult>(Expression expression) => _inner.Execute<TResult>(expression);

    public IAsyncEnumerable<TResult> ExecuteAsync<TResult>(Expression expression) =>
        new TestAsyncEnumerable<TResult>(new EnumerableQuery<TResult>(new List<TResult>()));

    public IAsyncEnumerable<TResult> ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken)
    {
        return new TestAsyncEnumerable<TResult>(new EnumerableQuery<TResult>(new List<TResult>()));
    }

    TResult IAsyncQueryProvider.ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken)
    {
        var expectedResultType = typeof(TResult).GetGenericArguments()[0];
        var executionResult = typeof(IQueryProvider)
          .GetMethods()
          .First(method => method.Name == nameof(IQueryProvider.Execute) && method.IsGenericMethod)
          .MakeGenericMethod(expectedResultType)
          .Invoke(this, new object[] { expression });

        return (TResult)typeof(Task).GetMethod(nameof(Task.FromResult))
          .MakeGenericMethod(expectedResultType)
          .Invoke(null, new[] { executionResult });
    }
}
