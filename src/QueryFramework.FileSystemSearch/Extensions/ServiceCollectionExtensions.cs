namespace QueryFramework.FileSystemSearch.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddFileSystemSearch(this IServiceCollection serviceCollection)
        => serviceCollection
            .AddQueryFrameworkInMemory()
            .AddSingleton<IDataProvider, FileSystemDataProvider>();
}
