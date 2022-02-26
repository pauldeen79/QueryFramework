namespace QueryFramework.SqlServer.Tests.Repositories;

public class TestEntityQueryProcessorSettingsProvider : IPagedDatabaseEntityRetrieverSettingsProvider
{
    public bool TryGet<TSource>(out IPagedDatabaseEntityRetrieverSettings? settings)
    {
        if (typeof(TSource) == typeof(TestQuery))
        {
            settings = new TestEntityQueryProcessorSettings();
            return true;
        }

        settings = default;
        return false;
    }
}
