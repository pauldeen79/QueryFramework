namespace ExpressionFramework.Core.Functions;

public record MonthFunction : IExpressionFunction
{
    public MonthFunction(IExpression expression, IExpressionFunction? innerFunction)
    {
        Expression = expression;
        InnerFunction = innerFunction;
    }

    public IExpression Expression { get; }
    public IExpressionFunction? InnerFunction { get; }

    public IExpressionFunctionBuilder ToBuilder()
        => new MonthFunctionBuilder().WithExpression(Expression.ToBuilder())
                                     .WithInnerFunction(InnerFunction?.ToBuilder());
}
