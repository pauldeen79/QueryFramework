namespace QueryFramework.InMemory.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddQueryFrameworkInMemory(this IServiceCollection serviceCollection)
        => serviceCollection
            .AddSingleton<IExpressionEvaluator, DefaultExpressionEvaluator>()
            .AddSingleton<IValueProvider, DefaultValueProvider>()
            .AddSingleton<IConditionEvaluator, DefaultConditionEvaluator>()
            .AddSingleton<IPaginator, DefaultPaginator>()
            //TODO: Refactor QueryProcessor so source data delegate doesn't have to be injected. We need a new interface that is injected in the c'tor.
            //.AddSingleton<IQueryProcessor, QueryProcessor>()
            .AddSingleton<IFunctionParser, LengthFunctionParser>()
            .AddSingleton<IFunctionParser, LeftFunctionParser>()
            .AddSingleton<IFunctionParser, RightFunctionParser>()
            .AddSingleton<IFunctionParser, UpperFunctionParser>()
            .AddSingleton<IFunctionParser, LowerFunctionParser>()
            .AddSingleton<IFunctionParser, TrimFunctionParser>();
}
