namespace QueryFramework.SqlServer.Abstractions;

public interface IPagedDatabaseCommandProviderProvider
{
    bool TryCreate<TResult>(ISingleEntityQuery query, out IPagedDatabaseCommandProvider? result) where TResult : class;
}
