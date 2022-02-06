namespace QueryFramework.SqlServer.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddQueryFrameworkSqlServer(this IServiceCollection serviceCollection)
        => serviceCollection
            .AddSingleton<IQueryFieldProvider, DefaultQueryFieldProvider>()
            .AddSingleton<IQueryExpressionEvaluator, DefaultQueryExpressionEvaluator>()
            .AddSingleton<IQueryProcessor, QueryProcessor>()
            .AddSingleton<IFunctionParser, CoalesceFunctionParser>()
            .AddSingleton<IFunctionParser, CountFunctionFunctionParser>()
            .AddSingleton<IFunctionParser, DayFunctionFunctionParser>()
            .AddSingleton<IFunctionParser, LeftFunctionParser>()
            .AddSingleton<IFunctionParser, LengthFunctionParser>()
            .AddSingleton<IFunctionParser, LowerFunctionParser>()
            .AddSingleton<IFunctionParser, MonthFunctionParser>()
            .AddSingleton<IFunctionParser, RightFunctionParser>()
            .AddSingleton<IFunctionParser, SumFunctionParser>()
            .AddSingleton<IFunctionParser, TrimFunctionParser>()
            .AddSingleton<IFunctionParser, UpperFunctionParser>()
            .AddSingleton<IFunctionParser, YearFunctionParser>();

    public static IServiceCollection AddQueryFrameworkSqlServer<TQuery>(this IServiceCollection serviceCollection)
        where TQuery : ISingleEntityQuery
        => serviceCollection
            .AddSingleton<IDatabaseCommandProvider<TQuery>, QueryDatabaseCommandProvider<TQuery>>()
            .AddSingleton<IPagedDatabaseCommandProvider<TQuery>, QueryPagedDatabaseCommandProvider<TQuery>>();
}
