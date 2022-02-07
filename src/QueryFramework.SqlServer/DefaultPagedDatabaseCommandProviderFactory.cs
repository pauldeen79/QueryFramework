namespace QueryFramework.SqlServer;

public class DefaultPagedDatabaseCommandProviderFactory : IPagedDatabaseCommandProviderFactory
{
    private readonly IEnumerable<IPagedDatabaseCommandProviderProvider> _providers;

    public DefaultPagedDatabaseCommandProviderFactory(IEnumerable<IPagedDatabaseCommandProviderProvider> providers)
        => _providers = providers;

    public IPagedDatabaseCommandProvider Create<TResult>(ISingleEntityQuery query) where TResult : class
    {
        foreach (var provider in _providers)
        {
            var success = provider.TryCreate<TResult>(query, out var result);
            if (success)
            {
                return result ?? throw new InvalidOperationException($"Paged database command provider of data type [{typeof(TResult).FullName}] for query type [{query.GetType().FullName}] provided an empty result");
            }
        }

        throw new InvalidOperationException($"Data type [{typeof(TResult).FullName}] does not have a paged database command provider for query type [{query.GetType().FullName}]");
    }
}
