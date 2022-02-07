namespace QueryFramework.SqlServer.Tests.TestHelpers;

public class PagedDatabaseCommandProviderProviderMock : IPagedDatabaseCommandProviderProvider
{
    public bool ReturnValue { get; set; }
    public Func<ISingleEntityQuery, IPagedDatabaseCommandProvider?> ResultDelegate { get; set; }
        = new Func<ISingleEntityQuery, IPagedDatabaseCommandProvider?>(_ => default(IPagedDatabaseCommandProvider));

    public bool TryCreate<TResult>(ISingleEntityQuery query, out IPagedDatabaseCommandProvider? result) where TResult : class
    {
        result = ResultDelegate.Invoke(query);
        return ReturnValue;
    }
}
