namespace QueryFramework.InMemory.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddQueryFrameworkInMemory(this IServiceCollection serviceCollection)
        => serviceCollection
            .AddSingleton<IPaginator, DefaultPaginator>()
            .AddSingleton<IDataFactory, DefaultDataFactory>()
            .AddSingleton<IQueryProcessor, QueryProcessor>();
}
