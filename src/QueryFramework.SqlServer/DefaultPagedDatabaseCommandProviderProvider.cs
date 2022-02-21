namespace QueryFramework.SqlServer;

public class DefaultPagedDatabaseCommandProviderProvider : IPagedDatabaseCommandProviderProvider
{
    private readonly IPagedDatabaseCommandProvider<ISingleEntityQuery> _pagedDatabaseCommandProvider;

    public DefaultPagedDatabaseCommandProviderProvider(IPagedDatabaseCommandProvider<ISingleEntityQuery> pagedDatabaseCommandProvider)
        => _pagedDatabaseCommandProvider = pagedDatabaseCommandProvider;

    public bool TryCreate(ISingleEntityQuery query, out IPagedDatabaseCommandProvider<ISingleEntityQuery>? result)
    {
        result = _pagedDatabaseCommandProvider;
        return true;
    }
}
