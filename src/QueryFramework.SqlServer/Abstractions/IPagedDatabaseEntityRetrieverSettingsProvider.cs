namespace QueryFramework.SqlServer.Abstractions;

public interface IPagedDatabaseEntityRetrieverSettingsProvider
{
    bool TryCreate(ISingleEntityQuery query, out IPagedDatabaseEntityRetrieverSettings? result);
}
