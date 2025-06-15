using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore.Query;

namespace DarwinCMS.UnitTests.Helpers
{
    /// <summary>
    /// Simulates EF Core asynchronous query behavior in unit tests.
    /// This class wraps a LINQ IQueryable and adds support for IAsyncEnumerable,
    /// enabling the use of ToListAsync, FirstOrDefaultAsync, etc., in unit tests
    /// without requiring a real database context.
    /// </summary>
    /// <typeparam name="T">The type of elements in the query.</typeparam>
    public class TestAsyncEnumerable<T> : EnumerableQuery<T>, IAsyncEnumerable<T>, IQueryable<T>
    {
        /// <summary>
        /// Creates a new TestAsyncEnumerable from a given enumerable list.
        /// </summary>
        public TestAsyncEnumerable(IEnumerable<T> enumerable) : base(enumerable) { }

        /// <summary>
        /// Creates a new TestAsyncEnumerable from an expression (e.g., a LINQ query expression).
        /// </summary>
        public TestAsyncEnumerable(Expression expression) : base(expression) { }

        /// <inheritdoc />
        public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        {
            return new TestAsyncEnumerator<T>(this.AsEnumerable().GetEnumerator());
        }

        /// <inheritdoc />
        IQueryProvider IQueryable.Provider => new TestAsyncQueryProvider<T>(this);
    }

    /// <summary>
    /// Provides an async enumerator that wraps a regular IEnumerator.
    /// Used to simulate async iteration over query results in tests.
    /// </summary>
    /// <typeparam name="T">The type of item being iterated.</typeparam>
    public class TestAsyncEnumerator<T> : IAsyncEnumerator<T>
    {
        private readonly IEnumerator<T> _inner;

        /// <summary>
        /// Initializes a new instance of TestAsyncEnumerator with an existing enumerator.
        /// </summary>
        public TestAsyncEnumerator(IEnumerator<T> inner)
        {
            _inner = inner;
        }

        /// <inheritdoc />
        public ValueTask DisposeAsync()
        {
            _inner.Dispose();
            return ValueTask.CompletedTask;
        }

        /// <inheritdoc />
        public ValueTask<bool> MoveNextAsync()
        {
            return ValueTask.FromResult(_inner.MoveNext());
        }

        /// <inheritdoc />
        public T Current => _inner.Current;
    }

    /// <summary>
    /// Provides an async-capable LINQ query provider.
    /// This enables EF Core async extensions (e.g., ToListAsync) to work in tests using LINQ expressions.
    /// </summary>
    /// <typeparam name="TEntity">The entity type returned by the query.</typeparam>
    public class TestAsyncQueryProvider<TEntity> : IAsyncQueryProvider
    {
        private readonly IQueryProvider _inner;

        /// <summary>
        /// Wraps a regular IQueryProvider with async capabilities.
        /// </summary>
        public TestAsyncQueryProvider(IQueryProvider inner)
        {
            _inner = inner;
        }

        /// <inheritdoc />
        public IQueryable CreateQuery(Expression expression)
        {
            return new TestAsyncEnumerable<TEntity>(expression);
        }

        /// <inheritdoc />
        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        {
            return new TestAsyncEnumerable<TElement>(expression);
        }

        /// <inheritdoc />
        public object? Execute(Expression expression)
        {
            return _inner.Execute(expression);
        }

        /// <inheritdoc />
        public TResult Execute<TResult>(Expression expression)
        {
            return _inner.Execute<TResult>(expression);
        }

        /// <inheritdoc />
        public TResult ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken = default)
        {
            return Execute<TResult>(expression);
        }
    }
}
