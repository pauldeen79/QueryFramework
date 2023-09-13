namespace QueryFramework.SqlServer.Abstractions;

public interface IDatabaseEntityRetrieverFactory
{
    IDatabaseEntityRetriever<TResult> Create<TResult>(IQuery query) where TResult : class;
}
