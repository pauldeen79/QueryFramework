namespace QueryFramework.SqlServer.Abstractions;

public interface IDatabaseEntityRetrieverProvider
{
    bool TryCreate<TResult>(ISingleEntityQuery query, out IDatabaseEntityRetriever<TResult>? result) where TResult : class;
}
