namespace QueryFramework.InMemory;

public class DefaultDataFactory : IDataFactory
{
    private readonly IEnumerable<IDataProvider> _providers;

    public DefaultDataFactory(IEnumerable<IDataProvider> providers)
        => _providers = providers;

    public IEnumerable<TResult> GetData<TResult>(ISingleEntityQuery query) where TResult : class
    {
        foreach (var provider in _providers)
        {
            var success = provider.TryGetData<TResult>(query, out var result);
            if (success)
            {
                return result ?? throw new InvalidOperationException($"Data provider of type [{provider.GetType().FullName}] for data type [{typeof(TResult).FullName}] provided an empty result");
            }
        }

        throw new InvalidOperationException($"Query type [{query.GetType().FullName}] for data type [{typeof(TResult).FullName}] does not have a data provider");
    }
}
