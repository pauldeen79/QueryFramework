namespace QueryFramework.Abstractions.Extensions;

public static class ContextQueryProcessorExtensions
{
    public static Task<TResult?> FindOneAsync<TResult>(this IContextQueryProcessor processor, IQuery query, object? context)
        where TResult : class
        => processor.FindOneAsync<TResult>(query, context, CancellationToken.None);

    public static Task<IReadOnlyCollection<TResult>> FindManyAsync<TResult>(this IContextQueryProcessor processor, IQuery query, object? context)
        where TResult : class
        => processor.FindManyAsync<TResult>(query, context, CancellationToken.None);

    public static Task<IPagedResult<TResult>> FindPagedAsync<TResult>(this IContextQueryProcessor processor, IQuery query, object? context)
        where TResult : class
        => processor.FindPagedAsync<TResult>(query, context, CancellationToken.None);
}
