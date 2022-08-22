namespace QueryFramework.InMemory.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddQueryFrameworkInMemory(this IServiceCollection serviceCollection)
        => serviceCollection
            .AddExpressionFramework()
            .With(x =>
            {
                x.TryAddSingleton<IPaginator, DefaultPaginator>();
                x.TryAddSingleton<IDataFactory, DefaultDataFactory>();
                x.TryAddSingleton<IQueryProcessor, QueryProcessor>();
            });
}
