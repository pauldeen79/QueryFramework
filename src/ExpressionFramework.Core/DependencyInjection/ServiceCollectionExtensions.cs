namespace ExpressionFramework.Core.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddExpressionFramework(this IServiceCollection services)
        => services.AddSingleton<IConditionEvaluator, ConditionEvaluator>()
                   .AddSingleton<IExpressionEvaluatorCallback, ExpressionEvaluatorCallback>()
                   .AddSingleton<IExpressionEvaluator, FieldExpressionEvaluator>()
                   .AddSingleton<IExpressionEvaluator, ConstantExpressionEvaluator>()
                   .AddSingleton<IValueProvider, ValueProvider>()
                   .AddSingleton<IFunctionEvaluator, CoalesceFunctionEvaluator>()
                   .AddSingleton<IFunctionEvaluator, LengthFunctionEvaluator>()
                   .AddSingleton<IFunctionEvaluator, LeftFunctionEvaluator>()
                   .AddSingleton<IFunctionEvaluator, RightFunctionEvaluator>()
                   .AddSingleton<IFunctionEvaluator, UpperFunctionEvaluator>()
                   .AddSingleton<IFunctionEvaluator, LowerFunctionEvaluator>()
                   .AddSingleton<IFunctionEvaluator, TrimFunctionEvaluator>();
}
