namespace QueryFramework.SqlServer.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddQueryFrameworkSqlServer(this IServiceCollection serviceCollection)
        => serviceCollection.AddQueryFrameworkSqlServer(new Action<IServiceCollection>(_ => { }));

    public static IServiceCollection AddQueryFrameworkSqlServer(this IServiceCollection serviceCollection,
                                                                Action<IServiceCollection> customConfigurationAction)
        => serviceCollection
            .AddCrossCuttingDataSql()
            .With(x =>
            {
                customConfigurationAction.Invoke(x);
                if (!x.Any(y => y.ImplementationType == typeof(QueryDatabaseCommandProvider)))
                {
                    x.AddSingleton<IDatabaseCommandProvider<IQuery>, QueryDatabaseCommandProvider>()
                     .AddSingleton<IContextDatabaseCommandProvider<IQuery>, QueryDatabaseCommandProvider>()
                     .AddSingleton<IPagedDatabaseCommandProvider<IQuery>, QueryPagedDatabaseCommandProvider>()
                     .AddSingleton<IContextPagedDatabaseCommandProvider<IQuery>, QueryPagedDatabaseCommandProvider>()
                     .AddSingleton<IQueryFieldInfoFactory, DefaultQueryFieldInfoFactory>()
                     .AddSingleton<IQueryFieldInfoProvider, DefaultQueryFieldInfoProvider>()
                     .AddSingleton<IDatabaseEntityRetrieverFactory, DefaultDatabaseEntityRetrieverFactory>()
                     .AddSingleton<ISqlExpressionEvaluator, DefaultSqlExpressionEvaluator>()
                     .AddSingleton<ISqlExpressionEvaluatorProvider, ConstantExpressionEvaluatorProvider>()
                     .AddSingleton<ISqlExpressionEvaluatorProvider, ContextExpressionEvaluatorProvider>()
                     .AddSingleton<ISqlExpressionEvaluatorProvider, DelegateExpressionEvaluatorProvider>()
                     .AddSingleton<ISqlExpressionEvaluatorProvider, EmptyExpressionEvaluatorProvider>()
                     .AddSingleton<ISqlExpressionEvaluatorProvider, FieldExpressionEvaluatorProvider>()
                     .AddSingleton<ISqlExpressionEvaluatorProvider, QueryParameterExpressionEvaluatorProvider>()
                     .AddSingleton<IQueryProcessor, DefaultQueryProcessor>()
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
            });
}
