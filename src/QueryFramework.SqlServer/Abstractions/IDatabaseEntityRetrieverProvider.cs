namespace QueryFramework.SqlServer.Abstractions;

public interface IDatabaseEntityRetrieverProvider
{
    bool TryCreate<TResult>(out IDatabaseEntityRetriever<TResult>? result) where TResult : class;
}
