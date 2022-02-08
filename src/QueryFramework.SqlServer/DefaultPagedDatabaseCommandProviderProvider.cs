namespace QueryFramework.SqlServer;

public class DefaultPagedDatabaseCommandProviderProvider<TQuery> : IPagedDatabaseCommandProviderProvider
    where TQuery : ISingleEntityQuery
{
    private readonly IPagedDatabaseCommandProvider<ISingleEntityQuery> _pagedDatabaseCommandProvider;

    public DefaultPagedDatabaseCommandProviderProvider(IPagedDatabaseCommandProvider<ISingleEntityQuery> pagedDatabaseCommandProvider)
        => _pagedDatabaseCommandProvider = pagedDatabaseCommandProvider;

    public bool TryCreate(ISingleEntityQuery query, out IPagedDatabaseCommandProvider<ISingleEntityQuery>? result)
    {
        if (query is TQuery)
        {
            result = _pagedDatabaseCommandProvider;
            return true;
        }

        result = default;
        return false;
    }
}
