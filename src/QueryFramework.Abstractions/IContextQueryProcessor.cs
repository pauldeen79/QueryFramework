namespace QueryFramework.Abstractions;

public interface IContextQueryProcessor : IQueryProcessor
{
    TResult? FindOne<TResult>(IQuery query, object? context)
        where TResult : class;
    IReadOnlyCollection<TResult> FindMany<TResult>(IQuery query, object? context)
        where TResult : class;
    IPagedResult<TResult> FindPaged<TResult>(IQuery query, object? context)
        where TResult : class;
}
