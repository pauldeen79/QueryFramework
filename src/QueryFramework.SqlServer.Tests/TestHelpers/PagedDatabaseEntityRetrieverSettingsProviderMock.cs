namespace QueryFramework.SqlServer.Tests.TestHelpers;

public class PagedDatabaseEntityRetrieverSettingsProviderMock : IPagedDatabaseEntityRetrieverSettingsProvider
{
    public bool ReturnValue { get; set; }
    public Func<ISingleEntityQuery, IPagedDatabaseEntityRetrieverSettings?> ResultDelegate { get; set; }
        = new Func<ISingleEntityQuery, IPagedDatabaseEntityRetrieverSettings?>(_ => default(IPagedDatabaseEntityRetrieverSettings));

    public bool TryCreate(ISingleEntityQuery query, out IPagedDatabaseEntityRetrieverSettings? result)
    {
        result = ResultDelegate.Invoke(query);
        return ReturnValue;
    }
}
