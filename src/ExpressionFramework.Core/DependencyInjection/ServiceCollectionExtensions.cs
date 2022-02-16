namespace ExpressionFramework.Core.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddExpressionFramework(this IServiceCollection serviceCollection)
        => serviceCollection.AddExpressionFramework(new Action<IServiceCollection>(_ => { }));

    public static IServiceCollection AddExpressionFramework(this IServiceCollection services,
                                                            Action<IServiceCollection> customConfigurationAction)
        => services
            .Chain(customConfigurationAction.Invoke)
            .AddSingleton<IExpressionEvaluatorCallback, ExpressionEvaluatorCallback>()
            .AddSingleton<IExpressionEvaluator, ConstantExpressionEvaluator>()
            .AddSingleton<IExpressionEvaluator, DelegateExpressionEvaluator>()
            .AddSingleton<IExpressionEvaluator, EmptyExpressionEvaluator>()
            .AddSingleton<IExpressionEvaluator, FieldExpressionEvaluator>()
            .AddSingleton<IValueProvider, ValueProvider>()
            .AddSingleton<IFunctionEvaluator, CoalesceFunctionEvaluator>()
            .AddSingleton<IFunctionEvaluator, ConditionFunctionEvaluator>()
            .AddSingleton<IFunctionEvaluator, CountFunctionEvaluator>()
            .AddSingleton<IFunctionEvaluator, DayFunctionEvaluator>()
            .AddSingleton<IFunctionEvaluator, LeftFunctionEvaluator>()
            .AddSingleton<IFunctionEvaluator, LengthFunctionEvaluator>()
            .AddSingleton<IFunctionEvaluator, LowerFunctionEvaluator>()
            .AddSingleton<IFunctionEvaluator, MonthFunctionEvaluator>()
            .AddSingleton<IFunctionEvaluator, RightFunctionEvaluator>()
            .AddSingleton<IFunctionEvaluator, SumFunctionEvaluator>()
            .AddSingleton<IFunctionEvaluator, TrimFunctionEvaluator>()
            .AddSingleton<IFunctionEvaluator, UpperFunctionEvaluator>()
            .AddSingleton<IFunctionEvaluator, YearFunctionEvaluator>();
}
