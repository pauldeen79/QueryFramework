namespace ExpressionFramework.Core.Functions;

public record TrimFunction : IExpressionFunction
{
    public TrimFunction(IExpressionFunction? innerFunction)
        => InnerFunction = innerFunction;

    public IExpressionFunction? InnerFunction { get; }

    public IExpressionFunctionBuilder ToBuilder()
        => new TrimFunctionBuilder().WithInnerFunction(InnerFunction?.ToBuilder());
}
