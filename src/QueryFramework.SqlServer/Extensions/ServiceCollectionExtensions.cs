namespace QueryFramework.SqlServer.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddQueryFrameworkSqlServer(this IServiceCollection serviceCollection)
        => serviceCollection.AddQueryFrameworkSqlServer(new Action<IServiceCollection>(_ => { }));

    public static IServiceCollection AddQueryFrameworkSqlServer(this IServiceCollection serviceCollection,
                                                                Action<IServiceCollection> customConfigurationAction)
        => serviceCollection
            .Chain(customConfigurationAction.Invoke)
            .AddSingleton<IDatabaseCommandProvider<ISingleEntityQuery>, QueryDatabaseCommandProvider>()
            .AddSingleton<IPagedDatabaseCommandProvider<ISingleEntityQuery>, QueryPagedDatabaseCommandProvider>()
            .AddSingleton<IPagedDatabaseEntityRetrieverSettingsFactory, DefaultPagedDatabaseEntityRetrieverSettingsFactory>()
            .AddSingleton<IQueryFieldInfoFactory, DefaultQueryFieldInfoFactory>()
            .AddSingleton<IQueryFieldInfoProvider, DefaultQueryFieldInfoProvider>()
            .AddSingleton<IDatabaseEntityRetrieverFactory, DefaultDatabaseEntityRetrieverFactory>()
            .AddSingleton<IPagedDatabaseCommandProviderFactory, DefaultPagedDatabaseCommandProviderFactory>()
            .AddSingleton<IPagedDatabaseCommandProviderProvider, DefaultPagedDatabaseCommandProviderProvider>()
            .AddSingleton<ISqlExpressionEvaluator, DefaultSqlExpressionEvaluator>()
            .AddSingleton<ISqlExpressionEvaluatorProvider, FieldExpressionEvaluatorProvider>()
            .AddSingleton<ISqlExpressionEvaluatorProvider, ConstantExpressionEvaluatorProvider>()
            .AddSingleton<ISqlExpressionEvaluatorProvider, DelegateExpressionEvaluatorProvider>()
            .AddSingleton<ISqlExpressionEvaluatorProvider, EmptyExpressionEvaluatorProvider>()
            .AddSingleton<IQueryProcessor, DefaultQueryProcessor>()
            .AddSingleton<IFunctionParser, ConditionFunctionFunctionParser>()
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
