namespace QueryFramework.InMemory;

public class DefaultDataFactory : IContextDataFactory
{
    private readonly IEnumerable<IDataProvider> _providers;
    private readonly IEnumerable<IContextDataProvider> _contextProviders;

    public DefaultDataFactory(IEnumerable<IDataProvider> providers, IEnumerable<IContextDataProvider> contextProviders)
    {
        _providers = providers;
        _contextProviders = contextProviders;
    }

    public IEnumerable<TResult> GetData<TResult>(IQuery query)
        where TResult : class
        => GetData<TResult>(query, default);

    public IEnumerable<TResult> GetData<TResult>(IQuery query, object? context)
        where TResult : class
    {
        foreach (var provider in _contextProviders)
        {
            var success = provider.TryGetData<TResult>(query, context, out var result);

            if (success)
            {
                return result ?? throw new InvalidOperationException($"Data provider of type [{provider.GetType().FullName}] for data type [{typeof(TResult).FullName}] provided an empty result");
            }
        }

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
