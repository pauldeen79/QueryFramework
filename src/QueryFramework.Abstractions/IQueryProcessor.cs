namespace QueryFramework.Abstractions;

public interface IQueryProcessor
{
    TResult? FindOne<TResult>(IQuery query)
        where TResult : class;
    IReadOnlyCollection<TResult> FindMany<TResult>(IQuery query)
        where TResult : class;
    IPagedResult<TResult> FindPaged<TResult>(IQuery query)
        where TResult : class;

    Task<TResult?> FindOneAsync<TResult>(IQuery query, CancellationToken cancellationToken)
        where TResult : class;
    Task<IReadOnlyCollection<TResult>> FindManyAsync<TResult>(IQuery query, CancellationToken cancellationToken)
        where TResult : class;
    Task<IPagedResult<TResult>> FindPagedAsync<TResult>(IQuery query, CancellationToken cancellationToken)
        where TResult : class;
}
