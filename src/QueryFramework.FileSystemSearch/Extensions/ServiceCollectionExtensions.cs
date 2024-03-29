﻿namespace QueryFramework.FileSystemSearch.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddQueryFrameworkFileSystemSearch(this IServiceCollection serviceCollection)
        => serviceCollection
            .AddQueryFrameworkInMemory()
            .With(x =>
            {
                if (!x.Any(y => y.ImplementationType == typeof(DefaultFileDataProvider)))
                {
                    x.AddSingleton<IDataProvider, DefaultFileDataProvider>();
                    x.AddSingleton<IContextDataProvider, DefaultFileDataProvider>();
                }
                x.TryAddSingleton<IFileDataProvider, FileSystemFileDataProvider>();
            });
}
