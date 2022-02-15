namespace ExpressionFramework.Core.Functions;

public record CountFunction : IExpressionFunction
{
    public CountFunction(IExpression expression, IExpressionFunction? innerFunction)
    {
        Expression = expression;
        InnerFunction = innerFunction;
    }

    public IExpression Expression { get; }
    public IExpressionFunction? InnerFunction { get; }

    public IExpressionFunctionBuilder ToBuilder()
        => new CountFunctionBuilder().WithExpression(Expression.ToBuilder())
                                     .WithInnerFunction(InnerFunction?.ToBuilder());
}
