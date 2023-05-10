namespace QueryFramework.SqlServer.Tests.TestHelpers.Expressions;

public sealed class CastExpressionBuilder<T> : ITypedExpressionBuilder<T>
{
    private readonly Expression _expression;

    public CastExpressionBuilder(Expression expression) => _expression = expression;

    public ITypedExpression<T> Build() => new CastExpression<T>(_expression);
}
