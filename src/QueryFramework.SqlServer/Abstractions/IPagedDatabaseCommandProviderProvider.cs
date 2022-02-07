namespace QueryFramework.SqlServer.Abstractions;

public interface IPagedDatabaseCommandProviderProvider
{
    bool TryCreate(ISingleEntityQuery query, out IPagedDatabaseCommandProvider<ISingleEntityQuery>? result);
}
