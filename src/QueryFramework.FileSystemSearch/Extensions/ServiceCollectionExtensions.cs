namespace QueryFramework.FileSystemSearch.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddQueryFrameworkFileSystemSearch(this IServiceCollection serviceCollection)
        => serviceCollection
            .AddQueryFrameworkInMemory()
            .AddSingleton<IDataProvider, FileSystemDataProvider>();
}
