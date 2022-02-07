namespace QueryFramework.SqlServer.Abstractions;

public interface IPagedDatabaseEntityRetrieverSettingsFactory
{
    IPagedDatabaseEntityRetrieverSettings Create(ISingleEntityQuery query);
}
