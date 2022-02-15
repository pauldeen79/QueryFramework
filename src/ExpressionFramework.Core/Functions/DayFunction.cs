namespace ExpressionFramework.Core.Functions;

public record DayFunction : IExpressionFunction
{
    public DayFunction(IExpression expression, IExpressionFunction? innerFunction)
    {
        Expression = expression;
        InnerFunction = innerFunction;
    }

    public IExpression Expression { get; }
    public IExpressionFunction? InnerFunction { get; }

    public IExpressionFunctionBuilder ToBuilder()
        => new DayFunctionBuilder().WithExpression(Expression.ToBuilder())
                                   .WithInnerFunction(InnerFunction?.ToBuilder());
}
