namespace QueryFramework.SqlServer.Abstractions;

public interface IDatabaseEntityRetrieverProvider
{
    IDatabaseEntityRetriever<TResult>? GetRetriever<TResult>() where TResult : class;
}
