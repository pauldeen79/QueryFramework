namespace QueryFramework.Abstractions;

public static class ComposableEvaluatableBuilderHelper
{
    public static ComposableEvaluatableBuilder Create(string fieldName, OperatorBuilder @operator, object? value, Combination? combination = null, bool startGroup = false, bool endGroup = false, ExpressionBuilder? expression = null)
        => new ComposableEvaluatableBuilder()
            .WithCombination(combination)
            .WithLeftExpression(expression ?? new FieldExpressionBuilder().WithExpression(new ContextExpressionBuilder()).WithFieldName(fieldName))
            .WithOperator(@operator)
            .WithRightExpression(new ConstantExpressionBuilder().WithValue(value))
            .WithStartGroup(startGroup)
            .WithEndGroup(endGroup);

    public static ComposableEvaluatableBuilder Create<T>(string fieldName, OperatorBuilder @operator, Func<T> valueDelegate, Combination? combination = null, bool startGroup = false, bool endGroup = false, ExpressionBuilder? expression = null)
    => new ComposableEvaluatableBuilder()
        .WithCombination(combination)
        .WithLeftExpression(expression ?? new FieldExpressionBuilder().WithExpression(new ContextExpressionBuilder()).WithFieldName(fieldName))
        .WithOperator(@operator)
        .WithRightExpression(new TypedDelegateExpressionBuilder<T>().WithValue(new Func<object?, T>(_ => valueDelegate())))
        .WithStartGroup(startGroup)
        .WithEndGroup(endGroup);

    public static ComposableEvaluatableBuilder Create(string fieldName, OperatorBuilder @operator, ExpressionBuilder rightExpression, Combination? combination = null, bool startGroup = false, bool endGroup = false, ExpressionBuilder? expression = null)
        => new ComposableEvaluatableBuilder()
            .WithCombination(combination)
            .WithLeftExpression(expression ?? new FieldExpressionBuilder().WithExpression(new ContextExpressionBuilder()).WithFieldName(fieldName))
            .WithOperator(@operator)
            .WithRightExpression(rightExpression)
            .WithStartGroup(startGroup)
            .WithEndGroup(endGroup);

    public static ComposableEvaluatableBuilder Create(ExpressionBuilder leftExpression, OperatorBuilder @operator, object? value, Combination? combination = null, bool startGroup = false, bool endGroup = false)
        => new ComposableEvaluatableBuilder()
            .WithCombination(combination)
            .WithLeftExpression(leftExpression)
            .WithOperator(@operator)
            .WithRightExpression(new ConstantExpressionBuilder().WithValue(value))
            .WithStartGroup(startGroup)
            .WithEndGroup(endGroup);

    public static ComposableEvaluatableBuilder Create<T>(ExpressionBuilder leftExpression, OperatorBuilder @operator, Func<T> valueDelegate, Combination? combination = null, bool startGroup = false, bool endGroup = false)
    => new ComposableEvaluatableBuilder()
        .WithCombination(combination)
        .WithLeftExpression(leftExpression)
        .WithOperator(@operator)
        .WithRightExpression(new TypedDelegateExpressionBuilder<T>().WithValue(new Func<object?, T>(_ => valueDelegate())))
        .WithStartGroup(startGroup)
        .WithEndGroup(endGroup);

    public static ComposableEvaluatableBuilder Create(ExpressionBuilder leftExpression, OperatorBuilder @operator, ExpressionBuilder rightExpression, Combination? combination = null, bool startGroup = false, bool endGroup = false)
        => new ComposableEvaluatableBuilder()
            .WithCombination(combination)
            .WithLeftExpression(leftExpression)
            .WithOperator(@operator)
            .WithRightExpression(rightExpression)
            .WithStartGroup(startGroup)
            .WithEndGroup(endGroup);

    public static ComposableEvaluatableBuilder Create(string fieldName, OperatorBuilder @operator, Combination? combination = null, bool startGroup = false, bool endGroup = false, ExpressionBuilder? expression = null)
        => new ComposableEvaluatableBuilder()
            .WithCombination(combination)
            .WithLeftExpression(expression ?? new FieldExpressionBuilder().WithExpression(new ContextExpressionBuilder()).WithFieldName(fieldName))
            .WithOperator(@operator)
            .WithRightExpression(new EmptyExpressionBuilder())
            .WithStartGroup(startGroup)
            .WithEndGroup(endGroup);

    public static ComposableEvaluatableBuilder Create(ExpressionBuilder leftExpression, OperatorBuilder @operator, Combination? combination = null, bool startGroup = false, bool endGroup = false)
        => new ComposableEvaluatableBuilder()
            .WithCombination(combination)
            .WithLeftExpression(leftExpression)
            .WithOperator(@operator)
            .WithRightExpression(new EmptyExpressionBuilder())
            .WithStartGroup(startGroup)
            .WithEndGroup(endGroup);
}
