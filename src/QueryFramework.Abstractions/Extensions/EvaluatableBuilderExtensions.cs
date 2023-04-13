namespace QueryFramework.Abstractions.Extensions;

public static class EvaluatableBuilderExtensions
{
    public static ExpressionBuilder GetLeftExpression(this EvaluatableBuilder instance)
        => instance.TryGetLeftExpression() ?? throw new NotSupportedException($"EvaluatableBuilder type {instance.GetType().FullName} is not supported. Only SingleEvaluatableBuilder and ComposableEvaluatableBuilder are supported");

    public static ExpressionBuilder? TryGetLeftExpression(this EvaluatableBuilder instance)
        => instance switch
        {
            SingleEvaluatableBuilder single => single.LeftExpression,
            ComposableEvaluatableBuilder composable => composable.LeftExpression,
            _ => default
        };

    public static ExpressionBuilder GetRightExpression(this EvaluatableBuilder instance)
        => instance.TryGetRightExpression() ?? throw new NotSupportedException($"EvaluatableBuilder type {instance.GetType().FullName} is not supported. Only SingleEvaluatableBuilder and ComposableEvaluatableBuilder are supported");

    public static ExpressionBuilder? TryGetRightExpression(this EvaluatableBuilder instance)
        => instance switch
        {
            SingleEvaluatableBuilder single => single.RightExpression,
            ComposableEvaluatableBuilder composable => composable.RightExpression,
            _ => null
        };

    public static OperatorBuilder GetOperator(this EvaluatableBuilder instance)
        => instance.TryGetOperator() ?? throw new NotSupportedException($"EvaluatableBuilder type {instance.GetType().FullName} is not supported. Only SingleEvaluatableBuilder and ComposableEvaluatableBuilder are supported");

    public static OperatorBuilder? TryGetOperator(this EvaluatableBuilder instance)
        => instance switch
        {
            SingleEvaluatableBuilder single => single.Operator,
            ComposableEvaluatableBuilder composable => composable.Operator,
            _ => default
        };
}
