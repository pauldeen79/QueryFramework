namespace QueryFramework.Abstractions.Extensions;

public static class QueryProcessorExtensions
{
    public static Task<TResult?> FindOneAsync<TResult>(this IQueryProcessor processor, IQuery query)
        where TResult : class
        => processor.FindOneAsync<TResult>(query, CancellationToken.None);

    public static Task<IReadOnlyCollection<TResult>> FindManyAsync<TResult>(this IQueryProcessor processor, IQuery query)
        where TResult : class
        => processor.FindManyAsync<TResult>(query, CancellationToken.None);

    public static Task<IPagedResult<TResult>> FindPagedAsync<TResult>(this IQueryProcessor processor, IQuery query)
        where TResult : class
        => processor.FindPagedAsync<TResult>(query, CancellationToken.None);
}
