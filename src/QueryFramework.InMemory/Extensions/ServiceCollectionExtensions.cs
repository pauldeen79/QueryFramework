namespace QueryFramework.InMemory.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddQueryFrameworkInMemory(this IServiceCollection serviceCollection)
        => serviceCollection
            .AddSingleton<IExpressionEvaluator, DefaultExpressionEvaluator>()
            .AddSingleton<IValueProvider, DefaultValueProvider>()
            .AddSingleton<IConditionEvaluator, DefaultConditionEvaluator>()
            .AddSingleton<IPaginator, DefaultPaginator>()
            .AddSingleton<IDataFactory, DefaultDataFactory>()
            .AddSingleton<IQueryProcessor, QueryProcessor>()
            .AddSingleton<IFunctionParser, LengthFunctionParser>()
            .AddSingleton<IFunctionParser, LeftFunctionParser>()
            .AddSingleton<IFunctionParser, RightFunctionParser>()
            .AddSingleton<IFunctionParser, UpperFunctionParser>()
            .AddSingleton<IFunctionParser, LowerFunctionParser>()
            .AddSingleton<IFunctionParser, TrimFunctionParser>();
}
