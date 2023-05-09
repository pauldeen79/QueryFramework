namespace QueryFramework.SqlServer.Tests.TestHelpers;

internal sealed record CastExpression<T> : Expression, ITypedExpression<T>
{
    private readonly Expression _sourceExpression;

    public CastExpression(Expression sourceExpression) : base()
    {
        _sourceExpression = sourceExpression;
    }

    public override Result<object?> Evaluate(object? context)
        => _sourceExpression.Evaluate(context);

    public Result<T> EvaluateTyped(object? context)
        => _sourceExpression.Evaluate(context).TryCast<T>($"Expression result is not of type [{typeof(T).FullName}]");

    public Expression ToUntyped()
        => _sourceExpression;
}
