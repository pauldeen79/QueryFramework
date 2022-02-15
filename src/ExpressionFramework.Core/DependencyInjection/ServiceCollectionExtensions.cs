namespace ExpressionFramework.Core.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddExpressionFramework(this IServiceCollection serviceCollection)
        => serviceCollection.AddExpressionFramework(new Action<IServiceCollection>(_ => { }));

    public static IServiceCollection AddExpressionFramework(this IServiceCollection services,
                                                            Action<IServiceCollection> customConfigurationAction)
        => services
            .Chain(customConfigurationAction.Invoke)
            .AddSingleton<IConditionEvaluator, ConditionEvaluator>()
            .AddSingleton<IExpressionEvaluatorCallback, ExpressionEvaluatorCallback>()
            .AddSingleton<IExpressionEvaluator, FieldExpressionEvaluator>()
            .AddSingleton<IExpressionEvaluator, ConstantExpressionEvaluator>()
            .AddSingleton<IExpressionEvaluator, EmptyExpressionEvaluator>()
            .AddSingleton<IValueProvider, ValueProvider>()
            .AddSingleton<IFunctionEvaluator, CoalesceFunctionEvaluator>()
            .AddSingleton<IFunctionEvaluator, CountFunctionEvaluator>()
            .AddSingleton<IFunctionEvaluator, LeftFunctionEvaluator>()
            .AddSingleton<IFunctionEvaluator, LengthFunctionEvaluator>()
            .AddSingleton<IFunctionEvaluator, LowerFunctionEvaluator>()
            .AddSingleton<IFunctionEvaluator, RightFunctionEvaluator>()
            .AddSingleton<IFunctionEvaluator, TrimFunctionEvaluator>()
            .AddSingleton<IFunctionEvaluator, UpperFunctionEvaluator>();
}
