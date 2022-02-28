namespace QueryFramework.FileSystemSearch.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddQueryFrameworkFileSystemSearch(this IServiceCollection serviceCollection)
        => serviceCollection
            .AddQueryFrameworkInMemory()
            .Chain(x =>
            {
                if (!x.Any(y => y.ImplementationType == typeof(DefaultFileDataProvider)))
                {
                    x.AddSingleton<IDataProvider, DefaultFileDataProvider>();
                }
                x.TryAddSingleton<IFileDataProvider, FileSystemFileDataProvider>();
            });
}
