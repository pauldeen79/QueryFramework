namespace QueryFramework.SqlServer.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddQueryFrameworkSqlServer(this IServiceCollection serviceCollection)
        => serviceCollection
            .AddSingleton<IDatabaseCommandProvider<ISingleEntityQuery>, QueryDatabaseCommandProvider>()
            .AddSingleton<IPagedDatabaseCommandProvider<ISingleEntityQuery>, QueryPagedDatabaseCommandProvider>()
            .AddSingleton<IPagedDatabaseEntityRetrieverSettingsFactory, DefaultPagedDatabaseEntityRetrieverSettingsFactory>()
            .AddSingleton<IQueryFieldInfoFactory, DefaultQueryFieldInfoFactory>()
            .AddSingleton<IDatabaseEntityRetrieverFactory, DefaultDatabaseEntityRetrieverFactory>()
            .AddSingleton<IPagedDatabaseCommandProviderFactory, DefaultPagedDatabaseCommandProviderFactory>()
            .AddSingleton<IQueryExpressionEvaluator, DefaultQueryExpressionEvaluator>()
            .AddSingleton<IQueryProcessor, DefaultQueryProcessor>()
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
}
