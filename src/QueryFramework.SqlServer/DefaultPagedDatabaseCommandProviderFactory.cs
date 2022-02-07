namespace QueryFramework.SqlServer;

public class DefaultPagedDatabaseCommandProviderFactory : IPagedDatabaseCommandProviderFactory
{
    private readonly IEnumerable<IPagedDatabaseCommandProviderProvider> _providers;

    public DefaultPagedDatabaseCommandProviderFactory(IEnumerable<IPagedDatabaseCommandProviderProvider> providers)
        => _providers = providers;

    public IPagedDatabaseCommandProvider<ISingleEntityQuery> Create(ISingleEntityQuery query)
    {
        foreach (var provider in _providers)
        {
            var success = provider.TryCreate(query, out var result);
            if (success)
            {
                return result ?? throw new InvalidOperationException($"Paged database command provider of query type [{query.GetType().FullName}] provided an empty result");
            }
        }

        throw new InvalidOperationException($"Query type [{query.GetType().FullName}] does not have a paged database command provider");
    }
}
