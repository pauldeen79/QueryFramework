namespace QueryFramework.Abstractions;

public interface IQueryProcessor
{
    TResult? FindOne<TResult>(IQuery query)
        where TResult : class;
    IReadOnlyCollection<TResult> FindMany<TResult>(IQuery query)
        where TResult : class;
    IPagedResult<TResult> FindPaged<TResult>(IQuery query)
        where TResult : class;
}
