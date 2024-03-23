namespace QueryFramework.Core;

public static class ComposableEvaluatableBuilderHelper
{
    public static ComposableEvaluatableBuilder Create(string fieldName, OperatorBuilder @operator, object? value)
        => new ComposableEvaluatableBuilder()
            .WithLeftExpression(new FieldExpressionBuilder().WithExpression(new ContextExpressionBuilder()).WithFieldName(fieldName))
            .WithOperator(@operator)
            .WithRightExpression(new ConstantExpressionBuilder().WithValue(value));

    public static ComposableEvaluatableBuilder Create(ExpressionBuilder leftExpression, OperatorBuilder @operator, object? value)
        => new ComposableEvaluatableBuilder()
            .WithLeftExpression(leftExpression)
            .WithOperator(@operator)
            .WithRightExpression(new ConstantExpressionBuilder().WithValue(value));

    public static ComposableEvaluatableBuilder Create(string fieldName, OperatorBuilder @operator)
        => new ComposableEvaluatableBuilder()
            .WithLeftExpression(new FieldExpressionBuilder().WithExpression(new ContextExpressionBuilder()).WithFieldName(fieldName))
            .WithOperator(@operator)
            .WithRightExpression(new EmptyExpressionBuilder());

    public static ComposableEvaluatableBuilder Create(ExpressionBuilder leftExpression, OperatorBuilder @operator)
        => new ComposableEvaluatableBuilder()
            .WithLeftExpression(leftExpression)
            .WithOperator(@operator)
            .WithRightExpression(new EmptyExpressionBuilder());
}
