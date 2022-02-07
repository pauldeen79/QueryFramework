namespace QueryFramework.SqlServer.Abstractions;

public interface IDatabaseEntityRetrieverFactory
{
    IDatabaseEntityRetriever<TResult> Create<TResult>(ISingleEntityQuery query) where TResult : class;
}
