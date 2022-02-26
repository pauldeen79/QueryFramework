namespace QueryFramework.SqlServer.Tests.TestHelpers;

public class PagedDatabaseEntityRetrieverSettingsProviderMock : IPagedDatabaseEntityRetrieverSettingsProvider
{
    public bool ReturnValue { get; set; }
    public Func<Type, IPagedDatabaseEntityRetrieverSettings?> ResultDelegate { get; set; }
        = new Func<Type, IPagedDatabaseEntityRetrieverSettings?>(_ => default(IPagedDatabaseEntityRetrieverSettings));

    public bool TryGet<TSource>(out IPagedDatabaseEntityRetrieverSettings? settings)
    {
        settings = ResultDelegate.Invoke(typeof(TSource));
        return ReturnValue;
    }
}
