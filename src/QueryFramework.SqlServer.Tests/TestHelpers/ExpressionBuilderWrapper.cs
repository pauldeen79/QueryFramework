namespace QueryFramework.SqlServer.Tests.TestHelpers;

internal sealed class ExpressionBuilderWrapper<T> : ExpressionBuilder
{
    private readonly ITypedExpressionBuilder<T> _expression;

    public ExpressionBuilderWrapper(ITypedExpressionBuilder<T> expression) => _expression = expression;

    public override Expression Build() => _expression.Build().ToUntyped();
}
