namespace QueryFramework.SqlServer.Tests.Repositories;

public class TestEntityQueryProcessorSettingsProvider : IPagedDatabaseEntityRetrieverSettingsProvider
{
    public bool TryCreate(ISingleEntityQuery query, out IPagedDatabaseEntityRetrieverSettings? result)
    {
        if (query is TestQuery)
        {
            result = new TestEntityQueryProcessorSettings();
            return true;
        }

        result = default;
        return false;
    }
}
