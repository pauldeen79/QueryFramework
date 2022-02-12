namespace ExpressionFramework.Core.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddExpressionFramework(this IServiceCollection services)
        => services.AddSingleton<IConditionEvaluator, ConditionEvaluator>()
                   .AddSingleton<IExpressionEvaluator, ExpressionEvaluator>()
                   .AddSingleton<IValueProvider, ValueProvider>()
                   .AddSingleton<IFunctionEvaluator, LengthFunctionEvaluator>()
                   .AddSingleton<IFunctionEvaluator, LeftFunctionEvaluator>()
                   .AddSingleton<IFunctionEvaluator, RightFunctionEvaluator>()
                   .AddSingleton<IFunctionEvaluator, UpperFunctionEvaluator>()
                   .AddSingleton<IFunctionEvaluator, LowerFunctionEvaluator>()
                   .AddSingleton<IFunctionEvaluator, TrimFunctionEvaluator>();
}
