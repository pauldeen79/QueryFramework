namespace QueryFramework.Abstractions;

public interface IContextQueryProcessor : IQueryProcessor
{
    TResult? FindOne<TResult>(ISingleEntityQuery query, object? context)
        where TResult : class;
    IReadOnlyCollection<TResult> FindMany<TResult>(ISingleEntityQuery query, object? context)
        where TResult : class;
    IPagedResult<TResult> FindPaged<TResult>(ISingleEntityQuery query, object? context)
        where TResult : class;
}
