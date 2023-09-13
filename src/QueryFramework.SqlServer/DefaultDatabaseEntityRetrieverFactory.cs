namespace QueryFramework.SqlServer;

public class DefaultDatabaseEntityRetrieverFactory : IDatabaseEntityRetrieverFactory
{
    private readonly IEnumerable<IDatabaseEntityRetrieverProvider> _providers;

    public DefaultDatabaseEntityRetrieverFactory(IEnumerable<IDatabaseEntityRetrieverProvider> providers)
        => _providers = providers;

    public IDatabaseEntityRetriever<TResult> Create<TResult>(IQuery query) where TResult : class
    {
        foreach (var provider in _providers)
        {
            var success = provider.TryCreate<TResult>(query, out var result);
            if (success)
            {
                return result ?? throw new InvalidOperationException($"Database entity retriever provider of data type [{typeof(TResult).FullName}] for query type [{query.GetType().FullName}] provided an empty result");
            }
        }

        throw new InvalidOperationException($"Data type [{typeof(TResult).FullName}] does not have a database entity retriever provider for query type [{query.GetType().FullName}]");
    }
}
