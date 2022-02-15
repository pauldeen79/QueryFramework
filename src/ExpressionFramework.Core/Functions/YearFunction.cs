namespace ExpressionFramework.Core.Functions;

public record YearFunction : IExpressionFunction
{
    public YearFunction(IExpression expression, IExpressionFunction? innerFunction)
    {
        Expression = expression;
        InnerFunction = innerFunction;
    }

    public IExpression Expression { get; }
    public IExpressionFunction? InnerFunction { get; }

    public IExpressionFunctionBuilder ToBuilder()
        => new YearFunctionBuilder().WithExpression(Expression.ToBuilder())
                                    .WithInnerFunction(InnerFunction?.ToBuilder());
}
