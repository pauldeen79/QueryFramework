namespace QueryFramework.SqlServer;

public class DefaultPagedDatabaseEntityRetrieverSettingsFactory : IPagedDatabaseEntityRetrieverSettingsFactory
{
    private readonly IEnumerable<IPagedDatabaseEntityRetrieverSettingsProvider> _providers;

    public DefaultPagedDatabaseEntityRetrieverSettingsFactory(IEnumerable<IPagedDatabaseEntityRetrieverSettingsProvider> providers)
        => _providers = providers;

    public IPagedDatabaseEntityRetrieverSettings Create(ISingleEntityQuery query)
    {
        foreach (var provider in _providers)
        {
            var success = provider.TryCreate(query, out var result);
            if (success)
            {
                return result ?? throw new InvalidOperationException($"Paged database entity retriever settings provider of type [{provider.GetType().FullName}] provided an empty result");
            }
        }
        
        throw new InvalidOperationException($"Query type [{query.GetType().FullName}] does not have a paged database entity retriever settings provider");
    }
}
