namespace QueryFramework.SqlServer.Tests.TestHelpers;

public class PagedDatabaseCommandProviderProviderMock : IPagedDatabaseCommandProviderProvider
{
    public bool ReturnValue { get; set; }
    public Func<ISingleEntityQuery, IPagedDatabaseCommandProvider<ISingleEntityQuery>?> ResultDelegate { get; set; }
        = new Func<ISingleEntityQuery, IPagedDatabaseCommandProvider<ISingleEntityQuery>?>(_ => default(IPagedDatabaseCommandProvider<ISingleEntityQuery>));

    public bool TryCreate(ISingleEntityQuery query, out IPagedDatabaseCommandProvider<ISingleEntityQuery>? result)
    {
        result = ResultDelegate.Invoke(query);
        return ReturnValue;
    }
}
