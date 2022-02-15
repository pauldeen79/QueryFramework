namespace ExpressionFramework.Core.Functions;

public record LeftFunction : IExpressionFunction
{
    public LeftFunction(int length, IExpression expression, IExpressionFunction? innerFunction)
    {
        Length = length;
        Expression = expression;
        InnerFunction = innerFunction;
    }

    public IExpression Expression { get; }
    public int Length { get; }
    public IExpressionFunction? InnerFunction { get; }

    public IExpressionFunctionBuilder ToBuilder()
        => new LeftFunctionBuilder().WithExpression(Expression.ToBuilder())
                                    .WithLength(Length)
                                    .WithInnerFunction(InnerFunction?.ToBuilder());
}
