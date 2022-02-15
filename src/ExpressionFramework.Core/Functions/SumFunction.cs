namespace ExpressionFramework.Core.Functions;

public record SumFunction : IExpressionFunction
{
    public SumFunction(IExpression expression, IExpressionFunction? innerFunction)
    {
        Expression = expression;
        InnerFunction = innerFunction;
    }

    public IExpression Expression { get; }
    public IExpressionFunction? InnerFunction { get; }

    public IExpressionFunctionBuilder ToBuilder()
        => new SumFunctionBuilder().WithExpression(Expression.ToBuilder())
                                   .WithInnerFunction(InnerFunction?.ToBuilder());
}
