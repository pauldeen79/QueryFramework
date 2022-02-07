namespace QueryFramework.SqlServer.Abstractions;

public interface IPagedDatabaseCommandProviderFactory
{
    IPagedDatabaseCommandProvider Create<TResult>(ISingleEntityQuery query) where TResult : class;
}
