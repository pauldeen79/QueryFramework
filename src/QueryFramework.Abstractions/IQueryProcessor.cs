namespace QueryFramework.Abstractions;

public interface IQueryProcessor
{
    TResult? FindOne<TResult>(ISingleEntityQuery query)
        where TResult : class;
    IReadOnlyCollection<TResult> FindMany<TResult>(ISingleEntityQuery query)
        where TResult : class;
    IPagedResult<TResult> FindPaged<TResult>(ISingleEntityQuery query)
        where TResult : class;
}
