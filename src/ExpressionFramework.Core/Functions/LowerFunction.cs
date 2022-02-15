namespace ExpressionFramework.Core.Functions;

public record LowerFunction : IExpressionFunction
{
    public LowerFunction(IExpressionFunction? innerFunction)
        => InnerFunction = innerFunction;

    public IExpressionFunction? InnerFunction { get; }

    public IExpressionFunctionBuilder ToBuilder()
        => new LowerFunctionBuilder().WithInnerFunction(InnerFunction?.ToBuilder());
}
