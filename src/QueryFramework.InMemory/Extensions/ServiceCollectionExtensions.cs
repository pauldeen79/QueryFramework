namespace QueryFramework.InMemory.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddQueryFrameworkInMemory<TQuery>(this IServiceCollection serviceCollection)
        where TQuery : ISingleEntityQuery
        => serviceCollection
            .AddSingleton<IExpressionEvaluator, DefaultExpressionEvaluator>()
            .AddSingleton<IValueProvider, DefaultValueProvider>()
            .AddSingleton<IFunctionParser, LengthFunctionParser>()
            .AddSingleton<IFunctionParser, LeftFunctionParser>()
            .AddSingleton<IFunctionParser, RightFunctionParser>()
            .AddSingleton<IFunctionParser, UpperFunctionParser>()
            .AddSingleton<IFunctionParser, LowerFunctionParser>()
            .AddSingleton<IFunctionParser, TrimFunctionParser>();
}
