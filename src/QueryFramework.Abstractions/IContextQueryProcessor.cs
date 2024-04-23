namespace QueryFramework.Abstractions;

public interface IContextQueryProcessor : IQueryProcessor
{
    TResult? FindOne<TResult>(IQuery query, object? context)
        where TResult : class;
    IReadOnlyCollection<TResult> FindMany<TResult>(IQuery query, object? context)
        where TResult : class;
    IPagedResult<TResult> FindPaged<TResult>(IQuery query, object? context)
        where TResult : class;

    Task<TResult?> FindOneAsync<TResult>(IQuery query, object? context, CancellationToken cancellationToken)
        where TResult : class;
    Task<IReadOnlyCollection<TResult>> FindManyAsync<TResult>(IQuery query, object? context, CancellationToken cancellationToken)
        where TResult : class;
    Task<IPagedResult<TResult>> FindPagedAsync<TResult>(IQuery query, object? context, CancellationToken cancellationToken)
        where TResult : class;
}
