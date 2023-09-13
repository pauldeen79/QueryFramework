namespace QueryFramework.SqlServer.Abstractions;

public interface IDatabaseEntityRetrieverProvider
{
    bool TryCreate<TResult>(IQuery query, out IDatabaseEntityRetriever<TResult>? result) where TResult : class;
}
