namespace QueryFramework.Core.Expressions;

public sealed class CastExpressionBuilder<T> : ITypedExpressionBuilder<T>
{
    private readonly ExpressionBuilder _builder;

    public CastExpressionBuilder(ExpressionBuilder builder) => _builder = builder;

    public ITypedExpression<T> Build() => new CastExpression<T>(_builder.Build());
}
