namespace QueryFramework.SqlServer.Abstractions;

public interface IPagedDatabaseCommandProviderFactory
{
    IPagedDatabaseCommandProvider<ISingleEntityQuery> Create(ISingleEntityQuery query);
}
