namespace QueryFramework.SqlServer;

public class DefaultQueryFieldInfoFactory : IQueryFieldInfoFactory
{
    private readonly IEnumerable<IQueryFieldInfoProvider> _providers;

    public DefaultQueryFieldInfoFactory(IEnumerable<IQueryFieldInfoProvider> providers)
        => _providers = providers;

    public IQueryFieldInfo Create(IQuery query)
    {
        foreach (var provider in _providers)
        {
            var success = provider.TryCreate(query, out var result);
            if (success)
            {
                return result ?? throw new InvalidOperationException($"Query field info provider of type [{provider.GetType().FullName}] provided an empty result");
            }
        }

        throw new InvalidOperationException($"Query type [{query.GetType().FullName}] does not have a query field info provider");
    }
}
