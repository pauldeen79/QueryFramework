namespace QueryFramework.Core.Expressions;

public sealed record CastExpression<T> : Expression, ITypedExpression<T>
{
    public Expression SourceExpression { get; }

    public CastExpression(Expression sourceExpression) : base() => SourceExpression = sourceExpression;

    public override Result<object?> Evaluate(object? context) => SourceExpression.Evaluate(context);

    public Result<T> EvaluateTyped(object? context) => SourceExpression.Evaluate(context).TryCast<T>($"Expression result is not of type [{typeof(T).FullName}]");

    public Expression ToUntyped() => SourceExpression;
}
